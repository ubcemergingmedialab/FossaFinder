using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class SceneTransitionButton : MonoBehaviour
{

    public enum ButtonState
    {
        Active, Disabled, Default
    }
    public VRTK_InteractableObject linkedObject;
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
        linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);

        if (linkedObject != null)
        {
            linkedObject.InteractableObjectUsed += InteractableObjectUsed;

            linkedObject.InteractableObjectTouched += InteractableObjectTouched;

            linkedObject.InteractableObjectUntouched += InteractableObjectUntouched;
        }
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

    public ButtonState GetButtonState()
    {
        return state;
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
        state = ButtonState.Disabled;
    }

    protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("used");
        if (state == ButtonState.Disabled)
        {
            return;
        }
        manager.ButtonClicked(this.gameObject);
    }

    void OnMouseDown()
    {
        if (state == ButtonState.Disabled)
        {
            return;
        }
        manager.ButtonClicked(this.gameObject);
    }

    protected virtual void InteractableObjectTouched(object sender, InteractableObjectEventArgs e)
    {
        mouseOver = true;
    }

    protected virtual void InteractableObjectUntouched(object sender, InteractableObjectEventArgs e)
    {
        mouseOver = false;
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
