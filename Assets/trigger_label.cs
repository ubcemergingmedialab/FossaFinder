using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using VRTK;

public class trigger_label : MonoBehaviour {
	public VRTK_InteractableObject linkedObject;
	public GameObject label;

	protected bool needbetriggered ;

	protected virtual void OnEnable()
	{
		needbetriggered = false;
		linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);

		if (linkedObject != null)
		{
			linkedObject.InteractableObjectUsed += InteractableObjectUsed;
		
		}

		
	}

	// Use this for initialization
	protected virtual void OnDisable()
	{
		if (linkedObject != null)
		{
			linkedObject.InteractableObjectUsed -= InteractableObjectUsed;
			//linkedObject.InteractableObjectUnused -= InteractableObjectUnused;
		}
	}

	// Update is called once per frame


	protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
	{
		if (needbetriggered)
		{
			label.SetActive(true);
			needbetriggered = false;
		}
		else {
			label.SetActive(false);
		}
	}


}

