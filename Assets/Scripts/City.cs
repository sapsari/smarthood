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

    // Start is called before the first frame update
    void Start()
    {
        var children = GetComponentsInChildren<Neighbourhood>();
        neighbourhoods = new List<Neighbourhood>(children);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Population => neighbourhoods.Sum(n => n.GetPopulation());

    public void AddNH(Vector2Int coords)
    {

    }
}
