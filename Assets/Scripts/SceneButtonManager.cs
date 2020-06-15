using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneButtonManager : MonoBehaviour {
    public Button previousSceneNumberButton;
    public Button nextSceneNumberButton;
    public Button skipButton;
    public GuidedTourManager guidedTourManager;

    bool isDuringSceneTransition;

	// Use this for initialization
	void Start () {
        // isDuringSceneTransition = guidedTourManager.GetComponent<GuidedTourManager>().GetIsDuringSceneTransition();
        skipButton.GetComponent<Button>().onClick.AddListener(() => guidedTourManager.SkipToScene(guidedTourManager.GetCurrentSceneDestination()))

        //register subscribers to GuidedTourManager's events here
	}
	
	// Update is called once per frame
    // Checks whether a scene transition is taking place every frame. Adjust the interactivity of the buttons accordingly.
	void Update () {
		if (guidedTourManager.GetIsDuringSceneTransition())
        {
            previousSceneNumberButton.interactable = false;
            nextSceneNumberButton.interactable = false;
            skipButton.interactable = true;
        } else
        {
            previousSceneNumberButton.interactable = true;
            nextSceneNumberButton.interactable = true;
            skipButton.interactable = false;
        }
	}

    //isDuringSceneTransition
     //          previousSceneNumberButton false
     //         nextButton false
     //         skip true
     //         zoom false
     //
     //
      // isOutOfSceneTransition
     //         previous true
     //         next true
      //         skip false
      //         zoom true
     //
      
    //
     // isZoomed
     //          previous false
     //         next false
    //          skip false
     //          zoom true
     //
}
