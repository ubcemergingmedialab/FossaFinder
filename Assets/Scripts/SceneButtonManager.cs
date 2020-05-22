using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneButtonManager : MonoBehaviour {

    public Button previousSceneButtonScript;
    public Button nextSceneButtonScript;
    public Button skipButtonScript;
    public GuidedTourManager guidedTourManagerScript;

    bool isDuringSceneTransition;

	// Use this for initialization
	void Start () {
        // isDuringSceneTransition = guidedTourManager.GetComponent<GuidedTourManager>().GetIsDuringSceneTransition();
        skipButtonScript.GetComponent<Button>().onClick.AddListener(() => guidedTourManagerScript.SkipToScene(guidedTourManagerScript.GetCurrentSceneNumber()));
	}
	
	// Update is called once per frame
	void Update () {
		if (guidedTourManagerScript.GetIsDuringSceneTransition())
        {
            previousSceneButtonScript.interactable = false;
            nextSceneButtonScript.interactable = false;
            skipButtonScript.interactable = true;
        } else
        {
            previousSceneButtonScript.interactable = true;
            nextSceneButtonScript.interactable = true;
            skipButtonScript.interactable = false;
        }
	}
}
