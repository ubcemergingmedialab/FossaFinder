using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class GuidedTourManager : MonoBehaviour {
    public GameObject head, headContainer, cameraRig, mainCamera; 
    public Animator anim;
    public SceneData[] sceneDataArray;

    public delegate void DefaultStateHandler();
    public delegate void DuringSceneTransitionHandler();
    public delegate void DuringZoomTransitionHandler();
    public delegate void ZoomedOutHandler();
    public static event DefaultStateHandler DefaultState;
    public static event DuringSceneTransitionHandler DuringSceneTransition;
    public static event DuringZoomTransitionHandler DuringZoomTransition;
    public static event ZoomedOutHandler ZoomedOut;

    Vector2 defaultTopDownCameraCoordinates; // the top down coordinates (x and z values only) of the default camera position
    int currentSceneDestination; // the current scene number
    string currentAnimationClipName;
    float currentAnimationClipLength;
    float distanceFromDefaultCameraPositionThreshold; // the maximum allowed distance from the default camera position before the user should be teleported
    bool isPastDistanceThreshold; // whether the user is past the aforementioned distance threshold
    Coroutine runningChangeButtonStatesCoroutine;

    // Use this for initialization
    void Start () {
        defaultTopDownCameraCoordinates = new Vector2(0, .5f);
        currentSceneDestination = 1;
        distanceFromDefaultCameraPositionThreshold = 0.2f;
        isPastDistanceThreshold = false;

        StartCoroutine(AdjustCameraRigAndUserHeight());
    }

    // Defines the world position of the camera rig and the skull, after the position of the camera is set
    IEnumerator AdjustCameraRigAndUserHeight()
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

    // Maintains all necessary variables for transitioning into the previous scene (the scene with the smaller scene number). Update() will then check these variables and handle the actual movement
    public void TransitionToPreviousScene()
    {
        if (currentSceneDestination > 1)
        {
            currentSceneDestination -= 1;
            currentAnimationClipName = sceneDataArray[currentSceneDestination - 1].backwardAnimationClipName;
            currentAnimationClipLength = sceneDataArray[currentSceneDestination - 1].backwardAnimationClipLength;

            TransitionToScene();
        }
    }

    // Maintains all necessary variables for transitioning into the next scene (the scene with the greater scene number). Update() will then check these variables and handle the actual movement
    public void TransitionToNextScene()
    {
        if (currentSceneDestination < sceneDataArray.Length)
        {
            currentSceneDestination += 1;
            currentAnimationClipName = sceneDataArray[currentSceneDestination - 1].forwardAnimationClipName;
            currentAnimationClipLength = sceneDataArray[currentSceneDestination - 1].forwardAnimationClipLength;

            TransitionToScene();
        }
    }

    // Checks whether the user needs to be teleported first. Then, incrementally changes the transform values of the skull based on which scene the user wants to go to
    void TransitionToScene()
    {
        CheckIfPastDistanceThreshold();
        if (isPastDistanceThreshold)
        {
            TeleportBackToDefaultCameraPosition();
        }

        if (!string.IsNullOrEmpty(currentAnimationClipName))
        {
            anim.Play(currentAnimationClipName);
            DuringSceneTransition?.Invoke();
        }

        runningChangeButtonStatesCoroutine = StartCoroutine(ChangeButtonStatesAfterAnimationIsCompleted());
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

    IEnumerator ChangeButtonStatesAfterAnimationIsCompleted()
    {
        yield return new WaitForSeconds(currentAnimationClipLength);
        DefaultState?.Invoke();
    }

    // Skips to a particular end state/scene of a transition 
    public void SkipToScene(int sceneNumber)
    {
        StopCoroutine(runningChangeButtonStatesCoroutine);
        anim.Play(currentAnimationClipName, -1, 1);
        //Vector3 endskullPosition = new Vector3(sceneDataArray[currentSceneDestination - 1].endSkullPosition.x, sceneDataArray[currentSceneDestination - 1].endSkullPosition.y + head.transform.position.y, sceneDataArray[currentSceneDestination - 1].endSkullPosition.z);
        //head.transform.position = endskullPosition;
        DefaultState?.Invoke();
    }
}
