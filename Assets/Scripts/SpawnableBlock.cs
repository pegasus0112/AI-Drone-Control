using UnityEngine;

public class SpawnableBlock : SpawnableObject
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        cleared = true;
    }
}

