using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHood : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var vs = new Unity.MLAgents.Sensors.VectorSensor(4);
        var ah = new AgentHood();
        //ah.Initialize();
        ah.InitTest();
        ah.CollectObservations(vs);

        var a1 = new Unity.MLAgents.Actuators.ActionBuffers(null, new int[] { 0, 1 });
        ah.OnActionReceived(a1);
        var a2 = new Unity.MLAgents.Actuators.ActionBuffers(null, new int[] { 1, 1 });
        ah.OnActionReceived(a2);
        var a3 = new Unity.MLAgents.Actuators.ActionBuffers(null, new int[] { 2, 1 });
        ah.OnActionReceived(a3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
