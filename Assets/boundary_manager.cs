using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boundary_manager : MonoBehaviour
{

    private Dictionary<string, GameObject> availableBoundaries;
    public Material defaultMaterial;
    //public Material highlightMaterial1;
    //public Material highlightMaterial2;
    //public Material highlightMaterial3;


    public void OnEnable()
    {
        GuidedTourManager.SetBoundaries += EnableHighlights;

    }

    public void OnDisable()
    {
        GuidedTourManager.SetBoundaries -= EnableHighlights;

    }

    //initialize list
    public void Start()
    {
        availableBoundaries = new Dictionary<string, GameObject>();
        foreach (Transform child in transform)
        {
            availableBoundaries.Add(child.name, child.gameObject);
        }
    }

    void EnableHighlights(string[] names)
    {
        foreach (KeyValuePair<string, GameObject> pair in availableBoundaries)
        {
            pair.Value.SetActive(false);
        }
        foreach (string name in names)
        {
           availableBoundaries[name].SetActive(true);
        }
    }
}