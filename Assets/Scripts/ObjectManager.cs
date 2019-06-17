using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectManager : MonoBehaviour {

    public static ObjectManager instance = null;

    GameObject head;            //Parent head GO, necessary for scaling
    GameObject[][] goLists;    //Array of game object arrays, each index corresponds to the array of game objects making up the parts enum index
    bool[] isActive;            //Array of booleans, each index corresponds to whether the corresponding part is active
    ArrayList[] materialLists; //Array of arraylists of materials, each index corresponds to the materials in a corresponding part. 
    GameObject[][] labelLists; //Array of game object label arrays, each index corresponds to the array of game objects making up the Labels enum index
    bool[] isLabelActive; //Array of booleans, each index corresponds to whether the corresponding label is active
    public bool labelOn = false;


    void Awake() //Creates singleton
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject); Doesn't work with parent manager object - must be root TODO: consider making a 'manager manager (metamanager?)' class that acts as the only singleton. Would certainly help with reducing duplicate code
    }
    void Start()
    {
        //Create array to manage Head Parts
        goLists = new GameObject[5][];
        isActive = new bool[5];
        materialLists = new ArrayList[5];
        //Loads full head GOs
        goLists[0] = GameObject.FindGameObjectsWithTag("skull_bone");
        goLists[1] = GameObject.FindGameObjectsWithTag("artery");
        goLists[2] = GameObject.FindGameObjectsWithTag("PPF_bone");
        goLists[3] = GameObject.FindGameObjectsWithTag("nerve");
        goLists[4] = GameObject.FindGameObjectsWithTag("cavity");

        int count = 0;
        foreach(GameObject[] objArray in goLists){
            // ASSSUME: parent is at the top of the list - element 0
            isActive[count] = objArray[0].activeInHierarchy;
            count++;
        }

        head = GameObject.FindGameObjectWithTag("Head");

        //Load material Lists with all materials in head
        for (int i = 0; i < 5; i++)
        {
            if (goLists[i].Length > 0)
            {
                materialLists[i] = new ArrayList();
                foreach (GameObject GO in goLists[i])
                {
                    foreach (Renderer r in GO.GetComponentsInChildren<Renderer>())
                    {
                        materialLists[i].Add(r.material);
                    }
                }
            }
        }

        labelLists = new GameObject[3][];
        isLabelActive = new bool[3];

        labelLists[0] = GameObject.FindGameObjectsWithTag("BonesLabel");
        labelLists[1] = GameObject.FindGameObjectsWithTag("NervesLabel");
        labelLists[2] = GameObject.FindGameObjectsWithTag("ArteriesLabel");

        count = 0;
        foreach (GameObject[] goArray in labelLists)
        {
            foreach (GameObject go in goArray)
            {
                go.SetActive(labelOn);
            }
            // sets the labels depending on whether the labelOn is off or on
            isLabelActive[count] = labelOn;
        }
    }

    //Traverses the corresponding part's material list and adjusts the alpha to the given value
    public void ChangeAlphaOnTag(Parts part, float alpha)
    {
        foreach (Material mat in materialLists[(int)part])
        {
            if (gameObject.CompareTag("PPF_bone"))
            {
                
                mat.SetFloat("_Mode", alpha);   //change between two rendering mode - Opaque (1) and Transparent (4)
            }
            else
            {
                mat.SetFloat("_Opacity", alpha);    //changing opacity when using force field shader
            }
        }
    }


    //TODO: COMBINE TOGGLE ON/OFF WITH LABEL ON/OFF; currently the label toggling ability is enabled/disabled by toggle object, not the label itself

    //Sets all GO's in the given part to be active. I know its a for loop, but based on our prefab structure each GO list should just have one parent game object in it. 
    public void ToggleObjectOnTag(Parts part)
    {
        foreach(GameObject GO in goLists[(int)part])
        {
            GO.SetActive(!isActive[(int)part]);
        }
        isActive[(int)part] = !isActive[(int)part];
    }

    //Returns whether the given part is on or off
    public bool IsObjectOn(Parts part)
    {        
        return isActive[(int)part];
    }

    public void ToggleLabelOnTag(Labels label, Parts part)
    {
        foreach (GameObject GO in labelLists[(int)label])
        {
            GO.SetActive(!isLabelActive[(int)label]);
        }
        isLabelActive[(int)label] = !isLabelActive[(int)label];
    }

    //Returns whether the given part is labelled
    public bool IsLabelOn(Labels label)
    {
        return isLabelActive[(int)label];
    }


    //Scales the head GO
    public void ChangeScale(int scale) //Scales all objects down to scale
    {
        Transform headTransform = head.transform;
        Vector3 initScale = headTransform.localScale;
        initScale.Set(scale, scale, scale);
        head.transform.localScale = initScale;
    }

    //Turns labels on the given part on or off

    public void Reset(int scale, Parts part, float alpha, Labels label)
    {
        ChangeScale(scale);
        ChangeAlphaOnTag(part, alpha);
        if (!IsObjectOn(part))
        {
            ToggleObjectOnTag(part);
        }
        if (IsLabelOn(label))
        {
            ToggleLabelOnTag(label, part);
        }
    }
}
