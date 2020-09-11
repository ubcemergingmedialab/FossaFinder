using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//using VRTK;

public class visibility : MonoBehaviour {
   public static int label_num = 11;
   public GameObject[] labels = new GameObject[label_num];
   public GameObject headset;
   public int Visible_num;
   protected Transform headset_transform;
   protected bool LabelGlance = true;

	// Use this for initialization
	void Start () {
		    //headset = VRTK_DeviceFinder.HeadsetCamera();
     
	}
	
	// Update is called once per frame
	void Update () {
     enableLabels();
     var distances = new float[label_num];
     var temp = new float[label_num];
     int num = 0;
     headset_transform = headset.transform;
     float distance = 0;
     while (num <= label_num-1){
          distance = getDistance(labels[num].transform);
          distances[num] = distance;
          temp[num] = distance;
          num ++;
     }

     Array.Sort(temp,0,label_num);
     Array.Reverse(temp,0,label_num);
     int[] indexList = invisibleIndex(temp,distances);
     disableLabels(indexList);
     


		
	}

    private void disableLabels(int[] indexes){
        for(int i=0; i<= label_num-Visible_num-1; i++){
        labels[indexes[i]].SetActive(false);
        }
    
    
    }
    private void enableLabels(){
        for(int i=0; i<= label_num-1; i++){
        labels[i].SetActive(true);
        }
    
    
    }
   private int[] invisibleIndex(float[] sorted,float[] origin){
        int invisible_num = label_num-Visible_num;
        int[] indexList = new int[invisible_num];
        int num = 0;
       //print(sorted[0]);
       // print(origin[0]);
       
        while (num <= invisible_num - 1){
            
            int index = Array.FindIndex(origin,x => x == sorted[num]);
            indexList[num] = index;
            num ++;
        }

        return indexList;
   
   }

    private float getDistance(Transform lab)
    {
               float distanceFromHeadsetToLabel = Vector3.Distance(headset_transform.position, lab.position);
               Vector3 lookPoint = headset_transform.position + (headset_transform.forward * distanceFromHeadsetToLabel);
               float distance = Vector3.Distance(lab.position, lookPoint);
               return distance;
               //Vector3 lookPoint = headset.position + (headset.forward * distanceFromHeadsetToController);
               //return Vector3.Distance(lab.position, lookPoint);


    }

	protected void CheckHeadsetView(Transform lab)
        {
            if (lab!= null)
            {
                float distanceFromHeadsetToController = Vector3.Distance(headset_transform.position, lab.position);
                Vector3 lookPoint = headset_transform.position + (headset_transform.forward * distanceFromHeadsetToController);

                if (Vector3.Distance(lab.position, lookPoint) >= 0.15f)
                {
                    LabelGlance = false;
                }

           

            }
        }
}
