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

        Debug.Log(mouseOver);
        if (mouseOver)
        {
            GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
        else
        {
            if (state == ButtonState.Active)
            {
                SetActiveState();
            } else if (state == ButtonState.Default)
            {
                SetDefaultState();
            } else
            {
                SetDisabledState();
            }
          
        }

        Debug.Log(GetComponent<MeshRenderer>().material.color.a);
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

    void OnMouseEnter()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }
}
