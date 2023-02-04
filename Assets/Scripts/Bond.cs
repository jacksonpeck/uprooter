using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BondState 
{
    None,
    Player1,
    Player2,
    Player3,
    Player4
}

public class Bond : MonoBehaviour
{
    public BondState State;
    public bool IsVertical;
    public Cell Cell1 = null;
    public Cell Cell2 = null;

    public Cell OtherCell(Cell cell)
    {
        if (cell == Cell1)
        {
            return Cell2;
        }
        else if (cell == Cell2)
        {
            return Cell1;
        }
        return null;
    }
}
