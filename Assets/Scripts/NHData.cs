using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NHData
{
    public int BlockedLotCount;
    public int HousePopulation;
    public int ParkBonus;

    public NHData()
    {
        Randomize();
    }

    public void Randomize()
    {
        BlockedLotCount = Random.Range(1, Neighbourhood.MaxInvalidLotCount + 1);
        HousePopulation = Random.Range(1, Neighbourhood.MaxHousePop + 1);
        ParkBonus = Random.Range(1, Neighbourhood.MaxParkBonus + 1);
    }
}
