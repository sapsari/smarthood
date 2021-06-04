using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum LotType
{
    Invalid,
    Empty,
    House,
    Park,
}


public class Lot
{
    public LotType Type;
    public bool NextToPark;
    public int Population => Type == LotType.House ? (NextToPark ? 2 : 1) : 0;

    public void Reset()
    {
        Type = LotType.Empty;
        //Population = 0;
        NextToPark = false;
    }
}

