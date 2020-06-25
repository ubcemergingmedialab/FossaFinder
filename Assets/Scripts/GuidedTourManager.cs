using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class GuidedTourManager : MonoBehaviour {
    public GameObject head, headContainer, cameraRig, mainCamera; 
    public Animator anim;
    public SceneData[] sceneDataArray;
    public Material highlightmaterial;
    public Material defaultmaterial;

    public delegate void DefaultStateHandler();
    public delegate void DuringSceneTransitionHandler();
    public delegate void DuringZoomTransitionHandler();
    public delegate void ZoomedOutHandler();
    public static event DefaultStateHandler DefaultState;
    public static event DuringSceneTransitionHandler DuringSceneTransition;
    public static event DuringZoomTransitionHandler DuringZoomTransition;
    public static event ZoomedOutHandler ZoomedOut;

    Vector3 adjustedCameraPosition;
    int currentSceneDestination; // the current scene destination number
    string currentAnimationClipName;
    float currentAnimationClipLength;
    float distanceFromAdjustedCameraPositionThreshold;
    Coroutine runningChangeButtonStatesCoroutine;

    // Use this for initialization
    void Start () {
        currentSceneDestination = 1;
        distanceFromAdjustedCameraPositionThreshold = 0.2f;

        StartCoroutine(AdjustCameraRigAndUserHeight());
    }

    // Defines the world position of the camera rig and the skull, after the position of the camera is set
    IEnumerator AdjustCameraRigAndUserHeight()
    {
        yield return new WaitForSeconds(.5f);
        cameraRig.transform.position = new Vector3(0, mainCamera.GetComponent<SteamVR_Camera>().head.position.y, .5f) - mainCamera.GetComponent<SteamVR_Camera>().head.position; // mainCamera.GetComponent<SteamVR_Camera>().head.localPosition.y, 1f) - mainCamera.GetComponent<SteamVR_Camera>().head.localPosition
        headContainer.transform.position = new Vector3(0, mainCamera.GetComponent<SteamVR_Camera>().head.position.y, 0);
        adjustedCameraPosition = mainCamera.transform.position;
    }

    // Returns the current scene number
    public int GetCurrentSceneDestination()
    {
        return currentSceneDestination;
    }

    // Maintains all necessary variables for transitioning into the previous scene (the scene with the smaller scene number). TransitionToAnotherScene() will handle the actual animation
    public void TransitionToPreviousScene()
    {
        Cleanthehighlights();
        if (currentSceneDestination > 1)
        {
            currentSceneDestination -= 1;
            currentAnimationClipName = sceneDataArray[currentSceneDestination - 1].backwardAnimationClipName;
            currentAnimationClipLength = sceneDataArray[currentSceneDestination - 1].backwardAnimationClipLength;

            TransitionToAnotherScene();
        }
    }

    // Maintains all necessary variables for transitioning into the next scene (the scene with the greater scene number). TransitionToAnotherScene() will handle the actual animation
    public void TransitionToNextScene()
    {
        Cleanthehighlights();
        if (currentSceneDestination < sceneDataArray.Length)
        {
            currentSceneDestination += 1;
            currentAnimationClipName = sceneDataArray[currentSceneDestination - 1].forwardAnimationClipName;
            currentAnimationClipLength = sceneDataArray[currentSceneDestination - 1].forwardAnimationClipLength;

            

            TransitionToAnotherScene();
        }
    }

    // Checks whether the skull needs to be adjusted first. Then, plays the appropriate animation.
    void TransitionToAnotherScene()
    {
        AdjustSkullPositionIfPastThreshold();
        if (sceneDataArray[currentSceneDestination - 1].highlights.Length != 0)
        {
            GameObject target;
            Renderer rend;
            foreach (string highlight in sceneDataArray[currentSceneDestination - 1].highlights)
            {
                //print(highlight);
                target = GameObject.Find(highlight);
                rend = target.GetComponent<Renderer>();
                rend.material = highlightmaterial;
            }
        }

        if (!string.IsNullOrEmpty(currentAnimationClipName))
        {
            anim.Play(currentAnimationClipName);
            DuringSceneTransition?.Invoke();
        }

        runningChangeButtonStatesCoroutine = StartCoroutine(ChangeButtonStatesAfterAnimationIsCompleted());
    }

    void Cleanthehighlights() {
        GameObject[] all= GameObject.FindGameObjectsWithTag("highlight");
        foreach (GameObject highlight in all) {
            highlight.GetComponent<Renderer>().material = defaultmaterial;
        
        }
    }
    void AdjustSkullPositionIfPastThreshold()
    {
        Vector3 currentCameraPosition = mainCamera.transform.position;
        if (Vector3.Distance(currentCameraPosition, adjustedCameraPosition) > distanceFromAdjustedCameraPositionThreshold)
        {
            Vector3 offset = new Vector3(currentCameraPosition.x - adjustedCameraPosition.x, currentCameraPosition.y - adjustedCameraPosition.y, currentCameraPosition.z - adjustedCameraPosition.z);
            headContainer.transform.position += offset;
            adjustedCameraPosition = mainCamera.transform.position;
        }
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
        DefaultState?.Invoke();
    }
}
