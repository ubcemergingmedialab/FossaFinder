using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneTransitionButton : MonoBehaviour
{

    public enum ButtonState
    {
        Active, Disabled, Default
    }

    public Material defaultColor, activeColor, disabledColor, hoverColor;
    public int targetScene;
    private SceneTransitionUI manager;
    private ButtonState state;
    private bool mouseOver;

    private Material buttonMaterial;

    // Use this for initialization
    void Start()
    {
        manager = GetComponentInParent<SceneTransitionUI>();
        state = ButtonState.Default;
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseOver)
        {
            if (state != ButtonState.Disabled)
            {
                GetComponent<MeshRenderer>().material = hoverColor;
            }

        }
        else
        {
            GetComponent<MeshRenderer>().material = buttonMaterial;
        }
    }

    public void SetDefaultState()
    {
        buttonMaterial = defaultColor;
        state = ButtonState.Default;
    }

    public void SetActiveState()
    {
        buttonMaterial = activeColor;
        state = ButtonState.Active;
    }

    public void SetDisabledState()
    {
        //GetComponent<MeshRenderer>().material = disabledColor;
        state = ButtonState.Disabled;
    }


    void OnMouseDown()
    {
        // Don't allow user to click if button is diabled
        if (state == ButtonState.Disabled)
        {
            return;
        }
        manager.ButtonClicked(this.gameObject);
    }

    void OnMouseEnter()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }
}
