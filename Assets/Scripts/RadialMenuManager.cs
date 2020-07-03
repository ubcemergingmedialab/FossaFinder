using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuManager : MonoBehaviour {
    enum ButtonMode
    {
        None,
        GotoNextScene,
        ZoomIn,
        GotoPreviousScene,
        ZoomOut,
        SkipNextSceneTransition,
        SkipPreviousSceneTransition
    }
    bool isRightButtonActive, isTopButtonActive, isLeftButtonActive, isDownButtonActive;
    ButtonMode currentSelectedButtonMode;
    float thumbStickThreshold;
    Coroutine runningCoroutine;

    GuidedTourManager guidedTourManager;

    // Use this for initialization
    void Start()
    {
        isRightButtonActive = true;
        isTopButtonActive = false;
        isLeftButtonActive = false;
        isDownButtonActive = true;
        currentSelectedButtonMode = ButtonMode.None;
        thumbStickThreshold = .8f;

        guidedTourManager = GuidedTourManager.Instance;
    }

    // Update is called once per frame
    void Update () {
        Vector2 thumbStickCoordinates = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        if (thumbStickCoordinates.x >= thumbStickThreshold || thumbStickCoordinates.y >= thumbStickThreshold)
        {
            HighlightCurrentSelectedButtonAndSetItsMode(thumbStickCoordinates);
        } else
        {
            StopCoroutine(runningCoroutine);
            DehighlightCurrentSelectedButtonAndPlayAnimation();
        }
	}

    void HighlightCurrentSelectedButtonAndSetItsMode(Vector2 thumbStickCoordinates)
    {
        float angle = Vector2.SignedAngle(Vector2.right, thumbStickCoordinates);
        if (-45 <= angle && angle <= 45 && isRightButtonActive)
        {
            if (currentSelectedButtonMode != ButtonMode.GotoNextScene)
            {
                StopCoroutine(runningCoroutine);
                HighlightCurrentSelectedButton();
                currentSelectedButtonMode = ButtonMode.GotoNextScene;
                runningCoroutine = StartCoroutine(UpgradeCurrentSelectedButtonMode());
            } else if (guidedTourManager.GetIsDuringSceneTransition() && currentSelectedButtonMode != ButtonMode.SkipNextSceneTransition)
            {
                HighlightCurrentSelectedButton();
                currentSelectedButtonMode = ButtonMode.SkipNextSceneTransition;
            }
        } else if (45 <= angle && angle <= 125 && isTopButtonActive)
        {
            if (currentSelectedButtonMode != ButtonMode.ZoomIn)
            {
                HighlightCurrentSelectedButton();
                currentSelectedButtonMode = ButtonMode.ZoomIn;
            }
        } else if ((angle >= 125 || angle <= -125) && isLeftButtonActive)
        {
            if (currentSelectedButtonMode != ButtonMode.GotoPreviousScene)
            {
                StopCoroutine(runningCoroutine);
                HighlightCurrentSelectedButton();
                currentSelectedButtonMode = ButtonMode.GotoPreviousScene;
                runningCoroutine = StartCoroutine(UpgradeCurrentSelectedButtonMode());
            }
            else if (guidedTourManager.GetIsDuringSceneTransition() && currentSelectedButtonMode != ButtonMode.SkipPreviousSceneTransition)
            {
                HighlightCurrentSelectedButton();
                currentSelectedButtonMode = ButtonMode.SkipPreviousSceneTransition;
            }
        } else if (-125 <= angle && angle <= -45 && isDownButtonActive)
        {
            if (currentSelectedButtonMode != ButtonMode.ZoomOut)
            {
                HighlightCurrentSelectedButton();
                currentSelectedButtonMode = ButtonMode.ZoomOut;
            }
        } else
        {
            currentSelectedButtonMode = ButtonMode.None;
        }
    }

    IEnumerator UpgradeCurrentSelectedButtonMode()
    {
        yield return new WaitForSeconds(1f);
        switch (currentSelectedButtonMode)
        {
            case ButtonMode.GotoNextScene:
                currentSelectedButtonMode = ButtonMode.SkipNextSceneTransition;
                break;
            case ButtonMode.GotoPreviousScene:
                currentSelectedButtonMode = ButtonMode.SkipPreviousSceneTransition;
                break;
        }
    }

    void HighlightCurrentSelectedButton()
    {

    }

    void DehighlightCurrentSelectedButtonAndPlayAnimation()
    {
        DehighlightCurrentSelectedButton();
        switch (currentSelectedButtonMode)
        {
            case ButtonMode.GotoNextScene:
                guidedTourManager.TransitionToNextScene();
                break;
            case ButtonMode.ZoomIn:
                guidedTourManager.ZoomInToCurrentScene();
                break;
            case ButtonMode.GotoPreviousScene:
                guidedTourManager.TransitionToPreviousScene();
                break;
            case ButtonMode.ZoomOut:
                guidedTourManager.ZoomOutFromCurrentScene();
                break;
            case ButtonMode.SkipNextSceneTransition:
            case ButtonMode.SkipPreviousSceneTransition:
                guidedTourManager.SkipTransition();
                break;
        }
    }

    void DehighlightCurrentSelectedButton()
    {

    }
}
