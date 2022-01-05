﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuManager : MonoBehaviour {
    enum ButtonType
    {
        None, Left, Right, Top, Down, LeftOuter, RightOuter
    }
    public GameObject leftButton, rightButton, topButton, downButton, leftOuterButton, rightOuterButton;
    ButtonType currentSelectedButtonType;
    ButtonType previousSelectedButtonType;
    float thumbStickThreshold;
    Coroutine upgradeButtonCoroutine;
    bool isUpgradeButtonCoroutineRunning;
    GuidedTourManager guidedTourManager;
    // bool isThumbstickHeldAfterTransition;


    // Use this for initialization
    void Start()
    {
        leftButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
        leftOuterButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
        topButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
        downButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
        Debug.Log("Ok, down button current state plz: " + downButton.GetComponent<RadialMenuButton>().CurrentState);
        // currentSelectedButtonType = ButtonType.None;
        // previousSelectedButtonType = ButtonType.None;
        thumbStickThreshold = .7f;
        isUpgradeButtonCoroutineRunning = false;
        guidedTourManager = GuidedTourManager.Instance;
        // isThumbstickHeldAfterTransition = false;
    }

    void OnEnable()
    {
        GuidedTourManager.DefaultState += OnDefaultState;
        GuidedTourManager.DuringTransitionEvent += OnDuringTransition;
        GuidedTourManager.ZoomedOut += OnZoomedOut;
    }

    // Update is called once per frame
    void Update () {
        // Debug.Log(rightButton.GetComponent<RadialMenuButton>().CurrentState);
        Vector2 thumbStickCoordinates = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        if (Mathf.Abs(thumbStickCoordinates.x) >= thumbStickThreshold || Mathf.Abs(thumbStickCoordinates.y) >= thumbStickThreshold)
        {
            SelectButtonBasedOnThumbstickCoordinates(thumbStickCoordinates);
            //if (!isThumbstickHeldAfterTransition) {}
        } else
        {
            previousSelectedButtonType = currentSelectedButtonType;
            DeselectButton();
            currentSelectedButtonType = ButtonType.None;
            if (isUpgradeButtonCoroutineRunning)
            {
                // Debug.Log(isCoroutineRunning);
                StopCoroutine(upgradeButtonCoroutine);
                isUpgradeButtonCoroutineRunning = false;
            }
            PlayTransition();
            //if (!guidedTourManager.GetIsDuringTransition()) {isThumbstickHeldAfterTransition = false;}
        }
	}

    void SelectButtonBasedOnThumbstickCoordinates(Vector2 thumbStickCoordinates)
    {
        float angle = Vector2.SignedAngle(Vector2.right, thumbStickCoordinates);
        if (-45 <= angle && angle <= 45)
        {
            // Debug.Log(currentSelectedButtonMode);
            /// the != NextScene check is to prevent the following code (73~84) from being called too many times; the != SkipNextScene check is to prevent the back and forth buttonMode change (rmb what
            /// the coroutine does) when holding the thumbstick; the !gtm check is to make sure the following code doesn't get called during transition; 
            if (!guidedTourManager.GetIsDuringTransition() && rightButton.GetComponent<RadialMenuButton>().CurrentState != ButtonState.Disabled && currentSelectedButtonType != ButtonType.Right &&
                currentSelectedButtonType != ButtonType.RightOuter) 
            {
                // Debug.Log("This should only get called from none to right");
                // Debug.Log("Does this still get called during transition? Cuz if so, hmm??");
                if (isUpgradeButtonCoroutineRunning)
                {
                    StopCoroutine(upgradeButtonCoroutine);
                    isUpgradeButtonCoroutineRunning = false;
                }
                DeselectButton();
                SelectButton(rightButton);
                currentSelectedButtonType = ButtonType.Right;
                // Debug.Log("is SelectButtonStates being called");
                upgradeButtonCoroutine = StartCoroutine(UpgradeCurrentSelectedButton());
            }
            /// this is for skipping in the middle of a transition
            else if (guidedTourManager.GetIsDuringTransition() && rightOuterButton.GetComponent<RadialMenuButton>().CurrentState != ButtonState.Disabled &&
                currentSelectedButtonType != ButtonType.RightOuter)
            {
                // Debug.Log("is this being called when during transition?");
                DeselectButton();
                SelectButton(rightOuterButton);
                currentSelectedButtonType = ButtonType.RightOuter;
            }
        }
        else if (45 <= angle && angle <= 125 && topButton.GetComponent<RadialMenuButton>().CurrentState != ButtonState.Disabled)
        {
            if (currentSelectedButtonType != ButtonType.Top)
            {
                DeselectButton();
                SelectButton(topButton);
                currentSelectedButtonType = ButtonType.Top;
            }
        }
        else if (angle >= 125 || angle <= -125)
        {
            if (!guidedTourManager.GetIsDuringTransition() && leftButton.GetComponent<RadialMenuButton>().CurrentState != ButtonState.Disabled && currentSelectedButtonType != ButtonType.Left &&
                currentSelectedButtonType != ButtonType.LeftOuter)
            {
                if (isUpgradeButtonCoroutineRunning)
                {
                    StopCoroutine(upgradeButtonCoroutine);
                    isUpgradeButtonCoroutineRunning = false;
                }
                DeselectButton();
                SelectButton(leftButton);
                currentSelectedButtonType = ButtonType.Left;
                upgradeButtonCoroutine = StartCoroutine(UpgradeCurrentSelectedButton());
            }
            else if (guidedTourManager.GetIsDuringTransition() && leftOuterButton.GetComponent<RadialMenuButton>().CurrentState != ButtonState.Disabled &&
                currentSelectedButtonType != ButtonType.LeftOuter)
            {
                DeselectButton();
                SelectButton(leftOuterButton);
                currentSelectedButtonType = ButtonType.LeftOuter;
            }
        }
        else if (-125 <= angle && angle <= -45 && downButton.GetComponent<RadialMenuButton>().CurrentState != ButtonState.Disabled)
        {
            if (currentSelectedButtonType != ButtonType.Down)
            {
                DeselectButton();
                SelectButton(downButton);
                currentSelectedButtonType = ButtonType.Down;
            }
        }
        else
        {
            DeselectButton();
            currentSelectedButtonType = ButtonType.None;
        }
    }

    IEnumerator UpgradeCurrentSelectedButton() // you prob need to DeselectAllButton here; come back after Deselect is handled
    {
        isUpgradeButtonCoroutineRunning = true;
        yield return new WaitForSeconds(1f);
        switch (currentSelectedButtonType)
        {
            case ButtonType.Left:
                DeselectButton();
                SelectButton(leftOuterButton);
                currentSelectedButtonType = ButtonType.LeftOuter;
                break;
            case ButtonType.Right:
                DeselectButton();
                SelectButton(rightOuterButton);
                currentSelectedButtonType = ButtonType.RightOuter;
                break;
        }
        isUpgradeButtonCoroutineRunning = false;
    }

    void SelectButton(GameObject button)
    {
        button.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Selected;
        button.GetComponent<RadialMenuButton>().SwitchToSelectedSprite();
    }

    void PlayTransition()
    {
        // Debug.Log("currentSelectedButtonType at PlayTransition():  " + currentSelectedButtonType);
        switch (previousSelectedButtonType)
        {
            case ButtonType.Right:
                // Debug.Log("Right should be called");
                guidedTourManager.VisitNextScene();
                break;
            case ButtonType.Top:
                guidedTourManager.ZoomInToCurrentScene();
                break;
            case ButtonType.Left:
                guidedTourManager.VisitPreviousScene();
                break;
            case ButtonType.Down:
                guidedTourManager.ZoomOutFromCurrentScene();
                break;
            case ButtonType.RightOuter: /// increment or decrement, then set animationclipname based on that number, to skip. And maybe length too, for consistency??
                if (!guidedTourManager.GetIsDuringTransition() && guidedTourManager.CurrentSceneNumber < guidedTourManager.sceneDataArray.Length)
                {
                    guidedTourManager.CurrentSceneNumber += 1;
                    guidedTourManager.CurrentAnimationClipName = guidedTourManager.sceneDataArray[guidedTourManager.CurrentSceneNumber - 1].forwardAnimationClipName;
                    // guidedTourManager.SetCurrentAnimationClipLength(guidedTourManager.sceneDataArray[guidedTourManager.GetCurrentSceneNumber() - 1].forwardAnimationClipLength);
                }
                guidedTourManager.SkipTransition();
                break;
            case ButtonType.LeftOuter:
                if (!guidedTourManager.GetIsDuringTransition() && guidedTourManager.CurrentSceneNumber > 1)
                {
                    guidedTourManager.CurrentSceneNumber -= 1;
                    guidedTourManager.CurrentAnimationClipName = guidedTourManager.sceneDataArray[guidedTourManager.CurrentSceneNumber - 1].backwardAnimationClipName;
                    // guidedTourManager.SetCurrentAnimationClipLength(guidedTourManager.sceneDataArray[guidedTourManager.GetCurrentSceneNumber() - 1].backwardAnimationClipLength);
                }
                guidedTourManager.SkipTransition();
                break;
        }
    }

    void DeselectButton()
    {
        GameObject[] buttons = { leftButton, rightButton, topButton, downButton, leftOuterButton, rightOuterButton };
        foreach(GameObject button in buttons)
        {
            if (button.GetComponent<RadialMenuButton>().CurrentState == ButtonState.Selected)
            {
                button.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Default;
                button.GetComponent<RadialMenuButton>().SwitchToDefaultSprite();
                break;
            }
          
        }
    }

    void OnDefaultState() // should probably set currentSelectedButtonType here ... 
    {
        if (guidedTourManager.CurrentSceneNumber != 1)
        {
            leftButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Default;
            leftButton.GetComponent<RadialMenuButton>().SwitchToDefaultSprite();

            leftOuterButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Default;
            leftOuterButton.GetComponent<RadialMenuButton>().SwitchToDefaultSprite();
        } else
        {
            leftButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
            leftButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();

            leftOuterButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
            leftOuterButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();
        }

        if (guidedTourManager.CurrentSceneNumber != guidedTourManager.sceneDataArray.Length)
        {
            rightButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Default;
            rightButton.GetComponent<RadialMenuButton>().SwitchToDefaultSprite();

            rightOuterButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Default;
            rightOuterButton.GetComponent<RadialMenuButton>().SwitchToDefaultSprite();
        } else
        {
            rightButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
            rightButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();

            rightOuterButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
            rightOuterButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();
        }     

        topButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
        topButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();

        if (!string.IsNullOrEmpty(guidedTourManager.sceneDataArray[guidedTourManager.CurrentSceneNumber - 1].ZoomOutAnimationClipName))
        {
            downButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Default;
            downButton.GetComponent<RadialMenuButton>().SwitchToDefaultSprite();
        } else
        {
            downButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
            downButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();
        }
        
        /// if this isn't here, then currentButtonType will still be left/right outer, and the SelectButtonBasedOnThumbstickCoordinates will have no effect. This means that with no buttons selected due
        /// to above and the thumbstick released, we will still be able to skip
        currentSelectedButtonType = ButtonType.None; 

        //if (currentSelectedButtonType == ButtonType.LeftOuter || currentSelectedButtonType == ButtonType.RightOuter)
        //{
        //    // Debug.Log("Does this statement even get called");
        //    isThumbstickHeldAfterTransition = true;
        //    currentSelectedButtonType = ButtonType.None;
        //}
    }

    void OnDuringTransition()
    {
        leftButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
        leftButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();

        rightButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
        rightButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();

        topButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
        topButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();

        downButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
        downButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();

        if (guidedTourManager.GetCurrentTransitionType() == TransitionType.Backward)
        {
            leftOuterButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Default;
            leftOuterButton.GetComponent<RadialMenuButton>().SwitchToDefaultSprite();
        } else
        {
            leftOuterButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
            leftOuterButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();
        }

        if (guidedTourManager.GetCurrentTransitionType() == TransitionType.Forward)
        {
            rightOuterButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Default;
            rightOuterButton.GetComponent<RadialMenuButton>().SwitchToDefaultSprite();
        }
        else
        {
            rightOuterButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
            rightOuterButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();
        }
    }

    void OnZoomedOut()
    {
        leftButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
        leftButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();

        rightButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
        rightButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();

        topButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Default;
        topButton.GetComponent<RadialMenuButton>().SwitchToDefaultSprite();

        downButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
        downButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();

        leftOuterButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
        leftOuterButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();

        rightOuterButton.GetComponent<RadialMenuButton>().CurrentState = ButtonState.Disabled;
        rightOuterButton.GetComponent<RadialMenuButton>().SwitchToDisabledSprite();

        // currentSelectedButtonType = ButtonType.None;
    }
}
