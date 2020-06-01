using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class GuidedTourManager : MonoBehaviour {
    public GameObject head, headContainer, cameraRig, mainCamera; 
    public Animator anim;
    public SceneData[] sceneDataArray;

    int currentSceneDestination; // the current scene number
    int lastVisitedScene; // the previous scene number after a scene transition
    bool isDuringSceneTransition; // whether a scene transition is taking place or not
    float distanceFromDefaultCameraPositionThreshold; // the maximum allowed distance from the default camera position before the user should be teleported
    bool isPastDistanceThreshold; // whether the user is past the aforementioned distance threshold
    Vector2 defaultTopDownCameraCoordinates; // the top down coordinates (x and z values only) of the default camera position

    string currentNameOfAnimationClip;

	// Use this for initialization
	void Start () {
        currentSceneDestination = 1;
        lastVisitedScene = 1;
        isDuringSceneTransition = false;
        distanceFromDefaultCameraPositionThreshold = 0.2f;
        isPastDistanceThreshold = false;
        defaultTopDownCameraCoordinates = new Vector2(0, .5f);

        StartCoroutine(Compensate());
    }

    // Defines the world position of the camera rig and the skull, after the position of the camera is set
    IEnumerator Compensate()
    {
        yield return new WaitForSeconds(.5f);
        cameraRig.transform.position = new Vector3(0, mainCamera.GetComponent<SteamVR_Camera>().head.position.y, .5f) - mainCamera.GetComponent<SteamVR_Camera>().head.position; // mainCamera.GetComponent<SteamVR_Camera>().head.localPosition.y, 1f) - mainCamera.GetComponent<SteamVR_Camera>().head.localPosition
        headContainer.transform.position = new Vector3(0, mainCamera.GetComponent<SteamVR_Camera>().head.position.y, 0);
    }

    // Returns the current scene number.
    public int GetCurrentSceneDestination()
    {
        return currentSceneDestination;
    }

    // Returns whether a scene transition is currently taking place.
    public bool GetIsDuringSceneTransition()
    {
        return isDuringSceneTransition;
    }

    // Maintains all necessary variables for transitioning into the previous scene (the scene with the smaller scene number). Update() will then check these variables and handle the actual movement
    public void SetUpVariablesForPreviousSceneNumber()
    {
        if (currentSceneDestination > 1)
        {
            lastVisitedScene = currentSceneDestination;
            currentSceneDestination -= 1;
            // if Dante's idea doesn't work, check if transition is completed here
            isDuringSceneTransition = true;
            currentNameOfAnimationClip = sceneDataArray[currentSceneDestination - 1].backwardAnimationClipName;
            
            CheckIfPastDistanceThreshold();
        }
    }

    // Maintains all necessary variables for transitioning into the next scene (the scene with the greater scene number). Update() will then check these variables and handle the actual movement
    public void SetUpVariablesForNextSceneNumber()
    {
        lastVisitedScene = currentSceneDestination;
        currentSceneDestination += 1;
        // if Dante's idea doesn't work, check if transition is completed here
        isDuringSceneTransition = true;
        currentNameOfAnimationClip = sceneDataArray[currentSceneDestination - 1].forwardAnimationClipName;

        CheckIfPastDistanceThreshold();
    }

    // Checks whether the user is past the distance threshold based on the default position of the user (positoin of user = position of camera)
    void CheckIfPastDistanceThreshold()
    {
        Vector2 currentTopDownCameraCoordinates = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z);
        if (Vector2.Distance(currentTopDownCameraCoordinates, defaultTopDownCameraCoordinates) > distanceFromDefaultCameraPositionThreshold)
        {
            isPastDistanceThreshold = true;
        }
    }

    // Teleports the user back to its default position
    void TeleportBackToDefaultCameraPosition()
    {
        cameraRig.transform.position = new Vector3(0, cameraRig.transform.position.y, .5f) - new Vector3(mainCamera.GetComponent<SteamVR_Camera>().head.position.x - cameraRig.transform.position.x, 0, mainCamera.GetComponent<SteamVR_Camera>().head.position.z - cameraRig.transform.position.z);
        isPastDistanceThreshold = false;
    }

    // Update is called once per frame
    // If the user is in a scene transition, incrementally changes the transform value of the skull based on which scene the user wants to go to
    void Update () {
        if (isDuringSceneTransition)
        {
            TransitionToAnotherScene();
        }
	}

    // Checks whether the user needs to be teleported first. Then, incrementally changes the transform values of the skull based on which scene the user wants to go to
    void TransitionToAnotherScene()
    {
        if (isPastDistanceThreshold)
        {
            TeleportBackToDefaultCameraPosition();
        }

        // If Dante's idea doesn't work, this is where you check if previous scenes are completed

        if (!string.IsNullOrEmpty(currentNameOfAnimationClip))
        {
            anim.Play(currentNameOfAnimationClip);
        }

        Vector3 endskullPosition = new Vector3(sceneDataArray[currentSceneDestination - 1].endSkullPosition.x, sceneDataArray[currentSceneDestination - 1].endSkullPosition.y + head.transform.position.y, sceneDataArray[currentSceneDestination - 1].endSkullPosition.z);
        if (Vector3.Distance(head.transform.position, endskullPosition) <= 0.01f && 
            Mathf.Abs(Mathf.DeltaAngle(head.transform.rotation.eulerAngles.x, sceneDataArray[currentSceneDestination - 1].endSkullRotation.x)) <= 0.01f &&
            Mathf.Abs(Mathf.DeltaAngle(head.transform.rotation.eulerAngles.y, sceneDataArray[currentSceneDestination - 1].endSkullRotation.y)) <= 0.01f && 
            Mathf.Abs(Mathf.DeltaAngle(head.transform.rotation.eulerAngles.z, sceneDataArray[currentSceneDestination - 1].endSkullRotation.z)) <= 0.01f &&
            Vector3.Distance(head.transform.localScale, sceneDataArray[currentSceneDestination - 1].endSkullScale) <= 0.01f)
        {
            isDuringSceneTransition = false;
        }
    }

    // Skips to a particular end state/scene of a transition 
    public void SkipToScene(int sceneNumber)
    {
        anim.Play(currentNameOfAnimationClip, -1, 1);
        Vector3 endskullPosition = new Vector3(sceneDataArray[currentSceneDestination - 1].endSkullPosition.x, sceneDataArray[currentSceneDestination - 1].endSkullPosition.y + head.transform.position.y, sceneDataArray[currentSceneDestination - 1].endSkullPosition.z);
        head.transform.position = endskullPosition;
        isDuringSceneTransition = false;
    }
}
