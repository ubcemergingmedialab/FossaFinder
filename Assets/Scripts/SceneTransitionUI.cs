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

        foreach (GameObject button in availableButtons)
        {
            button.GetComponent<SceneTransitionButton>().SetDefaultState();
        }
        currentActiveButton = null;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDefaultState()
    {
        Debug.Log("Default State");
        foreach (GameObject button in availableButtons)
        {
            button.GetComponent<SceneTransitionButton>().SetDefaultState();
        }
        currentActiveButton.GetComponent<SceneTransitionButton>().SetActiveState();
    }

    void OnDuringTransition()
    {
        Debug.Log("Transition");
        foreach (GameObject button in availableButtons)
        {
            button.GetComponent<SceneTransitionButton>().SetDefaultState();
        }
        currentActiveButton.GetComponent<SceneTransitionButton>().SetDisabledState();
    }

    // Change the value in guided tour manager to button's target scene, call visit next scene, and set active button to button
    public void ButtonClicked(GameObject button)
    {
        if (currentActiveButton == null)
        {
            Debug.Log("Setting currentActiveButton");
            currentActiveButton = button;
        } else
        {
            if (GameObject.ReferenceEquals(currentActiveButton, button))
            {
                // do nothing
            } else
            {
                foreach (GameObject b in availableButtons)
                {
                    b.GetComponent<SceneTransitionButton>().SetDefaultState();
                }
                currentActiveButton = button;
            }
        }
        currentActiveButton.GetComponent<SceneTransitionButton>().SetActiveState();
        guidedTourManager.CurrentSceneNumber = currentActiveButton.GetComponent<SceneTransitionButton>().targetScene;
        guidedTourManager.VisitNextScene();
    }
}
