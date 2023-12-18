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
    public DroneAI droneAI;

    //are drone parts okay?
    public bool droneFrameState = true;
    public bool rotorStateLF = true;
    public bool rotorStateRF = true;
    public bool rotorStateLB = true;
    public bool rotorStateRB = true;

    [Header("Status")]
    public bool isGrounded;

    [Space(10)]
    [Header("Settings")]
    [Range(1, 10)] public float maxGroundTime = 4;
    private float lastGroundTime;

    private int lastHitObjectHash = -1;

    //important for resetting state after restarting training
    private void OnEnable()
    {
        lastGroundTime = -1;
        droneFrameState = true;
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
        ResetDroneIfTooLongOnGround();
    }

    private void CheckAnyMotorGrounded()
    {
        isGrounded = droneControl.motorLF.isGrounded
             | droneControl.motorRF.isGrounded
             | droneControl.motorLB.isGrounded
             | droneControl.motorRB.isGrounded;
    }

    public bool CheckPartsAreOkay()
    {
        return droneFrameState && rotorStateLF && rotorStateRF && rotorStateLB && rotorStateRB;
    }
    private void ResetDroneIfTooLongOnGround()
    {
        if(isGrounded)
        {
            float currentTime = Time.time;
            if (isGrounded && lastGroundTime == -1)
            {
                lastGroundTime = Time.time;
            }
            else if (isGrounded && currentTime - lastGroundTime > maxGroundTime)
            {
                droneAI.EndEpisodeBecauseTooLongGrounded();
            }
        } else
        {
            lastGroundTime = -1;
        }

    }

    //walls, ground & gate frame
    private void OnCollisionEnter(Collision collision)
    {
        GameObject ownHittedObject = collision.GetContact(0).thisCollider.gameObject;
        if (ownHittedObject.tag == "Rotor")
        {
            ROTOR hittedMotor = ownHittedObject.transform.parent.parent.GetComponent<Motor>().motorPosition;

            Debug.Log(hittedMotor);

            switch (hittedMotor)
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
        } else if(ownHittedObject.tag == "Drone")
        {
            droneFrameState = false;
        }

        //Debug.Log(collision.GetContact(0).thisCollider.gameObject.tag + " - " + collision.GetContact(0).otherCollider.gameObject.tag);
    }

    //Hittables / Gates
    private void OnTriggerEnter(Collider other)
    {
        GameObject hittedSpawnable = other.gameObject;

        if (hittedSpawnable.tag == "Gate" || hittedSpawnable.tag == "HittableBlock")
        {
            SpawnableObject spawnedObject;

            //gate collider does not have the script & only hittable not complete gate would be destroyed later
            if (hittedSpawnable.tag == "Gate")
            {
                hittedSpawnable = hittedSpawnable.transform.parent.gameObject;
            }

            spawnedObject = hittedSpawnable.GetComponent<SpawnableObject>();

            if (spawnedObject.cleared)
            {
                if(lastHitObjectHash != spawnedObject.GetHashCode())
                {
                    lastHitObjectHash = spawnedObject.GetHashCode();
                    droneAI.Scored(spawnedObject.pointsForClearing);
                    Destroy(hittedSpawnable);
                } else
                {
                    Debug.Log("object already cleared");
                }
            }

        }
    }
}
