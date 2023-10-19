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

    //important for resetting state after restarting training
    private void OnEnable()
    {
        rotorStateLF = true;
        rotorStateRF = true;
        rotorStateLB = true;
        rotorStateRB = true;
    }

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

    public bool CheckRotorGotDestroyed()
    {
        return rotorStateLF && rotorStateRF && rotorStateLB && rotorStateRB;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hittedObject = collision.GetContact(0).thisCollider.gameObject;
        if (hittedObject.tag == "Rotor")
        {
            ROTOR hittedMotor = hittedObject.transform.parent.parent.GetComponent<Motor>().motorPosition;

            switch(hittedMotor)
            {
                case ROTOR.LF:
                    rotorStateLF = false;
                    break;
                case ROTOR.RF:
                    rotorStateRF = false;
                    break;
                case ROTOR.LB:
                    rotorStateLB = false;
                    break;
                case ROTOR.RB:
                    rotorStateRB = false;
                    break;
            }
        }

        //Debug.Log(collision.GetContact(0).thisCollider.gameObject.tag + " - " + collision.GetContact(0).otherCollider.gameObject.tag);
    }
}
