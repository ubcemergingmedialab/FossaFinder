using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneButtonManager : MonoBehaviour {
    public Button previousSceneNumberButton;
    public Button nextSceneNumberButton;
    public Button skipButton;
    public GuidedTourManager guidedTourManager;

    //void OnEnable()
    //{
    //    GuidedTourManager.DefaultState += OnDefaultState;
    //    GuidedTourManager.DuringSceneTransition += OnDuringSceneTransition;
    //}

    //void OnDisable()
    //{
    //    GuidedTourManager.DefaultState -= OnDefaultState;
    //    GuidedTourManager.DuringSceneTransition -= OnDuringSceneTransition;
    //}

    // Use this for initialization
    void Start () {
        skipButton.GetComponent<Button>().onClick.AddListener(() => guidedTourManager.SkipToScene(guidedTourManager.GetCurrentSceneNumber()));
	}

    void OnDefaultState()
    {
        previousSceneNumberButton.interactable = true;
        nextSceneNumberButton.interactable = true;
        skipButton.interactable = false;
    }

    void OnDuringSceneTransition()
    {
        previousSceneNumberButton.interactable = false;
        nextSceneNumberButton.interactable = false;
        skipButton.interactable = true;
    }
}
