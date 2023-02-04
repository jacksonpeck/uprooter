using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private GameObject _cellPrefab, _bondPrefab;

    private Cell[,] _cells;

    public static GameManager Instance;

    private void Awake()
    {
        GenerateCells();
    }

    private void GenerateCells()
    {
        _cells = new Cell[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Cell cell = Instantiate(_cellPrefab, new Vector3(x + 0.5f, y + 0.5f, 0f), Quaternion.identity).GetComponent<Cell>();
                cell.transform.parent = this.transform;

                _cells[x, y] = cell;

                if (x <= 0)
                {
                    cell.BondLeft = null;
                }
                else
                {
                    cell.BondLeft = _cells[x - 1, y].BondRight;
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
                    cell.BondDown = _cells[x, y - 1].BondUp;
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
}
