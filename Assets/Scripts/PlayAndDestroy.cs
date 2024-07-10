using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAndDestroy : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log("Play");
        // Play the audio clip
        audioSource.Play();
    }

    void Update()
    {
        // Check if the audio clip has finished playing
        if (!audioSource.isPlaying)
        {
            // Destroy the GameObject this script is attached to
            Destroy(gameObject);
        }
    }
}