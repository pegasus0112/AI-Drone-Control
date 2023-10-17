using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneControl : MonoBehaviour
{
    [Space(10)]
    [Header("Control")]
    [Space(3)]
    [Range(0, 1)] public float throttle = 0;
    [Range(-1, 1)] public float yaw = 0;
    [Range(-1, 1)] public float pitch = 0;
    [Range(-1, 1)] public float roll = 0;

    [Header("Motors")]
    public Motor motorLF;
    public Motor motorRF;
    public Motor motorLB;
    public Motor motorRB;

    private float newMotorPowerLF = 0;
    private float newMotorPowerLB = 0;
    private float newMotorPowerRF = 0;
    private float newMotorPowerRB = 0;

    // Start is called before the first frame update
    void Start()
    {
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

        /*
        //yaw
        
        newMotorPowerLF -= calculatedYaw;
        newMotorPowerRF += calculatedYaw;
        newMotorPowerLB += calculatedYaw;
        newMotorPowerRB -= calculatedYaw;
        //pitch
        
        newMotorPowerLF -= calculatedPitch;
        newMotorPowerRF -= calculatedPitch;
        newMotorPowerLB += calculatedPitch;
        newMotorPowerRB += calculatedPitch;
        //roll
        
        newMotorPowerLF += calculatedRoll;
        newMotorPowerRF -= calculatedRoll;
        newMotorPowerLB += calculatedRoll;
        newMotorPowerRB -= calculatedRoll;

        ClampMotorPower();
                */
        float calculatedYaw = calculateRatesValue(yaw);
        float calculatedPitch = calculateRatesValue(pitch);
        float calculatedRoll = calculateRatesValue(roll);
        AddTorqueRotation(calculatedYaw);
    }

    private void AddTorqueRotation(float calculatedYaw)
    {
        gameObject.transform.Rotate(new Vector3(0, calculatedYaw * 4, 0));
    }

    private void ClampMotorPower()
    {
        newMotorPowerLF = Math.Clamp(newMotorPowerLF, 0.01f, 1);
        newMotorPowerRB = Math.Clamp(newMotorPowerRB, 0.01f, 1);
        newMotorPowerLB = Math.Clamp(newMotorPowerLB, 0.01f, 1);
        newMotorPowerRF = Math.Clamp(newMotorPowerRF, 0.01f, 1);
    }

    //only use for ROLL, YAW, PITCH
    private float calculateRatesValue(float unmappedValue)
    {
        return 0.9f * Mathf.Pow(unmappedValue, 3) + 0.1f * unmappedValue;
    }
}
