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

    // Start is called before the first frame update
    void Start()
    {
        var children = GetComponentsInChildren<Neighbourhood>();
        neighbourhoods = new List<Neighbourhood>(children);
        array = new Neighbourhood[1, 1];
        array[0, 0] = neighbourhoods[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Population => neighbourhoods.Sum(n => n.GetPopulation());

    public void AddNH(Vector2Int coords)
    {

    }

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

    public void Expand(int dir)
    {
        var r1 = array.GetLength(0);
        var r2 = array.GetLength(1);

        var found = false;
        var x = -1;
        var y = -1;
        if (dir == 0)//south
        {
            for (int i = 0; i < r1; i++)
            {
                if (array[i, r2 - 1] == null)
                {
                    found = true;
                    x = i;
                    y = r2 - 1;
                    break;
                }
            }

            if (!found)
            {
                array = ResizeArray(array, r1 + 1, r2);
                x = r1;
                y = r2 - 1;
            }
        }
        if (dir == 1)
        {
            for (int i = 0; i < r2; i++)
            {
                if (array[r1 - 1, i] == null)
                {
                    found = true;
                    x = r1 - 1;
                    y = i;
                    break;
                }
            }

            if(!found)
            {
                array = ResizeArray(array, r1, r2 + 1);
                x = r1 - 1;
                y = r2;
            }
        }


        //var newNH = Instantiate(transform.GetChild(0), transform);
        var newNH = Instantiate(PrefabNH, transform);

        
        newNH.transform.localPosition = new Vector3(y * 60, 0, x * 60);
        var nh = newNH.GetComponent<Neighbourhood>();
        //var ah = newNH.GetComponent<AgentHood>();
        //nh.Reset();
        //ah.Initialize();

        array[x, y] = nh;
        neighbourhoods.Add(nh);
        

        //var ah = newNH.GetComponent<AgentHood>();
        //ah.enabled = false;
    }
}
