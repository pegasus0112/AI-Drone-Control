using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    [Header("State")]
    public bool cleared = false;

    [Header("Setup")]
    public int pointsForClearing = 1;
    public EnvironmentManager.Spawnables type;

    public float maxGateClearTime = 4;

    [Range(3, 20)] public float minDistanceToOtherObjects = 3;
    public float hittableMaterialTransparency = 1;

    public SphereCollider sphereCollider;

    public GameObject hittablepart;

    private HashSet<string> clearedMotors = new HashSet<string>();

    private void Start()
    {
        sphereCollider.radius = minDistanceToOtherObjects;

        Color materialOfHittable = hittablepart.GetComponent<MeshRenderer>().material.color;
        materialOfHittable.a = hittableMaterialTransparency;
        hittablepart.GetComponent<MeshRenderer>().material.color = materialOfHittable;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minDistanceToOtherObjects);
    }

    public void GateHitted()
    {

    }

    private float lastGateHit = -1;

    private void OnTriggerEnter(Collider other)
    {
        if (type == EnvironmentManager.Spawnables.SMALLGATE || type == EnvironmentManager.Spawnables.BIGGATE)
        {
            if (other.gameObject.TryGetComponent<Motor>(out Motor motor))
            {
                ResetGateIfTimout();
                clearedMotors.Add(motor.motorPosition.ToString());

                Debug.Log(Time.time);

                if (clearedMotors.Count == 4)
                {
                    cleared = true;
                }
            }
        }
        else
        {
            cleared = true;
        }

    }

    //resetting gate if clear takes to long
    //IMPORTANT: way cheaper because not in update and to permanent check
    private void ResetGateIfTimout()
    {
        float currentTime = Time.time;

        if (clearedMotors.Count == 0)
        {
            lastGateHit = currentTime;
        }
        else if (currentTime - lastGateHit > maxGateClearTime)
        {
            clearedMotors.Clear();
        }

        lastGateHit = Time.time;
    }
}
