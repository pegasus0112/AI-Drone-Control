using System;
using UnityEngine;

public class DroneControl : MonoBehaviour
{
    [Space(10)]
    [Header("Control")]
    [Space(3)]
    [Range(0, 1)] public float throttle = 0;
    [Range(-1, 1)] public float yaw = 0;
    [Range(-1, 1)] public float pitch = 0;
    [Range(-1, 1)] public float roll = 0;

    [Space(10)]
    [Header("Motors")]
    public Motor motorLF;
    public Motor motorRF;
    public Motor motorLB;
    public Motor motorRB;

    [Space(10)]
    [Header("Settings")]
    [Range(5, 50)] public float yawMultiplier;

    private void OnEnable()
    {
        ResetControls();
    }

    void FixedUpdate()
    {
        SetMotorPower();
    }

    private void SetMotorPower()
    {
        //adding polynomial function
        float calculatedYaw = CalculateRatesValue(yaw);
        float calculatedPitch = CalculateRatesValue(pitch);
        float calculatedRoll = CalculateRatesValue(roll);

        //setting motor speed
        motorLF.motorSpeed = (throttle + ((-calculatedYaw - calculatedPitch + calculatedRoll) / 3)) / 2;
        motorRF.motorSpeed = (throttle + ((calculatedYaw - calculatedPitch - calculatedRoll) / 3)) / 2;
        motorLB.motorSpeed = (throttle + ((calculatedYaw + calculatedPitch + calculatedRoll) / 3)) / 2;
        motorRB.motorSpeed = (throttle + ((-calculatedYaw + calculatedPitch - calculatedRoll) / 3)) / 2;

        AddTorqueRotation(calculatedYaw);
    }

    private void AddTorqueRotation(float calculatedYaw)
    {
        gameObject.transform.Rotate(new Vector3(0, calculatedYaw * yawMultiplier, 0));
    }

    private void ResetControls()
    {
        throttle = 0;
        yaw = 0;
        pitch = 0;
        roll = 0;
    }

    //only use for ROLL, YAW, PITCH
    private float CalculateRatesValue(float unmappedValue)
    {
        return 0.9f * Mathf.Pow(unmappedValue, 3) + 0.1f * unmappedValue;
    }
}
