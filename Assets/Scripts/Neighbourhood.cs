using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Neighbourhood : MonoBehaviour
{
    City city;

    //public bool IsTraining { get; set; }
    bool IsTraining => Unity.MLAgents.Academy.Instance.IsCommunicatorOn;


    public Lot[] lots;

    const int rowSize = 4;
    const int colSize = 4;

    public int housePop = 1;
    public int parkBonus = 1;


    public const int MaxInvalidLotCount = 3;
    public const int MaxHousePop = 3;
    public const int MaxParkBonus = 3;

    public void Awake()
    {
        lots = new Lot[rowSize * colSize];
        for (int i = 0; i < rowSize * colSize; i++)
        {
            lots[i] = new Lot();
            lots[i].Reset();
        }

        city = FindObjectOfType<City>();
    }

    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */


    public bool Build(int location, LotType lotType)
    {
        //Debug.Log("Build " + location + " " + typ);

        if (location < 0 || location >= lots.Length)
            return false;
        if (lotType == LotType.Invalid || lotType == LotType.Empty)
            return false;
        if (lots[location].Type != LotType.Empty)
            return false;

        lots[location].Type = lotType;

        if (lotType == LotType.House)
        {
            var left = location - 1;
            var right = location + 1;
            var up = location - rowSize;
            var down = location + rowSize;

            if (
                (left >= 0 && lots[left].Type == LotType.Park) ||
                (right < rowSize * colSize && lots[right].Type == LotType.Park) ||
                (up >= 0 && lots[up].Type == LotType.Park) ||
                (down < rowSize * colSize && lots[down].Type == LotType.Park)
                )
                lots[location].NextToPark = true;
        }

        else if (lotType == LotType.Park)
        {
            var left = location - 1;
            var right = location + 1;
            var up = location - rowSize;
            var down = location + rowSize;

            if (left >= 0 && lots[left].Type == LotType.House)
                lots[left].NextToPark = true;
            if (right < rowSize * colSize && lots[right].Type == LotType.House)
                lots[right].NextToPark = true;
            if (up >= 0 && lots[up].Type == LotType.House)
                lots[up].NextToPark = true;
            if (down < rowSize * colSize && lots[down].Type == LotType.House)
                lots[down].NextToPark = true;
        }

        SetColor(location);
        InsertModel(location);

        return true;
    }

    void SetColor(int location)
    {
        if (!IsTraining)
        {
            var lotType = lots[location].Type;

            Color color;
            switch (lotType)
            {
                case LotType.Invalid:
                    color = Color.gray;
                    break;
                case LotType.Empty:
                    color = Color.white;
                    break;
                case LotType.House:
                    color = Color.yellow;
                    break;
                case LotType.Park:
                    color = Color.green;
                    break;
                default:
                    color = Color.black;
                    break;
            }

            transform.GetChild(location).GetComponent<Renderer>().material.color = color;
        }
    }

    void InsertModel(int location)
    {
        if (IsTraining)
            return;

        var lotType = lots[location].Type;
        GameObject prefab;
        switch (lotType)
        {
            case LotType.Invalid:
                prefab = city.PrefabBoulder;
                break;
            case LotType.Empty:
                prefab = null;
                break;
            case LotType.House:
                prefab = city.PrefabHouse;
                break;
            case LotType.Park:
                prefab = city.PrefabPark;
                break;
            default:
                prefab = null;
                break;
        }

        if (prefab != null)
        {
            Instantiate(prefab, transform.GetChild(location));
        }
    }

    public int GetPopulation() =>
        lots.Sum(l => l.GetPopulation(housePop, parkBonus));

    public bool IsComplete() =>
        lots.All(l => l.Type != LotType.Empty);

    void Reset(int? invalidLotCount, int? housePop, int? parkBonus)
    {
        for (int i = 0; i < lots.Length; i++)
            lots[i].Reset();

        var invalidCount = invalidLotCount ?? Random.Range(1, MaxInvalidLotCount + 1);
        for (int i = 0; i < invalidCount; i++)
        {
            var location = Random.Range(0, rowSize * colSize);
            lots[location].Type = LotType.Invalid;
            InsertModel(location);
        }
        this.housePop = housePop ?? Random.Range(1, MaxHousePop + 1);
        this.parkBonus = parkBonus ?? Random.Range(1, MaxParkBonus + 1);

        if (!IsTraining)
        {
            for (int i = 0; i < rowSize * colSize; i++)
                SetColor(i);

            //foreach (Transform child in transform)
                //child.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void Reset() => Reset(null, null, null);

    public void Reset(NHData data) => Reset(data.BlockedLotCount, data.HousePopulation, data.ParkBonus);  
}
