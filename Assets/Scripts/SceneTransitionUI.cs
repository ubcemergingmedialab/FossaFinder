using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionUI : MonoBehaviour {

    GuidedTourManager guidedTourManager;
    public GameObject[] availableButtons;
    private GameObject currentActiveButton;
    public Animator fadeAnimator;
    public int[] originalSceneNumbers;

    // Use this for initialization
    void Start () {
        guidedTourManager = GameObject.Find("GuidedTourManager").GetComponent<GuidedTourManager>();
        GuidedTourManager.DefaultState += OnDefaultState;
        GuidedTourManager.DuringTransitionEvent += OnDuringTransition;

        for (var i = 0; i < availableButtons.Length; i++)
        {
            availableButtons[i].GetComponent<SceneTransitionButton>().SetDefaultState();
            originalSceneNumbers[i] = availableButtons[i].GetComponent<SceneTransitionButton>().targetScene;
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
        if (currentActiveButton != null)
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

        for (var i = 0; i < availableButtons.Length; i++)
        {
            if (guidedTourManager.CurrentSceneNumber == originalSceneNumbers[i])
            {
                foreach (GameObject b in availableButtons)
                {
                    b.GetComponent<SceneTransitionButton>().SetDefaultState();
                }
                currentActiveButton = availableButtons[i];
                return;
            }
        }
    }

    public void Refresh(GameObject button)
    {
        for (var i = 0; i < availableButtons.Length; i++)
        {
           availableButtons[i].GetComponent<SceneTransitionButton>().targetScene = originalSceneNumbers[i];    
        }
        currentActiveButton = availableButtons[0];
        StartCoroutine(TriggerTransition());
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
                if (guidedTourManager.CurrentSceneNumber < 2)
                {
                    currentActiveButton.GetComponent<SceneTransitionButton>().targetScene = guidedTourManager.CurrentSceneNumber;
                } else
                {
                    currentActiveButton.GetComponent<SceneTransitionButton>().targetScene = guidedTourManager.CurrentSceneNumber - 1;
                }
                
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
        Debug.Log("State before transition: " + guidedTourManager.CurrentSceneNumber);
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
