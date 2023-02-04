using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x, y;
    
    public Bond BondLeft = null;
    public Bond BondRight = null;
    public Bond BondUp = null;
    public Bond BondDown = null;
}
