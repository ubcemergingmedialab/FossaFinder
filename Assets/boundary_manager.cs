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

        GuidedTourManager.InitializeEvent += DisableBoundaries;
        GuidedTourManager.ZoomInEvent += DisableBoundaries;

    }

    public void OnDisable()
    {
        GuidedTourManager.EnableBoundaries -= EnableBoundaries;

        GuidedTourManager.InitializeEvent -= DisableBoundaries;
        GuidedTourManager.ZoomInEvent -= DisableBoundaries;

    }

    //initialize list
    public void Start()
    {
    }

    void EnableBoundaries(string[] names)
    {
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

    void EnableBoundaries(SceneData sceneData)
    {
        string[] names = sceneData.lights;
        EnableBoundaries(names);
    }


    void DisableBoundaries()
    {
        foreach (GameObject current in availableBoundaries)
        {
            current.SetActive(false);
        }
    }

    void DisableBoundaries(SceneData sceneData)
    {
        DisableBoundaries();
    }
}