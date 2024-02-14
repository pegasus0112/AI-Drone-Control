using System;
using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    [Header("State")]
    public bool cleared = false;

    [Header("Setup")]
    public int pointsForClearing = 1;

    [Range(3, 20)] public float minDistanceToOtherObjects = 3;
    public float hittableMaterialTransparency = 1;

    public GameObject hittablepart;

    private void Start()
    {
        //setting color & transparency of hittable part
        Color materialOfHittable = hittablepart.GetComponent<MeshRenderer>().material.color;
        materialOfHittable.a = hittableMaterialTransparency;
        hittablepart.GetComponent<MeshRenderer>().material.color = materialOfHittable;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //draw placement distance to other objects
        Gizmos.DrawWireSphere(transform.position, minDistanceToOtherObjects);
    }
}
