using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneTransitionButton : MonoBehaviour
{

    public enum ButtonState
    {
        Active, Disabled, Default
    }

    public Material defaultColor, activeColor, disabledColor;
    public int targetScene;
    private SceneTransitionUI manager;
    private ButtonState state;
    private bool mouseOver;

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
            Color hoverColor = GetComponent<MeshRenderer>().material.color;
            hoverColor.a = 0.25f;
            GetComponent<MeshRenderer>().material.color = hoverColor;
        }
        else
        {
            Color originalColor = GetComponent<MeshRenderer>().material.color;
            originalColor.a = 0.5f;
            GetComponent<MeshRenderer>().material.color = originalColor;
        }
    }

    public void SetDefaultState()
    {
        GetComponent<MeshRenderer>().material = defaultColor;
        state = ButtonState.Default;
    }

    public void SetActiveState()
    {
        GetComponent<MeshRenderer>().material = activeColor;
        state = ButtonState.Active;
    }

    public void SetDisabledState()
    {
        GetComponent<MeshRenderer>().material = disabledColor;
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

    void OnMouseOver()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }
}
