using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum SymmetryType
{
    None,
    Regular,
    Diagonal
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private SymmetryType _mapSymmetry;
    [SerializeField] private int _rocksPerPlayer, _waterPerPlayer;
    [SerializeField] private GameObject _cellPrefab, _bondPrefab;

    public Cell[,] Cells;

    public static GameManager Instance;

    private void Awake()
    {
        Cells = new Cell[_width, _height];

        GenerateDirt();
        GenerateWater();
        GenerateRock();
    }

    private void GenerateDirt()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Cell cell = Instantiate(_cellPrefab, new Vector3(x + 0.5f, y + 0.5f, 0f), Quaternion.identity).GetComponent<Cell>();
                cell.transform.parent = this.transform;
                cell.Type = CellType.Dirt;
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
                    cell.BondRight = Instantiate(_bondPrefab, new Vector3(x + 1f, y + 0.5f, 0f), Quaternion.identity).GetComponent<Bond>();
                    cell.BondRight.transform.parent = cell.transform;
                    cell.BondRight.IsVertical = false;
                    cell.BondRight.Cell2 = cell;
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
                }
            }
        }
    }

    private void GenerateWater()
    {
        Cells[0, 0].Type = CellType.Water;
        Cells[_width - 1, 0].Type = CellType.Water;
        Cells[0, _height - 1].Type = CellType.Water;
        Cells[_width - 1, _height - 1].Type = CellType.Water;

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

                Cells[x, y].Type = CellType.Water;
                Cells[_width - x - 1, y].Type = CellType.Water;
                Cells[x, _height - y - 1].Type = CellType.Water;
                Cells[_width - x - 1, _height - y - 1].Type = CellType.Water;
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

                Cells[x, y].Type = CellType.Rock;
                Cells[_width - x - 1, y].Type = CellType.Rock;
                Cells[x, _height - y - 1].Type = CellType.Rock;
                Cells[_width - x - 1, _height - y - 1].Type = CellType.Rock;
            }
            break;

        case SymmetryType.Diagonal:
            break;

        default:
            break;
        }
    }
}
