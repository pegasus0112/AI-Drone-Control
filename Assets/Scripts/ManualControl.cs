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

    //enable & disable needed for new input system
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
        throttle = Remap(throttle, -1, 1, 0, 1);

        //inverting pitch
        pitch = -pitchAxis.ReadValue<float>();

        roll = rollAxis.ReadValue<float>();
        yaw = yawAxis.ReadValue<float>();
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
