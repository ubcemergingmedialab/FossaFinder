using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RefreshButton : MonoBehaviour {

    public GameObject[] phaseButtons;
    public VRTK_InteractableObject linkedObject;
    private SceneTransitionUI sceneTransitionUI;

    // Use this for initialization
    void Start () {
        sceneTransitionUI = GetComponentInParent<SceneTransitionUI>();

        linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);
   
        if (linkedObject != null)
        {
            linkedObject.InteractableObjectUsed += InteractableObjectUsed;
            linkedObject.InteractableObjectTouched += InteractableObjectTouched;
            linkedObject.InteractableObjectUntouched += InteractableObjectUntouched;
        }
    }

    void Update()
    {
    }

    void OnMouseDown()
    {
        sceneTransitionUI.Refresh(this.gameObject);
    }

    protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        sceneTransitionUI.Refresh(this.gameObject);
    }

    protected virtual void InteractableObjectTouched(object sender, InteractableObjectEventArgs e)
    {
    }

    protected virtual void InteractableObjectUntouched(object sender, InteractableObjectEventArgs e)
    {
    }
}
