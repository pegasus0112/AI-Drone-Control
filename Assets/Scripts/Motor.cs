using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DroneHandler;

public class Motor : MonoBehaviour
{
    [Header("Settings")]
    [Range(0, 1)] public float motorSpeed = 0;
    public ROTOR motorPosition;

    //1 == cw
    //-1 == ccw
    private float spinDirection = 1;

    public float maxRPM = 3500;
    public GameObject rotor;

    [Range(1, 25)] public float forceMultiplier = 1;

    [Space(10)]
    [Header("GroundCheck")]
    public bool isGrounded = true;
    [Range(0.001f, 0.1f)] public float toCheckGroundDistance = 0.02f;
    public Transform groundCheckPoint;


    [Space(10)]
    [Header("Physics")]
    public Transform forcePoint;

    //drone rigidbody shared by all motors
    public Rigidbody droneRig;

    // Start is called before the first frame update
    void Start()
    {
        if (motorPosition == ROTOR.RF || motorPosition == ROTOR.LB)
        {
            spinDirection = -1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        droneRig.AddForceAtPosition(transform.up * motorSpeed * forceMultiplier, forcePoint.position);
        rotor.transform.Rotate(Vector3.up, spinDirection * Remap(motorSpeed, 0, 1, 360, maxRPM) * Time.deltaTime);

        //TODO: GroundCheck
        isGrounded = CheckGrounded();

    }

    private bool CheckGrounded()
    {
        Collider[] hitColliders = Physics.OverlapSphere(groundCheckPoint.position, toCheckGroundDistance);
        return Array.Exists(hitColliders, coll => coll.tag == "Ground" || coll.tag == "Wall");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(groundCheckPoint.position, toCheckGroundDistance);
    }

    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
