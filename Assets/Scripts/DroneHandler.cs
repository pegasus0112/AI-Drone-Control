using System;
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

    //are parts okay
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

    //resetting drone states after restarting training
    private void OnEnable()
    {
        lastGroundTime = -1;
        droneFrameState = true;
        rotorStateLF = true;
        rotorStateRF = true;
        rotorStateLB = true;
        rotorStateRB = true;
    }

    //everything except controle and AI
    void FixedUpdate()
    {
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
        if (isGrounded)
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
        }
        else
        {
            lastGroundTime = -1;
        }
    }

    //called for walls, ground or gate frame
    private void OnCollisionEnter(Collision collision)
    {
        GameObject ownHittedObject = collision.GetContact(0).thisCollider.gameObject;
        if (ownHittedObject.CompareTag("Rotor"))
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
        }
        else if (ownHittedObject.CompareTag("Drone"))
        {
            droneFrameState = false;
        }
    }

    //called for hittable parts of spawnables 
    private void OnTriggerEnter(Collider other)
    {
        GameObject hittedSpawnable = other.gameObject;

        if (hittedSpawnable.CompareTag("Gate") || hittedSpawnable.CompareTag("HittableBlock"))
        {
            SpawnableObject spawnedObject;

            if (hittedSpawnable.CompareTag("Gate"))
            {
                //hittable part, not gate collider has script
                hittedSpawnable = hittedSpawnable.transform.parent.gameObject;
            }

            spawnedObject = hittedSpawnable.GetComponent<SpawnableObject>();

            if (spawnedObject.cleared)
            {
                //check already collided with same cleared object
                if (lastHitObjectHash != spawnedObject.GetHashCode())
                {
                    lastHitObjectHash = spawnedObject.GetHashCode();
                    droneAI.Scored(spawnedObject.pointsForClearing);
                    Destroy(hittedSpawnable);
                }
                else
                {
                    // multiple hits with same cleared object
                    Debug.Log("object already cleared");
                }
            }
        }
    }
}
