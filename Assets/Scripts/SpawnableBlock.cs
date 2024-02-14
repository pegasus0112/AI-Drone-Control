using UnityEngine;

public class SpawnableBlock : SpawnableObject
{
    private void OnTriggerEnter(Collider other)
    {
        //blocks directly cleared on collision
        cleared = true;
    }
}

