using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class AgentHood : Agent
{
    BehaviorType behaviorType;
    BehaviorParameters behaviorParameters;


    bool isHeuristicPause = false;
    float fixedHeuristicPauseTime = 2.0f;
    float currentHeuristicPauseTime = 0.0f;



    Neighbourhood nh;
    EnvironmentParameters defaultParameters;

    const float timeBetweenDecisionsAtInference = .1f;
    float timeSinceDecision;


    public override void Initialize()
    {
        behaviorParameters = gameObject.GetComponent<BehaviorParameters>();
        behaviorType = behaviorParameters.BehaviorType;

        nh = this.GetComponent<Neighbourhood>();
        defaultParameters = Academy.Instance.EnvironmentParameters;
        ResetScene();

        nh.IsTraining = IsTraining;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (nh == null || nh.lots == null) // called when disabling the component
            return;

        foreach (var building in nh.lots)
            sensor.AddObservation((int)building.Type);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var pos = actions.DiscreteActions[0];
        var typ = actions.DiscreteActions[1];

        //Debug.Log("rew:" + nh.GetPopulation());
        var hasBuilt = nh.Build(pos, (LotType)typ);
        var pop = nh.GetPopulation();
        //Debug.Log("end");

        if (hasBuilt)
        {
            var isComplete = nh.IsComplete();
            if(isComplete)
            {
                //Debug.Log("complete");
                //SetReward(pop * 2);
                //AddReward(2);
                SetReward(pop + 3);
                EndEpisode();
            }
            else
            {
                SetReward(pop);
                //AddReward(1);
            }
        }
        else
        {
            //SetReward(pop - .5f);
            //if (pop > 1) Debug.Log("pop:" + pop);
            //SetReward(pop);

            /*
            if (!Academy.Instance.IsCommunicatorOn)
                this.enabled = false;
            */

            EndEpisode();
        }
    }

    void ResetScene()
    {
        //Debug.Log("reset");
        isHeuristicPause = true;
        nh.Reset();
    }

    bool IsTraining => Academy.Instance.IsCommunicatorOn;

    public override void OnEpisodeBegin()
    {
        if (IsTraining || CompletedEpisodes == 0)
            ResetScene();
    }

    public void FixedUpdate()
    {
        // Make a move every step if we're training, or we're overriding models in CI.
        var useFast = IsTraining;//) || (m_ModelOverrider != null && m_ModelOverrider.HasOverrides);
        if (useFast)
        {
            FastUpdate();
        }
        else
        {
            AnimatedUpdate();
        }
    }
            
    void FastUpdate() => RequestDecision();

    void AnimatedUpdate()
    {
        if (isHeuristicPause)
        {
            if (currentHeuristicPauseTime > fixedHeuristicPauseTime)
            {
                isHeuristicPause = false;
                currentHeuristicPauseTime = 0.0f;
            }
            currentHeuristicPauseTime += Time.fixedDeltaTime;
        }


        if (timeSinceDecision >= timeBetweenDecisionsAtInference)
        {
            timeSinceDecision = 0f;
            {
                //heuristic mode need break between games to ensure proper action
                if (behaviorType == BehaviorType.HeuristicOnly && isHeuristicPause)
                {
                    return;
                }
                RequestDecision();
            }
        }
        else
        {
            timeSinceDecision += Time.fixedDeltaTime;
        }

    }
}
