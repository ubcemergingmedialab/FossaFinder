using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public enum TransitionType
{
    None,
    Forward,
    Backward,
    Outward,
    Inward
}

public class GuidedTourManager : MonoBehaviour {

    private static GuidedTourManager _instance;
    public static GuidedTourManager Instance
    {
        get { return _instance; }
    }

    public GameObject head, headContainer, cameraRig, mainCamera; 
    public Animator anim;
    public SceneData[] sceneDataArray;

    public delegate void DefaultStateHandler();
    public delegate void DuringTransitionHandler();
    public delegate void ZoomedOutHandler();
    public delegate void SetHighlightsHandler(string[] names);
    public delegate void SetBoundarisHandler(string[] names);
    public static event DefaultStateHandler DefaultState;
    public static event DuringTransitionHandler DuringTransition;
    public static event ZoomedOutHandler ZoomedOut;
    public static event SetHighlightsHandler SetHighlights;
    public static event SetBoundarisHandler SetBoundaries;

    Vector3 adjustedCameraPosition;
    int currentSceneDestination; // the current scene destination number
    static bool isDuringTransition;
    TransitionType currentTransitionType;
    string currentAnimationClipName;
    float currentAnimationClipLength;
    float distanceFromAdjustedCameraPositionThreshold;
    Coroutine runningChangeButtonStatesCoroutine;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        currentSceneDestination = 1;
        isDuringTransition = false;
        currentTransitionType = TransitionType.None;
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

    public bool GetIsDuringTransition()
    {
        return isDuringTransition;
    }

    public TransitionType GetCurrentTransitionType()
    {

        return currentTransitionType;
    }

    // Maintains all necessary variables for transitioning into the previous scene (the scene with the smaller scene number). TransitionToAnotherScene() will handle the actual animation
    public void VisitPreviousScene()
    {
        if (currentSceneDestination > 1)
        {
            currentSceneDestination -= 1;
            isDuringTransition = true;
            currentTransitionType = TransitionType.Backward;
            currentAnimationClipName = sceneDataArray[currentSceneDestination - 1].backwardAnimationClipName;
            currentAnimationClipLength = sceneDataArray[currentSceneDestination - 1].backwardAnimationClipLength;

            PlayTransition();
        }
    }

    // Maintains all necessary variables for transitioning into the next scene (the scene with the greater scene number). TransitionToAnotherScene() will handle the actual animation
    public void VisitNextScene()
    {
        if (currentSceneDestination < sceneDataArray.Length)
        {
            currentSceneDestination += 1;
            isDuringTransition = true;
            currentTransitionType = TransitionType.Forward;
            currentAnimationClipName = sceneDataArray[currentSceneDestination - 1].forwardAnimationClipName;
            currentAnimationClipLength = sceneDataArray[currentSceneDestination - 1].forwardAnimationClipLength;

            PlayTransition();
        }
    }

    public void ZoomInToCurrentScene()
    {
        isDuringTransition = true;
        currentTransitionType = TransitionType.Inward;
        currentAnimationClipName = sceneDataArray[currentSceneDestination - 1].backwardAnimationClipName; // Need to change
        currentAnimationClipLength = sceneDataArray[currentSceneDestination - 1].backwardAnimationClipLength; // Need to change

        PlayTransition();
    }

    public void ZoomOutFromCurrentScene()
    {
        isDuringTransition = true;
        currentTransitionType = TransitionType.Outward;
        currentAnimationClipName = sceneDataArray[currentSceneDestination - 1].forwardAnimationClipName; // Need to change
        currentAnimationClipLength = sceneDataArray[currentSceneDestination - 1].forwardAnimationClipLength; // Need to change

        PlayTransition();
    }

    // Checks whether the skull needs to be adjusted first. Then, plays the appropriate animation.
    void PlayTransition()
    {
        AdjustSkullPositionIfPastThreshold();

        if (!string.IsNullOrEmpty(currentAnimationClipName))
        {
            anim.Play(currentAnimationClipName);
            DuringTransition?.Invoke();
            SetHighlights?.Invoke(sceneDataArray[currentSceneDestination - 1].highlights);
            SetBoundaries?.Invoke(sceneDataArray[currentSceneDestination - 1].boundaries);
        }

        runningChangeButtonStatesCoroutine = StartCoroutine(ChangeButtonStatesAfterTransitionIsCompleted());
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

    IEnumerator ChangeButtonStatesAfterTransitionIsCompleted()
    {
        yield return new WaitForSeconds(currentAnimationClipLength);
        isDuringTransition = false;
        currentTransitionType = TransitionType.None;
        DefaultState?.Invoke();
    }

    // Skips to a particular end state/scene of a transition 
    public void SkipToScene(int sceneNumber)
    {
        StopCoroutine(runningChangeButtonStatesCoroutine);
        anim.Play(currentAnimationClipName, -1, 1);
        isDuringTransition = false;
        DefaultState?.Invoke();
    }

    public void SkipTransition()
    {
        anim.Play(currentAnimationClipName, -1, 1);
        isDuringTransition = false;
        currentTransitionType = TransitionType.None;
        DefaultState?.Invoke();
    }
}
