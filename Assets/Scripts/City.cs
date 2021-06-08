using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class City : MonoBehaviour
{
    public List<Neighbourhood> neighbourhoods;

    public GameObject PrefabHouse;
    public GameObject PrefabPark;
    public GameObject PrefabBoulder;

    public GameObject PrefabNH;

    Neighbourhood[,] array;

    public NHData dataNorth, dataEast;

    // Start is called before the first frame update
    void Start()
    {
        var children = GetComponentsInChildren<Neighbourhood>();
        neighbourhoods = new List<Neighbourhood>(children);
        array = new Neighbourhood[1, 1];
        array[0, 0] = neighbourhoods[0];

        dataNorth = new NHData();
        dataEast = new NHData();

    }

    public int Population => neighbourhoods.Sum(n => n.GetPopulation());

    T[,] ResizeArray<T>(T[,] original, int rows, int cols)
    {
        var newArray = new T[rows, cols];
        int minRows = Math.Min(rows, original.GetLength(0));
        int minCols = Math.Min(cols, original.GetLength(1));
        for (int i = 0; i < minRows; i++)
            for (int j = 0; j < minCols; j++)
                newArray[i, j] = original[i, j];
        return newArray;
    }

    public void Expand(int dir)//0->north,1->east
    {
        var r1 = array.GetLength(0);
        var r2 = array.GetLength(1);
        int x, y;
        GetExpansionCoords(dir, r1, r2, out x, out y);


        //var newNH = Instantiate(transform.GetChild(0), transform);
        var newNH = Instantiate(PrefabNH, transform);


        newNH.transform.localPosition = new Vector3(y * 60, 0, x * 60);
        var nh = newNH.GetComponent<Neighbourhood>();
        //var ah = newNH.GetComponent<AgentHood>();
        //nh.Reset();
        //ah.Initialize();
        nh.Reset(dir == 0 ? dataNorth : dataEast);

        array[x, y] = nh;
        neighbourhoods.Add(nh);

        dataNorth = new NHData();
        dataEast = new NHData();


        //var ah = newNH.GetComponent<AgentHood>();
        //ah.enabled = false;
    }

    private void GetExpansionCoords(int dir, int r1, int r2, out int x, out int y)
    {
        var found = false;
        x = -1;
        y = -1;
        if (dir == 0)//north
        {
            for (int i = 0; i < r1; i++)
            {
                for (int j = 0; j < r2 - 1; j++)
                {
                    if (array[i, j] == null)
                    {
                        found = true;
                        x = i;
                        y = j;
                        break;
                    }
                }
            }

            if (!found)
            {
                array = ResizeArray(array, r1 + 1, r2);
                x = r1;
                y = 0;
            }
        }
        if (dir == 1)
        {
            for (int j = 0; j < r2; j++)
            {
                for (int i = 0; i < r1 - 1; i++)
                {
                    if (array[i, j] == null)
                    {
                        found = true;
                        x = i;
                        y = j;
                        break;
                    }
                }
            }

            if (!found)
            {
                array = ResizeArray(array, r1, r2 + 1);
                x = 0;
                y = r2;
            }
        }
    }
}
