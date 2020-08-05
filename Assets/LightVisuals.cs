using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightVisuals : MonoBehaviour {
    public List<GameObject> availableLights;

    // Use this for initialization

    /*! Setup onEnable
* 
* Add Enablelights() to eventsystem when enabled
* @see GuidedTourManager()
*/

    public void OnEnable()
    {
        GuidedTourManager.Setlights += Enablelights;
        GuidedTourManager.DisableLights += DisableAllLights;
    }

    /*! Setup OnDisable
    * 
    * Remove Enablelights() to eventsystem when enabled
    * @see GuidedTourManager()
    */
    public void OnDisable()
    {
        GuidedTourManager.Setlights -= Enablelights;
        GuidedTourManager.DisableLights -= DisableAllLights;
    }
    /*! Initialize list
* 
* Add all lights in the hierarchy to the dictionary
*/
    public void Start()
    {
        //availablelights = new Dictionary<string, GameObject>();
        //foreach (Transform child in transform)
        //{
        //    availablelights.Add(child.name, child.gameObject);
            

        //}

    }

    /*! \Manage lightss on the scene
        * only activate specific lights by the number of scene
        * @param names Names of fissures that should be shown on the scene
     */

    public void Enablelights(string[] names)
    {
        //foreach (KeyValuePair<string, GameObject> pair in availablelights)
        //{
        //    pair.Value.SetActive(false);
        //}
        //foreach (string name in names)
        //{
        //    availablelights[name].SetActive(true);
        //}

        foreach (GameObject availableLight in availableLights)
        {
            availableLight.SetActive(false);
        }

        foreach (string name in names)
        {
            foreach (GameObject availableLight in availableLights)
            {
                if (availableLight.name == name)
                {
                    availableLight.SetActive(true);
                    Debug.Log("LIGHT: enabling " + availableLight.name);
                }
            }
        }
    }

    public void DisableAllLights()
    {
        //foreach (KeyValuePair<string, GameObject> pair in availablelights)
        //{
        //    pair.Value.SetActive(false);
        //}

        foreach (GameObject availableLight in availableLights)
        {
            availableLight.SetActive(false);
        }
    }
}
