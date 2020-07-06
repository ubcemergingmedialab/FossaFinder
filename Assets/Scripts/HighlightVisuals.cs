using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightVisuals : MonoBehaviour {
    
    private Dictionary<string, GameObject> availableHighlights;
    public Material defaultMaterial;
    public Material highlightMaterial;


    public void OnEnable()
    {
        GuidedTourManager.SetHighlights += EnableHighlights;

    }

    public void OnDisable()
    {
        GuidedTourManager.SetHighlights -= EnableHighlights;

    }

    //initialize list
    public void Start()
    {
        availableHighlights = new Dictionary<string, GameObject>();
        foreach(Transform child in transform)
        {
            availableHighlights.Add(child.name, child.gameObject);
        }
    }

    void EnableHighlights(string[] names)
    {
        foreach(KeyValuePair<string, GameObject> pair in availableHighlights)
        {
            pair.Value.GetComponent<Renderer>().material = defaultMaterial;
        }
        foreach(string name in names)
        {
            availableHighlights[name].GetComponent<Renderer>().material = highlightMaterial;
        }
    }

}
