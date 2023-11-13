using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Color colour;
    public Transform coloredParent;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in coloredParent)
        {
            child.GetComponent<MeshRenderer>().material.color = colour;
        }

    }
}
