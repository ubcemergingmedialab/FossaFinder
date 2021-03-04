using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionUI : MonoBehaviour {

    GuidedTourManager guidedTourManager;
    bool isAnimationPlaying;

    public GameObject[] availableButtons;
    private GameObject currentActiveButton;
    private SceneTransitionButton transitionButton;

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
        availableButtons.GetComponent<SceneTransitionButton>().SetDisabledColor();
    }

    void OnDuringTransition()
    {           
        // change active button to disabled
        availableButtons.GetComponent<SceneTransitionButton>().SetDisabledColor();   
        // change all buttons to default state
        foreach (GameObject availableButtons in availableButtons)
        {
            availableButtons.OnDefaultState();
        }


    }

    public void ButtonClicked(GameObject button)
    {
        // change value in guided tour manager to button.GetComponent<SceneTransitionButton>().targetScene
        // call guidedTourManager.VisitNextScene()
        // set active button to button
    }
}
