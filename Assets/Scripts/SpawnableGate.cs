using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableGate : SpawnableObject
{
    private HashSet<string> clearedMotors = new HashSet<string>();
    private float lastGateHit = -1;
    public float maxGateClearTime = 4;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Motor>(out Motor motor))
        {
            ResetGateIfTimout();
            clearedMotors.Add(motor.motorPosition.ToString());

            if (clearedMotors.Count == 4)
            {
                cleared = true;
            }
        }
    }

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
