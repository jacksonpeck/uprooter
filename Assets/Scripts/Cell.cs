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
    [SerializeField] private Sprite _dirtSprite, _rockSprite, _waterSprite;
    
    public Vector2Int Location;

    private CellType _type;
    public CellType Type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;
            switch (value)
            {
            case CellType.Dirt:
                _renderer.sprite = _dirtSprite;
                break;
            case CellType.Rock:
                _renderer.sprite = _rockSprite;
                break;
            case CellType.Water:
                _renderer.sprite = _waterSprite;
                break;
            default:
                break;
            }
        }
    }
    
    public Bond BondLeft = null;
    public Bond BondRight = null;
    public Bond BondUp = null;
    public Bond BondDown = null;


    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
}
