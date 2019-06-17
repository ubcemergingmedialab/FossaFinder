using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MenuManager : MonoBehaviour {

    public static MenuManager instance = null;
    public GameObject menuPrefab; //Prefab to create if menu is not found in the scene
    private GameObject menu;        //menu parent object
    private GameObject menuPlane;   //actual menu to activate on and off
    private GameObject headset;
    private VRTK_TransformFollow menuFollower;  //Objectfollow component on the menu
    private VRTK_TransformFollow menuFollower2;  //Objectfollow component on the menu

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        //Setup menu, initialise if not in scene
        menu = GameObject.FindGameObjectWithTag("Menu");
        if(menu == null)
        {
            menu = GameObject.Instantiate(menuPrefab);
        }

        //Setup menu plane
        menuPlane = GameObject.FindGameObjectWithTag("MenuPlane");

        //Set up menu follower 1
        menuFollower = menu.GetComponent<VRTK_TransformFollow>();
        if(menuFollower == null)
        {
            menuFollower = menu.AddComponent<VRTK_TransformFollow>();
        }
        menuFollower.followsPosition = true;
        menuFollower.followsRotation = false;
        menuFollower.followsScale = false;
        menuFollower.smoothsPosition = false;
        menuFollower.smoothsRotation = false;
        menuFollower.moment = VRTK_TransformFollow.FollowMoment.OnPreRender; //earliest possible (?) TODO: ensure prerender is before precull
        menuFollower.gameObjectToChange = menu;

        //Set up headset follower
        headset = GameObject.FindGameObjectWithTag("HeadsetFollower");

        //Set up menu follower 2
        menuFollower2 = menu.AddComponent<VRTK_TransformFollow>();

        menuFollower2.followsPosition = false;
        menuFollower2.followsRotation = true;
        menuFollower2.followsScale = false;
        menuFollower2.smoothsPosition = false;
        menuFollower2.smoothsRotation = false;
        menuFollower2.moment = VRTK_TransformFollow.FollowMoment.OnPreRender; //earliest possible (?) TODO: ensure prerender is before precull
        menuFollower2.gameObjectToChange = menu;
        menuFollower2.gameObjectToFollow = headset;

        //Initially turn off menu follower and menu plane
        menuFollower.enabled = false;
        menuPlane.SetActive(false); 
    }

    //Called when menu button is pressed down. Passes the controller that was pressed as a parameter
    public void MenuButtonPressed(GameObject activeController)
    {
        menuPlane.SetActive(true);
        menuFollower.enabled = true;
        menuFollower2.enabled = true;
        menuFollower.gameObjectToFollow = activeController; //Menu tracks the controller that called the method
        StartCoroutine("MenuTracking"); //Turn off menu tracking after one tick
    }

    private IEnumerator MenuTracking()
    {
        yield return new WaitForFixedUpdate();
        menuFollower.enabled = false;
        menuFollower2.enabled = false;
    }

    //Called when menu button is unpressed
    public void MenuButtonUnpressed()
    {
        menuPlane.SetActive(false);
    }

}
