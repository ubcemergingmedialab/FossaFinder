namespace VRTK
{
    using UnityEngine;

    public class Trigger_labels : MonoBehaviour
    {
        public VRTK_InteractableObject linkedObject;
        public GameObject Label;
        

        
        protected bool showlabel;

        protected virtual void OnEnable()
        {
            showlabel = false;
            linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);

            if (linkedObject != null)
            {
                linkedObject.InteractableObjectUsed += InteractableObjectUsed;
                linkedObject.InteractableObjectUnused += InteractableObjectUnused;
            }

          
        }

        protected virtual void OnDisable()
        {
            if (linkedObject != null)
            {
                linkedObject.InteractableObjectUsed -= InteractableObjectUsed;
                linkedObject.InteractableObjectUnused -= InteractableObjectUnused;
            }
        }

        protected virtual void Update()
        {
            if (showlabel)
            {
                Label.SetActive(true);
            }
            else { Label.SetActive(false); }
        }

        protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
        {
            showlabel = true;
            //Label.SetActive(true);
        }

        protected virtual void InteractableObjectUnused(object sender, InteractableObjectEventArgs e)
        {
            showlabel = false;
        }
    }
}