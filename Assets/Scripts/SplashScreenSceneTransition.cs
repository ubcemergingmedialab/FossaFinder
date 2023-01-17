using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenSceneTransition : MonoBehaviour
{
    public string sceneToTransition;
    public Image emlLogo;

    private Color transparentLogo;
    private Color opaqueLogo;
    private float currentTime = 0f;

    //settings
    private float timeToTransition = 1.5f;
    private float timeToPause = 1.5f;

    // Use this for initialization
    void Start()
    {
        AssignColors();
        StartCoroutine(SplashAnimation());
    }

    public void AssignColors()
    {
        opaqueLogo = emlLogo.color;

        //make sure it's transparent to start off
        Color transColor = emlLogo.color;
        transColor.a = 0;
        transparentLogo = transColor;
        emlLogo.color = transparentLogo;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SplashAnimation()
    {
        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToTransition);

        //Disable automatic scene transition
        asyncOperation.allowSceneActivation = false;

        //transition fade in
        while (currentTime < timeToTransition)
        {
            currentTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
            emlLogo.color = Color.Lerp(transparentLogo, opaqueLogo, currentTime / timeToTransition);
        }

        //show for 'x' seconds
        yield return new WaitForSeconds(timeToPause);

        //transition fade out
        currentTime = 0;
        while (currentTime < timeToTransition)
        {
            currentTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
            emlLogo.color = Color.Lerp(opaqueLogo, transparentLogo, currentTime / timeToTransition);
        }

        //scene transition to main scene
        asyncOperation.allowSceneActivation = true;
    }
}
