using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using System;

public class ChangeEnviron : MonoBehaviour {

    private GameObject[] environs;
    private int index = 0;
    private int max_index;
    private VRTK_ControllerReference controllerReferenceRight;
    private VRTK_ControllerReference controllerReferenceLeft;
    private bool canPress;

    // Use this for initialization
    void Start () {
        environs = GameObject.FindGameObjectsWithTag("Environs");
        max_index = environs.Length;
        environs[index].SetActive(true);
        for(int i = index+1; i < max_index; i++)
        {
            environs[i].SetActive(false);
        }
    }

    private void Update()
    {
        ResetPress();
        GameObject rightHand = VRTK_DeviceFinder.GetControllerRightHand(true);
        controllerReferenceRight = VRTK_ControllerReference.GetControllerReference(rightHand);
        GameObject leftHand = VRTK_DeviceFinder.GetControllerLeftHand(true);
        controllerReferenceLeft = VRTK_ControllerReference.GetControllerReference(leftHand);

        int nextSceneIndex = index;

        if (RightGripPressed() || Input.GetKeyUp(KeyCode.Space))
        {
            nextSceneIndex++;
            if (nextSceneIndex >= max_index)
                nextSceneIndex = 0;
        }
        else if (LeftGripPressed() || Input.GetKeyUp(KeyCode.Backspace))
        {
            nextSceneIndex--;
            if (nextSceneIndex < 0)
                nextSceneIndex = max_index-1;
        }

        if (nextSceneIndex == index)
        {
            return;
        }

        environs[index].SetActive(false);
        environs[nextSceneIndex].SetActive(true);
        index = nextSceneIndex;
    }

    private bool RightGripPressed()
    {
        if (!VRTK_ControllerReference.IsValid(controllerReferenceRight))
        {
            return false;
        }
        if (canPress &&
            VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, controllerReferenceRight))
        {
            canPress = false;
            return true;
        }
        return false;
    }

    private bool LeftGripPressed()
    {
        if (!VRTK_ControllerReference.IsValid(controllerReferenceLeft))
        {
            return false;
        }

        if (canPress &&
            VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, controllerReferenceLeft))
        {
            canPress = false;
            return true;
        }
        return false;
    }

    private void ResetPress()
    {
        if (!canPress &&
            !VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, controllerReferenceRight) &&
            !VRTK_SDK_Bridge.GetControllerButtonState(SDK_BaseController.ButtonTypes.Grip, SDK_BaseController.ButtonPressTypes.Press, controllerReferenceLeft))
        {
            canPress = true;
        }
    }
}
