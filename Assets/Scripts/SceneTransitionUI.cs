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
        foreach (GameObject button in availableButtons)
        {
            button.GetComponent<SceneTransitionButton>().SetDisabledState();
        }
    }

    public void Refresh(GameObject button)
    {
        for (var i = 0; i < availableButtons.Length; i++)
        {
           availableButtons[i].GetComponent<SceneTransitionButton>().targetScene = button.GetComponent<RefreshButton>().originalSceneNumbers[i];
        }
        currentActiveButton = availableButtons[0];
        StartCoroutine(TriggerRefresh());
    }

    private IEnumerator TriggerRefresh()
    {
        fadeAnimator.Play("FadeToBlack");
        currentActiveButton.GetComponent<SceneTransitionButton>().SetActiveState();
        yield return new WaitForSeconds(0.667f);
        guidedTourManager.CurrentSceneNumber = currentActiveButton.GetComponent<SceneTransitionButton>().targetScene;
        guidedTourManager.VisitNextScene();
        guidedTourManager.SkipTransition();
        guidedTourManager.VisitPreviousScene();
        guidedTourManager.SkipTransition();
        yield return new WaitForSeconds(0.667f*2f);
        fadeAnimator.Play("FadeFromBlack");
    }


    public void ButtonClicked(GameObject button)
    {
        if (currentActiveButton == null)
        {
            currentActiveButton = button;
        } else
        {
            // If the button is not currently active or disabled, change the target scene to the current scene
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
