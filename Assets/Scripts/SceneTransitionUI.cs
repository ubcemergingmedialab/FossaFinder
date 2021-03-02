using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionUI : MonoBehaviour {

    GuidedTourManager guidedTourManager;
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
        foreach (GameObject button in availableButtons)
        {
            button.GetComponent<SceneTransitionButton>().SetDefaultColor();
        }
        currentActiveButton.GetComponent<SceneTransitionButton>().SetActiveColor();
    }

    // Change all buttons to their default state and change the active button to disabled
    // Shouldn't all buttons be disabled?
    void OnDuringTransition()
    {
        foreach (GameObject button in availableButtons)
        {
            button.GetComponent<SceneTransitionButton>().SetDisabledColor();
        }
    }

    // Change the value in guided tour manager to button's target scene, call visit next scene, and set active button to button
    public void ButtonClicked(GameObject button)
    {
        button.GetComponent<SceneTransitionButton>().SetActiveColor();
        currentActiveButton.GetComponent<SceneTransitionButton>().SetDefaultColor();
        currentActiveButton = button;

        guidedTourManager.CurrentSceneNumber = currentActiveButton.GetComponent<SceneTransitionButton>().targetScene;
        guidedTourManager.VisitNextScene();
    }
}
