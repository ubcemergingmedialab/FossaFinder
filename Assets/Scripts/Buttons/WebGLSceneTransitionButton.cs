using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class WebGLSceneTransitionButton : MonoBehaviour
{

    public enum ButtonState
    {
        Active, Disabled, Default
    }
    public SceneTransitionUI manager;
    public int targetScene;
    public int phaseNumber;
    private ButtonState state;
    private bool mouseOver;

    // Use this for initialization
    void Start()
    {
        state = ButtonState.Default;
    }

    public void ButtonClick()
    {
        // Don't allow user to click if button is diabled
        if (state == ButtonState.Disabled)
        {
            return;
        }
        manager.ButtonClicked(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDefaultState()
    {
        state = ButtonState.Default;
    }

    public void SetActiveState()
    {
        state = ButtonState.Active;
    }

    public void SetDisabledState()
    {
        state = ButtonState.Disabled;
    }
}
