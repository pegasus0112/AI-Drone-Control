using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
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
    [Header("Manual Control Setup")]
    public ManualControl manualControl;


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(droneHandler.rotorStateLF);
        sensor.AddObservation(droneHandler.rotorStateRF);
        sensor.AddObservation(droneHandler.rotorStateLB);
        sensor.AddObservation(droneHandler.rotorStateRB);

        //later add acceleration & rotation
        sensor.AddObservation(new Vector3(droneRig.velocity.x, droneRig.velocity.y, droneRig.velocity.z));
        sensor.AddObservation(gameObject.transform.rotation);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (droneHandler.CheckRotorGotDestroyed())
        {
            RequestDecision();
        }
        else
        {
            ResettingControls();
            environmentManager.EndTraining();
            //GAME OVER
        }
    }

    private void ResettingControls()
    {
        droneControl.throttle = 0;
        droneControl.yaw = 0;
        droneControl.pitch = 0;
        droneControl.roll = 0;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (selfPlay)
        {
            var continuousActionsOut = actionsOut.ContinuousActions;

            manualControl.ReadUserInputJoysticks();
            continuousActionsOut[1] = Input.GetAxis("Horizontal");
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
}
