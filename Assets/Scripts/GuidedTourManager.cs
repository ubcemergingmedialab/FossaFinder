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
    public delegate void EnableBoundariesHandler(string[] names);
    public delegate void DisableBoundariesHandler();
    public delegate void SetlightsHandler(string[] names);
    public delegate void DisableLightsHandler();
    public delegate void SetHighlightsHandler(string[] names);
    public delegate void DisableHighlightsHandler();
    public delegate void DisableLabelsHandler();
    public delegate void SetNerveshandler(string[] names);
    public delegate void DisableNervesHandler();
    public static event DefaultStateHandler DefaultState;
    public static event DuringTransitionHandler DuringTransition;
    public static event ZoomedOutHandler ZoomedOut;
    public static event EnableBoundariesHandler EnableBoundaries;
    public static event DisableBoundariesHandler DisableBoundaries;
    public static event SetlightsHandler Setlights;
    public static event DisableLightsHandler DisableLights;
    public static event SetHighlightsHandler SetHighlights;
    public static event DisableHighlightsHandler DisableHighlights;
    public static event DisableLabelsHandler DisableLabels;
    public static event SetNerveshandler SetNerves;
    public static event DisableNervesHandler DisableNerves;

    Vector3 adjustedCameraPosition;
    int currentSceneNumber; // the current scene destination number
    static bool isDuringTransition;
    TransitionType currentTransitionType;
    string currentAnimationClipName;
    float currentAnimationClipLength;
    float distanceFromAdjustedCameraPositionThreshold;
    Coroutine changeButtonStatesCoroutine;
    bool isChangeButtonStatesCoroutineRunning;

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
        currentSceneNumber = 1;
        isDuringTransition = false;
        currentTransitionType = TransitionType.None;
        distanceFromAdjustedCameraPositionThreshold = 0.2f;
        isChangeButtonStatesCoroutineRunning = false;
        // SetHighlights?.Invoke(sceneDataArray[currentSceneNumber - 1].highlights);
        DisableHighlights?.Invoke();
        DisableLights?.Invoke();
        DisableBoundaries?.Invoke();
        DisableLabels?.Invoke();
        DisableNerves?.Invoke();

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
    public int CurrentSceneNumber
    {
        get { return currentSceneNumber; }
        set { currentSceneNumber = value; }
    }

    public bool GetIsDuringTransition()
    {
        return isDuringTransition;
    }

    public TransitionType GetCurrentTransitionType()
    {

        return currentTransitionType;
    }

    public string CurrentAnimationClipName
    {
        get { return currentAnimationClipName; }
        set { currentAnimationClipName = value; }
    }

    //public void SetCurrentAnimationClipLength(float clipLength)
    //{
    //    currentAnimationClipLength = clipLength;
    //}

    // Maintains all necessary variables for transitioning into the previous scene (the scene with the smaller scene number). TransitionToAnotherScene() will handle the actual animation
    public void VisitPreviousScene()
    {
        // Debug.Log("Before decreement: " + currentSceneNumber);
        if (currentSceneNumber > 1)
        {
            currentSceneNumber -= 1;
            isDuringTransition = true;
            currentTransitionType = TransitionType.Backward;
            currentAnimationClipName = sceneDataArray[currentSceneNumber - 1].backwardAnimationClipName;
            currentAnimationClipLength = sceneDataArray[currentSceneNumber - 1].backwardAnimationClipLength;

            DisableLabels?.Invoke();
            SetHighlights?.Invoke(sceneDataArray[currentSceneNumber - 1].highlights);
            Setlights?.Invoke(sceneDataArray[currentSceneNumber - 1].lights);
            SetNerves?.Invoke(sceneDataArray[currentSceneNumber - 1].nerves);

            PlayTransition();
        }
    }

    // Maintains all necessary variables for transitioning into the next scene (the scene with the greater scene number). TransitionToAnotherScene() will handle the actual animation
    public void VisitNextScene()
    {
        if (currentSceneNumber < sceneDataArray.Length)
        {
            currentSceneNumber += 1;
            isDuringTransition = true;
            currentTransitionType = TransitionType.Forward;
            currentAnimationClipName = sceneDataArray[currentSceneNumber - 1].forwardAnimationClipName;
            currentAnimationClipLength = sceneDataArray[currentSceneNumber - 1].forwardAnimationClipLength;

            DisableLabels?.Invoke();
            SetHighlights?.Invoke(sceneDataArray[currentSceneNumber - 1].highlights);
            Setlights?.Invoke(sceneDataArray[currentSceneNumber - 1].lights);
            SetNerves?.Invoke(sceneDataArray[currentSceneNumber - 1].nerves);

            PlayTransition();
        }
    }

    public void ZoomInToCurrentScene()
    {
        isDuringTransition = true;
        currentTransitionType = TransitionType.Inward;
        currentAnimationClipName = sceneDataArray[currentSceneNumber - 1].ZoomInAnimationClipName;
        currentAnimationClipLength = sceneDataArray[currentSceneNumber - 1].ZoomInAnimationClipLength;

        SetHighlights?.Invoke(sceneDataArray[currentSceneNumber - 1].highlights);
        Setlights?.Invoke(sceneDataArray[currentSceneNumber - 1].lights);
        DisableBoundaries?.Invoke();

        PlayTransition();
    }

    public void ZoomOutFromCurrentScene()
    {
        isDuringTransition = true;
        currentTransitionType = TransitionType.Outward;
        currentAnimationClipName = sceneDataArray[currentSceneNumber - 1].ZoomOutAnimationClipName;
        currentAnimationClipLength = sceneDataArray[currentSceneNumber - 1].ZoomOutAnimationClipLength;

        DisableLabels?.Invoke();
        DisableHighlights?.Invoke();
        DisableLights?.Invoke();

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
        }

        changeButtonStatesCoroutine = StartCoroutine(ChangeButtonStatesAfterTransitionIsCompleted());
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
        isChangeButtonStatesCoroutineRunning = true;
        yield return new WaitForSeconds(currentAnimationClipLength);
        isDuringTransition = false;
        if (currentTransitionType == TransitionType.Forward || currentTransitionType == TransitionType.Backward || currentTransitionType == TransitionType.Inward)
        {
            DefaultState?.Invoke();
        }
        else if (currentTransitionType == TransitionType.Outward)
        {
            ZoomedOut?.Invoke();
            EnableBoundaries?.Invoke(sceneDataArray[currentSceneNumber - 1].boundaries);
        }
        currentTransitionType = TransitionType.None;
        currentAnimationClipName = "";
        currentAnimationClipLength = 0;
        isChangeButtonStatesCoroutineRunning = false;
    }

    public void SkipTransition()
    {
        if (isChangeButtonStatesCoroutineRunning)
        {
            StopCoroutine(changeButtonStatesCoroutine); /// you need this because you don't want this effect to take place unintentionally
            isChangeButtonStatesCoroutineRunning = false;
        }
        anim.Play(currentAnimationClipName, -1, 1);
        isDuringTransition = false;
        currentTransitionType = TransitionType.None;
        currentAnimationClipName = "";
        currentAnimationClipLength = 0;
        DefaultState?.Invoke();
        DisableLabels?.Invoke();
        SetHighlights?.Invoke(sceneDataArray[currentSceneNumber - 1].highlights);
        Setlights?.Invoke(sceneDataArray[currentSceneNumber - 1].lights);
    }
}
