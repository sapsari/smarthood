using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    public City City;

    public Text TextPopulation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TextPopulation.text = "Population: " + City.Population;
    }

    public void Expand(int index)
    {
        City.Expand(index);
    }
}
