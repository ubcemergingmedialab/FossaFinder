using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boundary_manager : MonoBehaviour
{

    public List<GameObject> availableBoundaries;
    public Material defaultMaterial;



    public void OnEnable()
    {
        GuidedTourManager.EnableBoundaries += EnableBoundaries;
        GuidedTourManager.DisableBoundaries += DisableBoundaries;

    }

    public void OnDisable()
    {
        GuidedTourManager.EnableBoundaries -= EnableBoundaries;
        GuidedTourManager.DisableBoundaries -= DisableBoundaries;

    }

    //initialize list
    public void Start()
    {
    }

    void EnableBoundaries(string[] names)
    {
        // Debug.Log("Boundary_manager names length: " + names.Length);
        foreach (GameObject current in availableBoundaries) 
        {
            current.SetActive(false);
        }

        foreach (string name in names)
        {
            foreach (GameObject current in availableBoundaries)
            {
                if (current.name == name)
                {
                    current.SetActive(true);
                    Debug.Log("BOUNDARY: enabling " + current.name);
                }
            }
        }
    }


    void DisableBoundaries()
    {
        foreach (GameObject current in availableBoundaries)
        {
            current.SetActive(false);
        }
    }
}