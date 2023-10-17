using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManualControl : MonoBehaviour
{
    [Space(5)]
    public InputAction throttleAxis;
    public InputAction yawAxis;
    public InputAction pitchAxis;
    public InputAction rollAxis;

    [Range(0, 1)] public float throttle;
    [Range(-1, 1)] public float yaw = 0;
    [Range(-1, 1)] public float pitch = 0;
    [Range(-1, 1)] public float roll = 0;

    private void OnEnable()
    {
        throttleAxis.Enable();
        yawAxis.Enable();
        pitchAxis.Enable();
        rollAxis.Enable();
    }

    private void OnDisable()
    {
        throttleAxis.Disable();
        yawAxis.Disable();
        pitchAxis.Disable();
        rollAxis.Disable();
    }


    public void ReadUserInputJoysticks()
    {
        throttle = throttleAxis.ReadValue<float>();
        throttle = Remap(throttle, 0, 0.63f, 0, 1);
        Debug.Log("throttle " + throttle);

        pitch = pitchAxis.ReadValue<float>();
        pitch = Remap(pitch, 0, 1, -1, 1);
        Debug.Log("pitch " + pitch);

        roll = rollAxis.ReadValue<float>();
        roll = Remap(roll, -1, 0, 1, -1);
        Debug.Log("roll " + roll);

        yaw = yawAxis.ReadValue<float>();
        yaw = Remap(yaw, 0, -1, -1, 1);
        Debug.Log("yaw " + yaw);
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
