using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionUI : MonoBehaviour {

    GuidedTourManager guidedTourManager;
    public GameObject[] availableButtons;
    private GameObject currentActiveButton;
    public Animator fadeAnimator;

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
        if(currentActiveButton != null)
        {
            currentActiveButton.GetComponent<SceneTransitionButton>().SetActiveState();
        }
    }

    void OnDuringTransition()
    {
        Debug.Log("Transition");
        foreach (GameObject button in availableButtons)
        {
            button.GetComponent<SceneTransitionButton>().SetDisabledState();
        }
    }

    // Change the value in guided tour manager to button's target scene, call visit next scene, and set active button to button
    public void ButtonClicked(GameObject button)
    {
        if (currentActiveButton == null)
        {
            currentActiveButton = button;
        } else
        {
            if (button.GetComponent<SceneTransitionButton>().GetButtonState() == SceneTransitionButton.ButtonState.Default)
            {
                currentActiveButton.GetComponent<SceneTransitionButton>().targetScene = guidedTourManager.CurrentSceneNumber - 1;
            }
            foreach (GameObject b in availableButtons)
            {
                b.GetComponent<SceneTransitionButton>().SetDefaultState();
            }
                currentActiveButton = button;
        }
        StartCoroutine(TriggerTransition());
    }


    private IEnumerator TriggerTransition()
    {
        fadeAnimator.Play("FadeToBlack");
        currentActiveButton.GetComponent<SceneTransitionButton>().SetActiveState();
        yield return new WaitForSeconds(0.667f);
        guidedTourManager.CurrentSceneNumber = currentActiveButton.GetComponent<SceneTransitionButton>().targetScene;
        guidedTourManager.VisitNextScene();
        guidedTourManager.SkipTransition();
        yield return new WaitForSeconds(0.667f);
        fadeAnimator.Play("FadeFromBlack");
    }
}
