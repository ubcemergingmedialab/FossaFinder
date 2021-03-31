using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionUI : MonoBehaviour {

    GuidedTourManager guidedTourManager;
    public GameObject[] availableButtons;
    public int currentPhase;
    public GameObject currentActiveButton;
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
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDefaultState()
    {
        foreach (GameObject button in availableButtons)
        {
            button.GetComponent<SceneTransitionButton>().SetDefaultState();
        }
        Debug.Log(currentPhase);
        currentActiveButton.GetComponent<SceneTransitionButton>().SetActiveState();
    }

    void OnDuringTransition()
    {
        for (int i = 0; i < availableButtons.Length; i++)
        {
            availableButtons[i].GetComponent<SceneTransitionButton>().SetDisabledState();

            if (currentPhase == i && guidedTourManager.CurrentSceneNumber >= availableButtons[i].GetComponent<SceneTransitionButton>().targetScene)
            {
                currentActiveButton = availableButtons[i];
            } else if (currentPhase == i + 2 && guidedTourManager.CurrentSceneNumber < currentActiveButton.GetComponent<SceneTransitionButton>().targetScene)
            {
                Debug.Log("Phase 2 to 1");
                currentActiveButton = availableButtons[i];
            }
        }
        currentPhase = currentActiveButton.GetComponent<SceneTransitionButton>().phaseNumber;
    }

    // Change the value in guided tour manager to button's target scene, call visit next scene, and set active button to button
    public void ButtonClicked(GameObject button)
    {
        if (currentActiveButton == null)
        {
            currentActiveButton = button;
        } else
        {
           foreach (GameObject b in availableButtons)
            {
                b.GetComponent<SceneTransitionButton>().SetDefaultState();
            }
                currentActiveButton = button;
        }
        currentPhase = currentActiveButton.GetComponent<SceneTransitionButton>().phaseNumber;
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
