using UnityEngine;

public class Wall : MonoBehaviour
{

    public Color colour; // color of wall pattern
    public Transform coloredParent;

    void Start()
    {
        // change color of wall pattern at start
        foreach (Transform child in coloredParent)
        {
            child.GetComponent<MeshRenderer>().material.color = colour;
        }
    }
}
