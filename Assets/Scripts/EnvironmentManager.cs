using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public enum Spawnables
    {
        NONE,
        HITTABLEBLOCK,
        BIGGATE,
        SMALLGATE
    }

    public bool inTraining = false;

    [Header("Setup")]
    public GameObject drone;
    [Range(5, 25)] public int environmentSize = 5;

    [Space(10)]
    [Range(0, 1)] public float hittableColorTransparecy = 1;
    public Spawnables spawnables;

    public Transform ground;
    public Transform top;
    public Transform left;
    public Transform right;
    public Transform front;
    public Transform back;

    [Space(10)]
    public GameObject hittableBlock;
    public GameObject smallGate;
    public GameObject bigGate;

    private List<GameObject> spawnedObjects = new();

    // Start is called before the first frame update
    void Start()
    {
        PreapareEnvironment();
    }

    private void Update()
    {
        if (inTraining)
        {
            //checkSpawnabels exist
        }
        else
        {
            PreapareEnvironment();
        }
    }

    private void PreapareEnvironment()
    {
        //removing old objects (appears when training aborted)
        spawnedObjects.ForEach(spawnedObject => Destroy(spawnedObject));

        PlaceWallsBasedOnEnvSize();
        PlaceSpawnables();

        inTraining = true;

        RestartDrone();
    }

    private void RestartDrone()
    {
        drone.GetComponent<Rigidbody>().velocity = Vector3.zero;
        drone.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        drone.transform.position = Vector3.zero;
        drone.transform.rotation = new Quaternion(0, 0, 0, 0);

        drone.SetActive(true);
    }

    private void PlaceSpawnables()
    {
        switch (spawnables)
        {
            case Spawnables.NONE:
                break;
            case Spawnables.HITTABLEBLOCK:

                break;
            case Spawnables.SMALLGATE:
                break;
            case Spawnables.BIGGATE:
                break;
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

        top.position = new Vector3(0, 10 * environmentSize, 0);

        left.position = new Vector3(0, 5 * environmentSize, -5 * environmentSize);
        right.position = new Vector3(0, 5 * environmentSize, 5 * environmentSize);
        front.position = new Vector3(-5 * environmentSize, 5 * environmentSize, 0);
        back.position = new Vector3(5 * environmentSize, 5 * environmentSize, 0);
    }

    public void EndTraining()
    {
        inTraining = false;
        drone.SetActive(false);
    }
}
