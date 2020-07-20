using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Created by Cindy Shi.
/// Manage audio for chapter three.
/// </summary>
public class AudioChap3Manager : MonoBehaviour
{
    [SerializeField] AudioClip collideSounds;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.Log("Cannot find audio source for Audio Manager");
        }
    }

    public virtual void PlayCollideSound()
    {
        PlayClip(collideSounds);
    }

    private void PlayClip(AudioClip clip)
    {
        if (audioSource == null)
            return;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
