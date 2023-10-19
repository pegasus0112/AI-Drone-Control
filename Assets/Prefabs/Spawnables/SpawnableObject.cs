using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    [Range(3, 20)] public float minDistanceToOther = 3;

    public float hittableMaterialTransparency = 1;

    public GameObject hittablepart;


    private void Start()
    {
        Color materialOfHittable = hittablepart.GetComponent<MeshRenderer>().material.color;
        materialOfHittable.a = hittableMaterialTransparency;
        hittablepart.GetComponent<MeshRenderer>().material.color = materialOfHittable;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minDistanceToOther);
    }
}
