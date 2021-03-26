using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RefreshButton : MonoBehaviour {

    public GameObject[] phaseButtons;
    public int[] originalSceneNumbers;
    public VRTK_InteractableObject linkedObject;
    private SceneTransitionUI sceneTransitionUI;

    // Use this for initialization
    void Start () {
        Debug.Log("Initializing refresh button");
        sceneTransitionUI = GetComponentInParent<SceneTransitionUI>();

        for (var i = 0; i < phaseButtons.Length; i++)
        {
            originalSceneNumbers[i] = phaseButtons[i].GetComponent<SceneTransitionButton>().targetScene;
        }
        linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);

        if (linkedObject != null)
        {
            linkedObject.InteractableObjectUsed += InteractableObjectUsed;
            linkedObject.InteractableObjectTouched += InteractableObjectTouched;
            linkedObject.InteractableObjectUntouched += InteractableObjectUntouched;
        }
    }

    void OnMouseDown()
    {
        sceneTransitionUI.Refresh(this.gameObject);
    }

    protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("here");
        sceneTransitionUI.Refresh(this.gameObject);
     //   if (state == ButtonState.Disabled)
      //  {
     //       return;
     //   }
     //   manager.ButtonClicked(this.gameObject);
    }

    protected virtual void InteractableObjectTouched(object sender, InteractableObjectEventArgs e)
    {
        //mouseOver = true;
    }

    protected virtual void InteractableObjectUntouched(object sender, InteractableObjectEventArgs e)
    {
      //  mouseOver = false;
    }

    // Update is called once per frame
    void Update () {
	}
}
