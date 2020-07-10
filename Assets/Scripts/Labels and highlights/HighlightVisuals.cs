using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*! \brief Manage all the highlights over scences
* 
* This class will manage all highlights on the scene by the number of scene
*/

public class HighlightVisuals : MonoBehaviour {
    
    private Dictionary<string, GameObject> availableHighlights; 
    /// Material of inactive highlights
    public Material defaultMaterial; 
    /// Material of active highlights
    public Material highlightMaterial; 

    /*! Setup onEnable
    * 
    * Add EnableHighlights() to eventsystem when enabled
    * @see GuidedTourManager()
    */

    public void OnEnable()
    {
        GuidedTourManager.SetHighlights += EnableHighlights;

    }
    /*! Setup OnDisable
    * 
    * Remove EnableHighlights() to eventsystem when enabled
    * @see GuidedTourManager()
    */
    public void OnDisable()
    {
        GuidedTourManager.SetHighlights -= EnableHighlights;

    }

         /*! Initialize list
        * 
        * Add all highlights in the hierarchy to the dictionary
     */
    public void Start()
    {
        availableHighlights = new Dictionary<string, GameObject>();
        foreach(Transform child in transform)
        {
            availableHighlights.Add(child.name, child.gameObject);
        }
    }

    /*! \Manage labels on the scene
        * only activate specific highlights by the number of scene
        * @param names Names of fissures that should be shown on the scene
     */

    public void EnableHighlights(string[] names)
    {
        foreach(KeyValuePair<string, GameObject> pair in availableHighlights)
        {
            pair.Value.GetComponent<Renderer>().material = defaultMaterial;
        }
        foreach(string name in names)
        {
            availableHighlights[name].GetComponent<Renderer>().material = highlightMaterial;
        }
    }

}
