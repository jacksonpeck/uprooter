using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    Dirt,
    Rock,
    Water
}

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject _waterPrefab;
    [SerializeField] private GameObject _rockPrefab;
    
    public Vector2Int Location;

    public CellType Type
    {
        get
        {
            if (HasRock())
                return CellType.Rock;
            if (HasWater())
                return CellType.Water;
            return CellType.Dirt;
        }
    }
    
    public Bond BondLeft = null;
    public Bond BondRight = null;
    public Bond BondUp = null;
    public Bond BondDown = null;

    private Water _water = null;
    private Rock _rock = null;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void AddWater()
    {
        _water = Instantiate(_waterPrefab, this.transform).GetComponent<Water>();
        _water.transform.SetParent(this.transform);
    }

    public bool HasWater()
    {
        return _water != null;
    }

    public void AddRock()
    {
        _rock = Instantiate(_rockPrefab, this.transform).GetComponent<Rock>();
        _rock.transform.SetParent(this.transform);
    }

    public bool HasRock()
    {
        return _rock != null;
    }
}
