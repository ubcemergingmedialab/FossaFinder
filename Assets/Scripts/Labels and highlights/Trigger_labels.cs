namespace VRTK
{
    using UnityEngine;

    public class Trigger_labels : MonoBehaviour
    {
        public VRTK_InteractableObject linkedObject;
        // public GameObject Label;

        public GameObject[] labels;

        protected bool showlabel;


        private LabelRecording recorder;

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


        protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
        {

            // Label.SetActive(true);

            foreach(GameObject label in labels)
            {
                label.SetActive(true);

                if (recorder != null)
                {
                    recorder.QueueMessage(label.name + ";true");
                }
            }
            //print("triggered");
            showlabel = true;
        }

        protected virtual void InteractableObjectUnused(object sender, InteractableObjectEventArgs e)
        { 
            // Label.SetActive(false);

            foreach(GameObject label in labels)
            {
                label.SetActive(false);

                if (recorder != null)
                {
                    recorder.QueueMessage(label.name + ";false");
                }
            }
        }

        public void OnMouseDown()
        {
            if (isActive)
            {
                foreach (GameObject label in labels)
                {
                    label.SetActive(false);

                    if (recorder != null)
                    {
                        recorder.QueueMessage(label.name + ";false");
                    }
                }
            }
            else
            {
                foreach (GameObject label in labels)
                {
                    label.SetActive(true);

                    if (recorder != null)
                    {
                        recorder.QueueMessage(label.name + ";true");
                    }
                }
            }
            isActive = !isActive;
        }

        public void InjectRecorder(LabelRecording recorder)
        {
            Debug.Log("inserted recorder");
            this.recorder = recorder;
        }
    }
}