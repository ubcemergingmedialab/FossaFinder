using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightVisuals : MonoBehaviour {
	private Dictionary<string, GameObject> availablelights;
    // Use this for initialization

    /*! Setup onEnable
* 
* Add Enablelights() to eventsystem when enabled
* @see GuidedTourManager()
*/

    public void OnEnable()
    {
        GuidedTourManager.Setlights += Enablelights;

    }
    /*! Setup OnDisable
    * 
    * Remove Enablelights() to eventsystem when enabled
    * @see GuidedTourManager()
    */
    public void OnDisable()
    {
        GuidedTourManager.Setlights -= Enablelights;

    }
    /*! Initialize list
* 
* Add all lights in the hierarchy to the dictionary
*/
    public void Start()
    {
        availablelights = new Dictionary<string, GameObject>();
        foreach (Transform child in transform)
        {
            availablelights.Add(child.name, child.gameObject);
            

        }

    }

    /*! \Manage lightss on the scene
        * only activate specific lights by the number of scene
        * @param names Names of fissures that should be shown on the scene
     */

    public void Enablelights(string[] names)
    {
        foreach (KeyValuePair<string, GameObject> pair in availablelights)
        {
            pair.Value.SetActive(false);
        }
        foreach (string name in names)
        {
            availablelights[name].SetActive(true);
        }
    }

}
