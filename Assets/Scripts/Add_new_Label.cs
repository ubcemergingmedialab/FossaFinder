using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Add_new_Label : MonoBehaviour {
    public LabelScript mytarget;
    //SerializedProperty list;
    //SerializedObject GetTarget;
    bool showWindow = false;
    public Camera cam;
    public GameObject helpbox;

    public void Start() {
        helpbox.SetActive(false);
    }
    public void AddLabel(string Labelname) {
        helpbox.SetActive(true);
       //if(Event.current.type == EventType.MouseDown)
        
            //Vector2 guiPosition = Event.current.mousePosition;
            Ray ray = cam.ScreenPointToRay(new Vector2(1,2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                mytarget.AddLabel(hit.point, hit.normal);
            }

            //startTrackingMouse = false;
        

    }
}
