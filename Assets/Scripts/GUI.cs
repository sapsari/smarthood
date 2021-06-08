using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    public City City;

    public Text TextPopulation;

    public Text TextNorth;
    public Text TextEast;

    public GameObject PanelToggleNorth;
    public GameObject PanelToggleEast;

    Toggle[,] ToggleTableNorth;
    Toggle[] ToggleArrayNorth;
    Toggle[][] ToggleRowsNorth;
    Toggle[][] ToggleColsNorth;

    Toggle[,] ToggleTableEast;
    Toggle[] ToggleArrayEast;
    Toggle[][] ToggleRowsEast;
    Toggle[][] ToggleColsEast;


    // Start is called before the first frame update
    void Start()
    {
        InitTable(PanelToggleNorth, out ToggleTableNorth, out ToggleArrayNorth,
            out ToggleRowsNorth, out ToggleColsNorth);

        InitTable(PanelToggleEast, out ToggleTableEast, out ToggleArrayEast,
            out ToggleRowsEast, out ToggleColsEast);

        UpdateToggles(City.dataNorth, City.dataEast);
    }

    static void InitTable(GameObject parent, out Toggle[,] table, out Toggle[] array,
        out Toggle[][] rows, out Toggle[][] cols)
    {
        table = new Toggle[3, 3];
        array = parent.GetComponentsInChildren<Toggle>();
        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
                table[x, y] = array[x + y * 3];

        rows = new Toggle[3][];
        for (int y = 0; y < 3; y++)
        {
            rows[y] = new Toggle[3];
            for (int x = 0; x < 3; x++)
            {
                rows[y][x] = table[x, y];
            }
        }

        cols = new Toggle[3][];
        for (int x = 0; x < 3; x++)
        {
            cols[x] = new Toggle[3];
            for (int y = 0; y < 3; y++)
            {
                cols[x][y] = table[x, y];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        TextPopulation.text = "Population: " + City.Population;
    }

    public void Expand(int index)
    {
        City.Expand(index);
        UpdateToggles(City.dataNorth, City.dataEast);
    }

    public void OnToggle(int northSouth, int row, int col)
    {

    }

    public void OnToggle2(bool val)
    {
        // Debug.Log("toggle:" + val);
    }


    static void ToggleAux(Toggle toggle, Toggle[,] table, Toggle[][] cols, Toggle[][] rows)
    {
        var xy = table.CoordinatesOf(toggle);
        var x = xy.x;
        var y = xy.y;
        var col = cols[x];
        var row = rows[y];

        var unselectedCol = Array.IndexOf(cols, cols.FirstOrDefault(c => c.All(t => !t.isOn)));

        if (unselectedCol != -1)
        {
            var otherRow = Array.IndexOf(rows, rows.FirstOrDefault(r => r != row && r[x].isOn));

            if (otherRow != -1)
                table[unselectedCol, otherRow].isOn = true;
        }
    }

    public void OnToggleNorth(Toggle toggle)
    {
        //Debug.Log(toggle.name + ":" + toggle.isOn);

        if (toggle.isOn)
            ToggleAux(toggle, ToggleTableNorth, ToggleColsNorth, ToggleRowsNorth);
    }

    public void OnToggleEast(Toggle toggle)
    {
        //Debug.Log(toggle.name + ":" + toggle.isOn);

        if (toggle.isOn)
            ToggleAux(toggle, ToggleTableEast, ToggleColsEast, ToggleRowsEast);
    }

    public void UpdateToggles(NHData north, NHData east)
    {

        int[] dataNorth = new int[3] { north.BlockedLotCount, north.HousePopulation, north.ParkBonus };
        int[] dataEast = new int[3] { east.BlockedLotCount, east.HousePopulation, east.ParkBonus };
        
        for (int x=0;x<3;x++)
        {
            var val = dataNorth[x].ToString();
            for (int y = 0; y < 3; y++)
                ToggleTableNorth[x, y].GetComponentInChildren<Text>().text = val;
        }

        for (int x = 0; x < 3; x++)
        {
            var val = dataEast[x].ToString();
            for (int y = 0; y < 3; y++)
                ToggleTableEast[x, y].GetComponentInChildren<Text>().text = val;
        }
    }
}

public static class ExtensionMethods
{
    public static Vector2Int CoordinatesOf<T>(this T[,] matrix, T value)
    {
        int w = matrix.GetLength(0); // width
        int h = matrix.GetLength(1); // height

        for (int x = 0; x < w; ++x)
        {
            for (int y = 0; y < h; ++y)
            {
                if (matrix[x, y].Equals(value))
                    return new Vector2Int(x, y);
            }
        }

        return new Vector2Int(-1, -1);
    }
}
