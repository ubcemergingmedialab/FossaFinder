using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuManager : MonoBehaviour {
    public GameObject radialMenuSection;

    enum ButtonMode
    {
        None,
        NextScene,
        ZoomIn,
        PreviousScene,
        ZoomOut,
        SkipNextSceneTransition,
        SkipPreviousSceneTransition
    }
    bool isRightButtonActive, isTopButtonActive, isLeftButtonActive, isDownButtonActive;
    ButtonMode currentSelectedButtonMode;
    float thumbStickThreshold;
    Coroutine upgradeButtonModeCoroutine;
    bool isCoroutineRunning;
    GuidedTourManager guidedTourManager;

    // Use this for initialization
    void Start()
    {
        isRightButtonActive = true;
        isTopButtonActive = true; // false
        isLeftButtonActive = true; // false
        isDownButtonActive = true;
        currentSelectedButtonMode = ButtonMode.None;
        thumbStickThreshold = .5f;
        isCoroutineRunning = false;
        guidedTourManager = GuidedTourManager.Instance;
    }

    void OnEnable()
    {
        GuidedTourManager.DefaultState += OnDefaultState;
        GuidedTourManager.DuringTransition += OnDuringTransition;
        GuidedTourManager.ZoomedOut += OnZoomedOut;
    }

    // Update is called once per frame
    void Update () {
        // Debug.Log(isCoroutineRunning);
        Vector2 thumbStickCoordinates = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        if (Mathf.Abs(thumbStickCoordinates.x) >= thumbStickThreshold || Mathf.Abs(thumbStickCoordinates.y) >= thumbStickThreshold)
        {
            HighlightCurrentSelectedButtonAndSetItsMode(thumbStickCoordinates);
        } else
        {
            if (isCoroutineRunning)
            {
                // Debug.Log(isCoroutineRunning);
                StopCoroutine(upgradeButtonModeCoroutine);
                isCoroutineRunning = false;
            }
            DehighlightCurrentSelectedButtonAndPlayAnimation();
        }
	}

    void HighlightCurrentSelectedButtonAndSetItsMode(Vector2 thumbStickCoordinates)
    {
        float angle = Vector2.SignedAngle(Vector2.right, thumbStickCoordinates);
        if (-45 <= angle && angle <= 45 && isRightButtonActive)
        {
            // Debug.Log(currentSelectedButtonMode);
            /// the != NextScene check is to prevent the following code (73~84) from being called too many times; the != SkipNextScene check is to prevent the back and forth buttonMode change (rmb what
            /// the coroutine does) when holding the thumbstick
            if (!guidedTourManager.GetIsDuringTransition() && currentSelectedButtonMode != ButtonMode.NextScene && currentSelectedButtonMode != ButtonMode.SkipNextSceneTransition) 
            {
                // Debug.Log("This should only get called from none to right");
                Debug.Log("Does this still get called during transition? Cuz if so, hmm??");
                if (isCoroutineRunning)
                {
                    StopCoroutine(upgradeButtonModeCoroutine);
                    isCoroutineRunning = false;
                }
                HighlightCurrentSelectedButton(90);
                currentSelectedButtonMode = ButtonMode.NextScene;
                upgradeButtonModeCoroutine = StartCoroutine(UpgradeCurrentSelectedButtonMode());
            }
            /// this is for skipping in the middle of a transition
            else if (guidedTourManager.GetIsDuringTransition() && currentSelectedButtonMode != ButtonMode.SkipNextSceneTransition)
            {
                HighlightCurrentSelectedButton(90);
                currentSelectedButtonMode = ButtonMode.SkipNextSceneTransition;
            }
        }
        //else if (45 <= angle && angle <= 125 && isTopButtonActive)
        //{
        //    if (currentSelectedButtonMode != ButtonMode.ZoomIn)
        //    {
        //        HighlightCurrentSelectedButton(0);
        //        currentSelectedButtonMode = ButtonMode.ZoomIn;
        //    }
        //}
        else if ((angle >= 125 || angle <= -125) && isLeftButtonActive)
        {
            if (!guidedTourManager.GetIsDuringTransition() && currentSelectedButtonMode != ButtonMode.PreviousScene && currentSelectedButtonMode != ButtonMode.SkipPreviousSceneTransition)
            {
                if (isCoroutineRunning)
                {
                    StopCoroutine(upgradeButtonModeCoroutine);
                    isCoroutineRunning = false;
                }
                HighlightCurrentSelectedButton(270);
                currentSelectedButtonMode = ButtonMode.PreviousScene;
                upgradeButtonModeCoroutine = StartCoroutine(UpgradeCurrentSelectedButtonMode());
            }
            else if (guidedTourManager.GetIsDuringTransition() && currentSelectedButtonMode != ButtonMode.SkipPreviousSceneTransition)
            {
                HighlightCurrentSelectedButton(270);
                currentSelectedButtonMode = ButtonMode.SkipPreviousSceneTransition;
            }
        }
        //else if (-125 <= angle && angle <= -45 && isDownButtonActive)
        //{
        //    if (currentSelectedButtonMode != ButtonMode.ZoomOut)
        //    {
        //        HighlightCurrentSelectedButton(180);
        //        currentSelectedButtonMode = ButtonMode.ZoomOut;
        //    }
        //}
        else
        {
            currentSelectedButtonMode = ButtonMode.None;
        }
    }

    IEnumerator UpgradeCurrentSelectedButtonMode()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(1f);
        switch (currentSelectedButtonMode)
        {
            case ButtonMode.NextScene:
                currentSelectedButtonMode = ButtonMode.SkipNextSceneTransition;
                break;
            case ButtonMode.PreviousScene:
                currentSelectedButtonMode = ButtonMode.SkipPreviousSceneTransition;
                break;
        }
        isCoroutineRunning = false;
    }

    void HighlightCurrentSelectedButton(int angle)
    {
        radialMenuSection.SetActive(true);
        radialMenuSection.transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    void DehighlightCurrentSelectedButtonAndPlayAnimation()
    {
        DehighlightCurrentSelectedButton();
        switch (currentSelectedButtonMode)
        {
            case ButtonMode.NextScene:
                guidedTourManager.VisitNextScene();
                currentSelectedButtonMode = ButtonMode.None;
                break;
            case ButtonMode.ZoomIn:
                guidedTourManager.ZoomInToCurrentScene();
                currentSelectedButtonMode = ButtonMode.None;
                break;
            case ButtonMode.PreviousScene:
                guidedTourManager.VisitPreviousScene();
                currentSelectedButtonMode = ButtonMode.None;
                break;
            case ButtonMode.ZoomOut:
                guidedTourManager.ZoomOutFromCurrentScene();
                currentSelectedButtonMode = ButtonMode.None;
                break;
            case ButtonMode.SkipNextSceneTransition:
            case ButtonMode.SkipPreviousSceneTransition:
                guidedTourManager.SkipTransition();
                currentSelectedButtonMode = ButtonMode.None;
                break;
        }
    }

    void DehighlightCurrentSelectedButton()
    {
        radialMenuSection.transform.eulerAngles = new Vector3(0, 0, 0);
        radialMenuSection.SetActive(false);
    }

    void OnDefaultState()
    {
        isRightButtonActive = (guidedTourManager.GetCurrentSceneDestination() != guidedTourManager.sceneDataArray.Length) ? true : false;
        isTopButtonActive = false;
        isLeftButtonActive = (guidedTourManager.GetCurrentSceneDestination() != 1)? true : false;
        isDownButtonActive = true;
    }

    void OnDuringTransition()
    {
        isRightButtonActive = (guidedTourManager.GetCurrentTransitionType() == TransitionType.Forward) ? true : false;
        isLeftButtonActive = (guidedTourManager.GetCurrentTransitionType() == TransitionType.Backward) ? true : false;
        isTopButtonActive = false;
        isDownButtonActive = false;
    }

    void OnZoomedOut()
    {
        isRightButtonActive = false;
        isTopButtonActive = true;
        isLeftButtonActive = false;
        isDownButtonActive = false;
    }
}
