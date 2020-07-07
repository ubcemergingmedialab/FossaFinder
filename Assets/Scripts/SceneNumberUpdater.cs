using UnityEngine;
using UnityEngine.UI;
using System;

public class SceneNumberUpdater : MonoBehaviour {
    public GameObject guidedTourManager;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Text>().text = guidedTourManager.GetComponent<GuidedTourManager>().GetCurrentSceneDestination().ToString();
    }
}
