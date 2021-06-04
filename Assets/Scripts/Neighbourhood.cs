using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Neighbourhood : MonoBehaviour
{
    public int[] Buildings;

    // Start is called before the first frame update
    void Start()
    {
        //Buildings = new int[4 * 4];
        Buildings = new int[2 * 2];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestStart() => Start();

    public bool Build(int location, int typ)
    {
        //Debug.Log("Build " + location + " " + typ);

        if (location < 0 || location >= Buildings.Length)
            return false;
        if (typ < 0 || typ > 1)
            return false;
        if (Buildings[location] != 0)
            return false;

        if (typ == 0)
            return false;

        Buildings[location] = typ;

        //transform.GetChild(location).GetComponent<Renderer>().material.color = Color.green;

        return true;
    }

    public int GetPopulation() =>
        Buildings.Count(b => b == 1);

    public bool IsComplete() =>
        Buildings.All(b => b != 0);

    public void Reset()
    {
        for (int i = 0; i < Buildings.Length; i++)
            Buildings[i] = 0;
        /*
        foreach (Transform child in transform)
            child.GetComponent<Renderer>().material.color = Color.white;
        */
    }
}
