using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

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
    [Range(5, 20)] public float yawMultiplier;



    private float newMotorPowerLF = 0;
    private float newMotorPowerLB = 0;
    private float newMotorPowerRF = 0;
    private float newMotorPowerRB = 0;

    private void OnEnable()
    {
        ResetControls();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CalculateMotorPower();
        UpdateMotorPower();
    }

    private void UpdateMotorPower()
    {
        motorLF.motorSpeed = newMotorPowerLF;
        motorRF.motorSpeed = newMotorPowerRF;
        motorLB.motorSpeed = newMotorPowerLB;
        motorRB.motorSpeed = newMotorPowerRB;
    }

    private void CalculateMotorPower()
    {
        //throttle
        newMotorPowerLF = throttle;
        newMotorPowerRF = throttle;
        newMotorPowerLB = throttle;
        newMotorPowerRB = throttle;

        //yaw
        float calculatedYaw = CalculateRatesValue(yaw);
        newMotorPowerLF -= calculatedYaw;
        newMotorPowerRF += calculatedYaw;
        newMotorPowerLB += calculatedYaw;
        newMotorPowerRB -= calculatedYaw;

        //pitch
        float calculatedPitch = CalculateRatesValue(pitch);
        newMotorPowerLF -= calculatedPitch;
        newMotorPowerRF -= calculatedPitch;
        newMotorPowerLB += calculatedPitch;
        newMotorPowerRB += calculatedPitch;

        //roll
        float calculatedRoll = CalculateRatesValue(roll);
        newMotorPowerLF += calculatedRoll;
        newMotorPowerRF -= calculatedRoll;
        newMotorPowerLB += calculatedRoll;
        newMotorPowerRB -= calculatedRoll;

        ClampMotorPower();
        AddTorqueRotation(calculatedYaw);
    }

    private void AddTorqueRotation(float calculatedYaw)
    {
        gameObject.transform.Rotate(new Vector3(0, calculatedYaw * yawMultiplier, 0));
    }

    private void ClampMotorPower()
    {
        newMotorPowerLF = Math.Clamp(newMotorPowerLF, 0, 1);
        newMotorPowerRB = Math.Clamp(newMotorPowerRB, 0, 1);
        newMotorPowerLB = Math.Clamp(newMotorPowerLB, 0, 1);
        newMotorPowerRF = Math.Clamp(newMotorPowerRF, 0, 1);
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