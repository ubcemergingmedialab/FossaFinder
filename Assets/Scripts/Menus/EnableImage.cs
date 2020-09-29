using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableImage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleImage()
    {
        LevelLoaderUtility.Async.allowSceneActivation = true;
    }
}
