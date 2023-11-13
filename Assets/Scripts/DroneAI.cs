using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class DroneAI : Agent
{
    [Header("Status")]
    public bool selfPlay = true;


    [Space(10)]
    [Header("Setup")]


    public DroneHandler droneHandler;
    public DroneControl droneControl;
    public Rigidbody droneRig;

    public EnvironmentManager environmentManager;

    [Space(10)]
    [Header("Reward / Pentalies")]
    public float rewardForFlying = 1;

    [Space(5)]
    public float penaltyForTooLongGrounded = -10;


    [Space(10)]
    [Header("Manual Control Setup")]
    public ManualControl manualControl;


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(droneHandler.droneFrameState);
        sensor.AddObservation(droneHandler.rotorStateLF);
        sensor.AddObservation(droneHandler.rotorStateRF);
        sensor.AddObservation(droneHandler.rotorStateLB);
        sensor.AddObservation(droneHandler.rotorStateRB);

        sensor.AddObservation(droneHandler.isGrounded);

        //later add acceleration & rotation
        sensor.AddObservation(new Vector3(droneRig.velocity.x, droneRig.velocity.y, droneRig.velocity.z));
        sensor.AddObservation(gameObject.transform.rotation);

        // y height (relative heigth to ground)
        sensor.AddObservation(gameObject.transform.position.y - environmentManager.ground.position.y);
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
            //AddReward(penalyForCrashing);
            environmentManager.EndTraining();
        }

        AddRewardAndPenalties();
    }

    private void AddRewardAndPenalties()
    {
        if(droneHandler.isGrounded)
        {
            AddReward(-rewardForFlying * Time.deltaTime);
        } else
        {
            AddReward(rewardForFlying * Time.deltaTime);
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
        Debug.Log("Drone scored " + points);
        AddReward(points);
    }

    public void EndEpisodeBecauseTooLongGrounded()
    {
        Debug.Log("Resetting training because of ground time");
        AddReward(penaltyForTooLongGrounded);
        environmentManager.EndTraining();
    }
}
