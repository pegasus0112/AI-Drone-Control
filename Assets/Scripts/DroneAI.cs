using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class DroneAI : Agent
{
    [Header("Status")]
    public bool selfPlay = true;
    public int clearedObjectCount = 0;


    [Space(10)]
    [Header("Setup")]
    public DroneHandler droneHandler;
    public DroneControl droneControl;
    public Rigidbody droneRig;

    public EnvironmentManager environmentManager;

    [Space(10)]
    [Header("Reward / Pentalies")]
    public float rewardForFlyingOverTime = 3;

    [Space(5)]
    public float penalyForCrashing = -30;
    public float penaltyForGroundedOverTime = -10;
    public float penaltyForTooLongGrounded = -20;


    [Space(10)]
    [Header("Manual Control Setup")]
    public ManualControl manualControl;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(droneHandler.isGrounded);

        sensor.AddObservation(droneHandler.droneFrameState);
        sensor.AddObservation(droneHandler.rotorStateLF);
        sensor.AddObservation(droneHandler.rotorStateRF);
        sensor.AddObservation(droneHandler.rotorStateLB);
        sensor.AddObservation(droneHandler.rotorStateRB);

        //later add acceleration & rotation
        sensor.AddObservation(new Vector3(droneRig.velocity.x, droneRig.velocity.y, droneRig.velocity.z));
        sensor.AddObservation(gameObject.transform.rotation);

        // current controls
        sensor.AddObservation(droneControl.throttle);
        sensor.AddObservation(droneControl.yaw);
        sensor.AddObservation(droneControl.pitch);
        sensor.AddObservation(droneControl.roll);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (droneHandler.CheckPartsAreOkay())
        {
            RequestDecision();
        }
        else
        {
            //GAME OVER
            AddReward(penalyForCrashing);
            environmentManager.EndTraining();
        }

        AddRewardAndPenalties();
    }

    private void AddRewardAndPenalties()
    {
        if(droneHandler.isGrounded)
        {
            AddReward(penaltyForGroundedOverTime * Time.deltaTime);
        } else
        {
            AddReward(rewardForFlyingOverTime * Time.deltaTime);
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (selfPlay)
        {
            var continuousActionsOut = actionsOut.ContinuousActions;

            manualControl.ReadUserInputJoysticks();
            continuousActionsOut[0] = manualControl.throttle;
            continuousActionsOut[1] = manualControl.yaw;
            continuousActionsOut[2] = manualControl.pitch;
            continuousActionsOut[3] = manualControl.roll;
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        droneControl.throttle = actionBuffers.ContinuousActions[0];
        droneControl.yaw = actionBuffers.ContinuousActions[1];
        droneControl.pitch = actionBuffers.ContinuousActions[2];
        droneControl.roll = actionBuffers.ContinuousActions[3];
    }

    public void Scored(float points)
    {
        //Debug.Log("Drone scored " + points);
        Debug.Log("Drone scored ");
        clearedObjectCount++;
        AddReward(points);
    }

    public void EndEpisodeBecauseTooLongGrounded()
    {
        Debug.Log("Resetting training because of ground time");
        //AddReward(penaltyForTooLongGrounded);
        environmentManager.EndTraining();
    }
}
