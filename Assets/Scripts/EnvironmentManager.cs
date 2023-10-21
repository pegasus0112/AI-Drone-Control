using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnvironmentManager : MonoBehaviour
{
    public enum Spawnables
    {
        NONE,
        HITTABLEBLOCK,
        BIGGATE,
        SMALLGATE
    }

    [Header("Status")]
    public bool inTraining = false;

    [Space(10)]
    [Header("Settings")]
    [Range(5, 25)] public int environmentSize = 5;
    [Range(1, 40)] public int spawnableCount = 1;
    [Range(30, 500)] public int placementTries = 30;
    [Range(0, 1)] public float hittableColorTransparecy = 1;

    [Space(10)]
    [Header("Setup")]
    public GameObject drone;
    public Transform droneSpawnPoint;

    [Space(5)]
    public Spawnables spawnables;

    [Space(5)]
    public Transform center;

    [Space(5)]
    public Transform ground;
    public Transform top;
    public Transform left;
    public Transform right;
    public Transform front;
    public Transform back;

    [Space(5)]
    public Transform spawnedObjects;
    public GameObject hittableBlock;
    public GameObject smallGate;
    public GameObject bigGate;


    private void Start()
    {
        //setting center
        center.position = new Vector3(center.position.x, center.position.y + 2 * environmentSize, center.position.z);
    }


    private void Update()
    {
        if (inTraining)
        {
            if(spawnables != Spawnables.NONE && spawnedObjects.childCount <= 0)
            {
                EndTraining();
            }
        }
        else
        {
            StartTraining();
        }
    }

    private void StartTraining()
    {
        //removing old objects (appears when training aborted)
        foreach (Transform spawnedObject in spawnedObjects)
        {
            Destroy(spawnedObject.gameObject);
        }

        PlaceWallsBasedOnEnvSize();
        PlaceSpawnables();

        inTraining = true;

        RestartDrone();
    }

    private void RestartDrone()
    {
        drone.GetComponent<Rigidbody>().velocity = Vector3.zero;
        drone.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        drone.transform.position = droneSpawnPoint.position;
        drone.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        drone.SetActive(true);
    }

    private void PlaceSpawnables()
    {
        switch (spawnables)
        {
            case Spawnables.NONE:
                Debug.Log("Envorinment without spawnables");
                break;
            case Spawnables.HITTABLEBLOCK:
                for(int i=0; i< spawnableCount; i++)
                {
                    PlaceSpawnable(hittableBlock);
                }
                break;
            case Spawnables.SMALLGATE:
                for (int i = 0; i < spawnableCount; i++)
                {
                    PlaceSpawnable(smallGate);
                }
                break;
            case Spawnables.BIGGATE:
                for (int i = 0; i < spawnableCount; i++)
                {
                    PlaceSpawnable(bigGate);
                }
                break;
        }
    }

    private void PlaceSpawnable(GameObject toSpawnObject)
    {
        SpawnableObject spawnableObject = toSpawnObject.GetComponent<SpawnableObject>();
        spawnableObject.hittableMaterialTransparency = hittableColorTransparecy;

        float rangeToCheck = spawnableObject.minDistanceToOtherObjects;


        for (int i=0; i < placementTries; i++)
        {
            Vector3 randomPostiton = new Vector3(
                Random.Range( center.transform.position.x - 5 * environmentSize + rangeToCheck, center.transform.position.x + 5 * environmentSize - rangeToCheck),
                Random.Range(center.transform.position.y - 5 * environmentSize + rangeToCheck, center.transform.position.y + 5 * environmentSize - rangeToCheck),
                Random.Range(center.transform.position.z - 5 * environmentSize + rangeToCheck, center.transform.position.z + 5 * environmentSize - rangeToCheck));

            //if no object in range
            if (!Physics.CheckSphere(randomPostiton, rangeToCheck))
            {
                GameObject spawnedObject = Instantiate(toSpawnObject, randomPostiton, Random.rotation);
                spawnedObject.transform.parent = spawnedObjects.transform;
                return;
            }
        }
    }

    private void PlaceWallsBasedOnEnvSize()
    {
        ground.localScale
            = top.localScale
            = front.localScale
            = back.localScale
            = left.localScale
            = right.localScale
            = new Vector3(environmentSize, 1, environmentSize);

        //all center based because of multiple gyms in multiple positions
        Vector3 newPostionTop = center.position;
        newPostionTop.y = newPostionTop.y * 2 ;
        top.position = newPostionTop;

        Vector3 newPostionLeft = center.position;
        newPostionLeft.z = newPostionLeft.z + -5 * environmentSize;
        left.position = newPostionLeft;

        Vector3 newPostionRight = center.position;
        newPostionRight.z = newPostionRight.z + 5 * environmentSize;
        right.position = newPostionRight;

        Vector3 newPostionFront = center.position;
        newPostionFront.x = newPostionFront.x + -5 * environmentSize;
        front.position = newPostionFront;

        Vector3 newPostionBack = center.position;
        newPostionBack.x = newPostionBack.x + 5 * environmentSize;
        back.position = newPostionBack;
    }

    public void EndTraining()
    {
        inTraining = false;
        drone.SetActive(false);
    }
}
