using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LabelManager : MonoBehaviour {

    // Use this for initialization
    private Dictionary<string, GameObject> availableLabels;
    public void OnEnable()
    {
        GuidedTourManager.DuringSceneTransition += DisableLabels;

    }

    void OnDisable()
    {
        GuidedTourManager.DuringSceneTransition -= DisableLabels;
    }

    //initialize list
    public void Start()
    {
        availableLabels = new Dictionary<string, GameObject>();
        foreach (Transform child in transform)
        {
            availableLabels.Add(child.name, child.gameObject);
            print(child.name);
        }
        int z = availableLabels.Count;
    }

    void DisableLabels()
    {
        int i = 0;
        foreach (KeyValuePair<string, GameObject> pair in availableLabels)
        {
            
            pair.Value.SetActive(false);
            print(pair.Key);
            print(i);
            i++;
        }
    
    }
}
