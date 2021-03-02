using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionUI : MonoBehaviour {

    GuidedTourManager guidedTourManager;
    bool isAnimationPlaying;

    public GameObject[] availableButtons;
    private GameObject currentActiveButton;

    // Use this for initialization
    void Start () {
        guidedTourManager = GameObject.Find("GuidedTourManager").GetComponent<GuidedTourManager>();
        GuidedTourManager.DefaultState += OnDefaultState;
        GuidedTourManager.DuringTransitionEvent += OnDuringTransition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDefaultState()
    {
        // change active button to active
    }

    void OnDuringTransition()
    {
        // change all buttons to default state
        // change active button to disabled
    }

    public void ButtonClicked(GameObject button)
    {
        // change value in guided tour manager to button.GetComponent<SceneTransitionButton>().targetScene
        // call guidedTourManager.VisitNextScene()
        // set active button to button
    }
}
