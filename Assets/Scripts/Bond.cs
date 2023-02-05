using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bond : MonoBehaviour
{
    public int Player = 0;
    public bool IsVertical;
    public Cell Cell1 = null;
    public Cell Cell2 = null;
    public int index;

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

    public bool HasWater()
    {
        return Cell1.HasWater() || Cell2.HasWater();
    }
}
