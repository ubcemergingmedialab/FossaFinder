using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*! \brief Manage all the labels over scences
* 
* This class will disable all labels on the scene when the scene transformed
*/

public class LabelManager : MonoBehaviour {

    
    private Dictionary<string, GameObject> availableLabels;

    /*! Setup onEnable
    * 
    * Add DisbleLabels() to eventsystem when enabled
    * @see GuidedTourManager()
    */

    public void OnEnable()
    {
        GuidedTourManager.DuringSceneTransition += DisableLabels;

    }
    /*! Setup OnDisable
    * 
    * Remove DisbleLabels() to eventsystem when enabled
    * @see GuidedTourManager()
    */
    void OnDisable()
    {
        GuidedTourManager.DuringSceneTransition -= DisableLabels;
    }

     /*! Initialize list
        * 
        * Add all labels in the hierarchy to the dictionary
     */
    public void Start()
    {
        availableLabels = new Dictionary<string, GameObject>();
        foreach (Transform child in transform)
        {
            availableLabels.Add(child.name, child.gameObject);
            print(child.name);
        }
        int z = availableLabels.Count;
    }

     /*! \Disable labels on the scene
        * 
        * 
     */
    public void DisableLabels()
    {
        int i = 0;
        foreach (KeyValuePair<string, GameObject> pair in availableLabels)
        {
            
            pair.Value.SetActive(false);
            print(pair.Key);
            print(i);
            i++;
        }
    
    }
}
