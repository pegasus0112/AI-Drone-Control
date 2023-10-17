using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DroneHandler : MonoBehaviour
{
    public enum ROTOR
    {
        LF, // Left Front
        RF, // Right Front
        LB, // Left Back
        RB  // Right Back
    }

    public DroneControl droneControl;

    //are rotors ok?
    public bool rotorStateLF = true;
    public bool rotorStateRF = true;
    public bool rotorStateLB = true;
    public bool rotorStateRB = true;

    [Header("Status")]
    public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Everything except controle and AI
        CheckAnyMotorGrounded();
    }

    private void CheckAnyMotorGrounded()
    {
       isGrounded = droneControl.motorLF.isGrounded 
            | droneControl.motorRF.isGrounded 
            | droneControl.motorLB.isGrounded 
            | droneControl.motorRB.isGrounded;
    }

    private void CheckRotorGotDestroyed()
    {
        rotorStateLF = droneControl.motorLF.rotor.IsDestroyed();
        rotorStateRF = droneControl.motorRF.rotor.IsDestroyed();
        rotorStateLB = droneControl.motorLB.rotor.IsDestroyed();
        rotorStateRB = droneControl.motorRB.rotor.IsDestroyed();
    }

}
