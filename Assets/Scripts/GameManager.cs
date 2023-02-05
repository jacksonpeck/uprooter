using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public enum SymmetryType
{
    None,
    Regular,
    Diagonal
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int _width, _height;
    [SerializeField] private SymmetryType _mapSymmetry;
    [SerializeField] private int _rocksPerPlayer, _waterPerPlayer;
    [SerializeField] private GameObject _cellPrefab, _bondPrefab;
    [SerializeField] private GameObject _wallPrefab, _cornerPrefab;
    [SerializeField] private GameObject _playerPrefab;

    private bool isGameInProgress = false;
    public bool GameOver = false;
    public List<Player> connectedPlayers = new List<Player>();

    public Cell[,] Cells;
    public Bond[] Bonds;

    private void Awake()
    {
        GenerateLevel();
    }

    private void FixedUpdate()
    {
        if (isGameInProgress && !GameOver)
        {
            CullRoots();
            UpdateRoots();
            CheckPlayers();
        }
    }

    private void CheckPlayers()
    {
        List<Player> killPlayers = new List<Player>();
        foreach(Player player in connectedPlayers)
        {
            if (!player.occupiedCell.IsConnected(player.playerNum))
            {
                killPlayers.Add(player);
            }
        }
        foreach (Player player in killPlayers)
        {
            connectedPlayers.Remove(player);
            Destroy(player.gameObject);
        }
        if (connectedPlayers.Count <= 1)
        {
            SetGameOver();
        }
    }

    private void Start()
    {
        Instance = this;
    }

    public void PlayerJoined(Player newPlayer)
    {
        Vector2Int[] startingLocs = { new Vector2Int(0, 0), new Vector2Int(_width - 1, _height - 1), new Vector2Int(_width - 1, 0), new Vector2Int(0, _height - 1) };

        connectedPlayers.Add(newPlayer);
        newPlayer.SetPlayerNum(connectedPlayers.Count);
        Vector2Int selectedLoc = startingLocs[connectedPlayers.Count - 1];
        newPlayer.SetOccupiedCell(Cells[selectedLoc.x, selectedLoc.y]);

        if (connectedPlayers.Count >= SettingsStorage.numPlayers)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        isGameInProgress = true;
        PlayerInputManager.instance.DisableJoining();
    }

    private void EndGame()
    {
        isGameInProgress = false;
        PlayerInputManager.instance.EnableJoining();
    }

    private void GenerateLevel()
    {
        Cells = new Cell[_width, _height];
        Bonds = new Bond[2 * _width * _height - _width - _height];

        GenerateWalls();
        GenerateDirt();
        GenerateWater();
        GenerateRock();
        //GeneratePlayers();
    }

    public void RegenerateLevel()
    {
        foreach (Transform child in this.transform)
        {
             GameObject.Destroy(child.gameObject);
        }

        GenerateLevel();
    }

    private void GenerateWalls()
    {
        Instantiate(_cornerPrefab, new Vector3(0.5f, 0.5f, 0f), Quaternion.Euler(0f, 0f, 90f));
        Instantiate(_cornerPrefab, new Vector3(_width + 1.5f, 0.5f, 0f), Quaternion.Euler(0f, 0f, 180f));
        Instantiate(_cornerPrefab, new Vector3(0.5f, _height + 1.5f, 0f), Quaternion.identity);
        Instantiate(_cornerPrefab, new Vector3(_width + 1.5f, _height + 1.5f, 0f), Quaternion.Euler(0f, 0f, 270f));

        for (int x = 1; x < _width + 1; x++)
        {
            Instantiate(_wallPrefab, new Vector3(x + 0.5f, 0.5f, 0f), Quaternion.Euler(0f, 0f, 180f));
            Instantiate(_wallPrefab, new Vector3(x + 0.5f, _height + 1.5f, 0f), Quaternion.identity);
        }

        for (int y = 1; y < _height + 1; y++)
        {
            Instantiate(_wallPrefab, new Vector3(0.5f, y + 0.5f, 0f), Quaternion.Euler(0f, 0f, 90f));
            Instantiate(_wallPrefab, new Vector3(_width + 1.5f, y + 0.5f, 0f), Quaternion.Euler(0f, 0f, 270f));
        }
    }

    private void GenerateDirt()
    {
        int bondNumber = 0;
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Cell cell = Instantiate(_cellPrefab, new Vector3(x + 1.5f, y + 1.5f, 0f), Quaternion.identity).GetComponent<Cell>();
                cell.transform.parent = this.transform;
                cell.Location = new Vector2Int(x, y);

                Cells[x, y] = cell;

                if (x <= 0)
                {
                    cell.BondLeft = null;
                }
                else
                {
                    cell.BondLeft = Cells[x - 1, y].BondRight;
                    cell.BondLeft.Cell1 = cell;
                }

                if (x >= _width - 1)
                {
                    cell.BondRight = null;
                }
                else
                {
                    cell.BondRight = Instantiate(_bondPrefab, new Vector3(x + 2f, y + 1.5f, 0f), Quaternion.identity).GetComponent<Bond>();
                    cell.BondRight.transform.parent = cell.transform;
                    cell.BondRight.IsVertical = false;
                    cell.BondRight.Cell2 = cell;
                    Bonds[bondNumber] = cell.BondRight;
                    Bonds[bondNumber].index = bondNumber;
                    bondNumber++;
                }

                if (y <= 0)
                {
                    cell.BondDown = null;
                }
                else
                {
                    cell.BondDown = Cells[x, y - 1].BondUp;
                    cell.BondDown.Cell1 = cell;
                }

                if (y >= _height - 1)
                {
                    cell.BondUp = null;
                }
                else
                {
                    cell.BondUp = Instantiate(_bondPrefab, new Vector3(x + 0.5f, y + 1f, 0f), Quaternion.identity).GetComponent<Bond>();
                    cell.BondUp.transform.parent = cell.transform;
                    cell.BondUp.IsVertical = true;
                    cell.BondUp.Cell2 = cell;
                    Bonds[bondNumber] = cell.BondUp;
                    Bonds[bondNumber].index = bondNumber;
                    bondNumber++;
                }
            }
        }
    }

    private void GenerateWater()
    {
        Cells[0, 0].AddWater();
        Cells[_width - 1, 0].AddWater();
        Cells[0, _height - 1].AddWater();
        Cells[_width - 1, _height - 1].AddWater();

        switch (_mapSymmetry)
        {
            case SymmetryType.None:
                break;

            case SymmetryType.Regular:
                for (int i = 0; i < _waterPerPlayer; i++)
                {
                    int x;
                    int y;

                    while (true)
                    {
                        x = Random.Range(0, _width / 2 - 1);
                        y = Random.Range(0, _height / 2 - 1);

                        Cell cell = Cells[x, y];

                        if (cell.Type != CellType.Dirt) continue;

                        CellType leftType = x > 0 ? Cells[x - 1, y].Type : CellType.Rock;
                        CellType rightType = Cells[x + 1, y].Type;
                        CellType upType = Cells[x, y + 1].Type;
                        CellType downType = y > 0 ? Cells[x, y - 1].Type : CellType.Rock;

                        if (leftType == CellType.Water) continue;
                        if (rightType == CellType.Water) continue;
                        if (upType == CellType.Water) continue;
                        if (downType == CellType.Water) continue;

                        break;
                    }

                    Cells[x, y].AddWater();
                    Cells[_width - x - 1, y].AddWater();
                    Cells[x, _height - y - 1].AddWater();
                    Cells[_width - x - 1, _height - y - 1].AddWater();
                }
                break;

            case SymmetryType.Diagonal:
                break;

            default:
                break;
        }
    }

    private void GenerateRock()
    {
        switch (_mapSymmetry)
        {
            case SymmetryType.None:
                break;

            case SymmetryType.Regular:
                for (int i = 0; i < _rocksPerPlayer; i++)
                {
                    int x;
                    int y;

                    while (true)
                    {
                        x = Random.Range(0, _width / 2);
                        y = Random.Range(0, _height / 2);

                        Cell cell = Cells[x, y];

                        if (cell.Type != CellType.Dirt) continue;

                        // CellType leftType = x > 0 ? Cells[x - 1, y].Type : CellType.Rock;
                        // CellType rightType = Cells[x + 1, y].Type;
                        // CellType upType = Cells[x, y + 1].Type;
                        // CellType downType = y > 0 ? Cells[x, y - 1].Type : CellType.Rock;

                        break;
                    }

                    Cells[x, y].AddRock();
                    Cells[_width - x - 1, y].AddRock();
                    Cells[x, _height - y - 1].AddRock();
                    Cells[_width - x - 1, _height - y - 1].AddRock();
                }
                break;

            case SymmetryType.Diagonal:
                break;

            default:
                break;
        }
    }

    //private void GeneratePlayers()
    //{
    //    // THIS IS WHERE I WOULD READ THE NUMBER OF PLAYERS IN GAME, BUT AS I DO NOT KNOW IF THAT IS IN THIS BRANCH I'LL JUST LEAVE THIS MEMO HERE
    //    int numPlayers = 4;
    //    // ---------------------

    //    Vector2Int[] startingLocs = { new Vector2Int(0, 0), new Vector2Int(_width - 1, _height - 1), new Vector2Int(_width - 1, 0), new Vector2Int(0, _height - 1) };

    //    for (int i = 0; i < numPlayers; i++)
    //    {
    //        Player newPlayer = Instantiate(_playerPrefab).GetComponent<Player>();
    //        newPlayer.SetOccupiedCell(Cells[startingLocs[i].x, startingLocs[i].y]);
    //        newPlayer.SetPlayerNum(i + 1);
    //    }
    //}

    private void CullRoots()
    {
        bool[] bondSafe = new bool[Bonds.Length];

        foreach (Bond bond in Bonds)
        {
            if (bond.HasWater())
            {
                bondSafe[bond.index] = true;
                SaveRoots(bond, bondSafe);
            }
        }

        foreach (Bond bond in Bonds)
        {
            CullRoot(bond, bondSafe);
        }
    }

    private void SaveRoots(Bond bond, bool[] bondSafe)
    {
        bondSafe[bond.index] = true;

        Cell cell1 = bond.Cell1;
        Cell cell2 = bond.Cell2;

        if (cell1.BondLeft != null && cell1.BondLeft.Player == bond.Player && !bondSafe[cell1.BondLeft.index])
            SaveRoots(cell1.BondLeft, bondSafe);
        if (cell1.BondRight != null && cell1.BondRight.Player == bond.Player && !bondSafe[cell1.BondRight.index])
            SaveRoots(cell1.BondRight, bondSafe);
        if (cell1.BondUp != null && cell1.BondUp.Player == bond.Player && !bondSafe[cell1.BondUp.index])
            SaveRoots(cell1.BondUp, bondSafe);
        if (cell1.BondDown != null && cell1.BondDown.Player == bond.Player && !bondSafe[cell1.BondDown.index])
            SaveRoots(cell1.BondDown, bondSafe);

        if (cell2.BondLeft != null && cell2.BondLeft.Player == bond.Player && !bondSafe[cell2.BondLeft.index])
            SaveRoots(cell2.BondLeft, bondSafe);
        if (cell2.BondRight != null && cell2.BondRight.Player == bond.Player && !bondSafe[cell2.BondRight.index])
            SaveRoots(cell2.BondRight, bondSafe);
        if (cell2.BondUp != null && cell2.BondUp.Player == bond.Player && !bondSafe[cell2.BondUp.index])
            SaveRoots(cell2.BondUp, bondSafe);
        if (cell2.BondDown != null && cell2.BondDown.Player == bond.Player && !bondSafe[cell2.BondDown.index])
            SaveRoots(cell2.BondDown, bondSafe);
    }

    private void CullRoot(Bond bond, bool[] bondSafe)
    {
        if (!bondSafe[bond.index])
        {
            bond.Player = 0;
        }
    }

    // private bool CullRoot(Bond bond, bool[] bondSafe)
    // {
    //     if (bond == null || bond.Player == 0)
    //         return false;

    //     if (bond.HasWater() || bondSafe[bond.index])
    //         return (bondSafe[bond.index] = true);

    //     if (CullRoot(bond.Cell1.BondLeft, bondSafe))
    //         return (bondSafe[bond.index] = true);
    //     if (CullRoot(bond.Cell1.BondRight, bondSafe))
    //         return (bondSafe[bond.index] = true);
    //     if (CullRoot(bond.Cell1.BondUp, bondSafe))
    //         return (bondSafe[bond.index] = true);
    //     if (CullRoot(bond.Cell1.BondDown, bondSafe))
    //         return (bondSafe[bond.index] = true);

    //     if (CullRoot(bond.Cell2.BondLeft, bondSafe))
    //         return (bondSafe[bond.index] = true);
    //     if (CullRoot(bond.Cell2.BondRight, bondSafe))
    //         return (bondSafe[bond.index] = true);
    //     if (CullRoot(bond.Cell2.BondUp, bondSafe))
    //         return (bondSafe[bond.index] = true);
    //     if (CullRoot(bond.Cell2.BondDown, bondSafe))
    //         return (bondSafe[bond.index] = true);

    //     bond.Player = 0;

    //     return false;
    // }

    private void UpdateRoots()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Cells[x, y].UpdateRoot();
            }
        }
	}
    public bool GetIsGameInProgress()
    {
        return isGameInProgress;
    }

    public void SetGameOver()
    {
        GameOver = true;
    }
}
