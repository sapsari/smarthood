using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentHood : Agent
{
    Neighbourhood nh;
    EnvironmentParameters defaultParameters;

    public override void Initialize()
    {
        nh = this.GetComponent<Neighbourhood>();
        defaultParameters = Academy.Instance.EnvironmentParameters;
        ResetScene();
    }

    public void InitTest()
    {
        nh = new Neighbourhood();
        nh.TestStart();
        defaultParameters = Academy.Instance.EnvironmentParameters;
        ResetScene();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation()
        foreach (var building in nh.Buildings)
            sensor.AddObservation(building);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var pos = actions.DiscreteActions[0];
        var typ = actions.DiscreteActions[1];

        //Debug.Log("rew:" + nh.GetPopulation());
        var hasBuilt = nh.Build(pos, typ);
        var pop = nh.GetPopulation();
        //Debug.Log("end");

        if (hasBuilt)
        {
            var isComplete = nh.IsComplete();
            if(isComplete)
            {
                Debug.Log("complete");
                //SetReward(pop * 2);
                AddReward(2);
                EndEpisode();
            }
            else
            {
                //SetReward(pop);
                AddReward(1);
            }
        }
        else
        {
            //SetReward(pop - .5f);
            if (pop > 1)
                Debug.Log("pop:" + pop);
            //SetReward(pop);
            EndEpisode();
        }
    }

    void ResetScene()
    {
        //Debug.Log("reset");
        nh.Reset();
    }

    public override void OnEpisodeBegin()
    {
        ResetScene();
    }

    public void FixedUpdate()
    {
        WaitTimeInference();
    }

    void WaitTimeInference()
    {
        if (Academy.Instance.IsCommunicatorOn)
        {
            //if ((area.teamTurn == TEAM_TURN_O && teamId == TEAM_ID_O) || (area.teamTurn == TEAM_TURN_X && teamId == TEAM_ID_X))
            {
                RequestDecision();
            }
        }
        else
        {
/*
            if (m_TimeSinceDecision >= timeBetweenDecisionsAtInference)
            {
                m_TimeSinceDecision = 0f;
                if ((area.teamTurn == TEAM_TURN_O && teamId == TEAM_ID_O) || (area.teamTurn == TEAM_TURN_X && teamId == TEAM_ID_X))
                {
                    //heuristic mode need break between games to ensure proper action
                    if (behaviorType == BehaviorType.HeuristicOnly && area.isHeuristicPause)
                    {
                        return;
                    }
                    RequestDecision();
                }
            }
            else
            {
                m_TimeSinceDecision += Time.fixedDeltaTime;
            }*/
        }
    }
}
