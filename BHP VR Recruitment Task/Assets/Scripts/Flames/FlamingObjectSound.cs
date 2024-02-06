using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingObjectSound : MonoBehaviour
{
    [SerializeField] private FlamingObject flamingObject;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null ) 
        {
            Debug.LogError(gameObject + " don't have an AudioSource");
        }
    }

    private void Update()
    {
        audioSource.volume = flamingObject.GetFlamingIntensity();

        if (flamingObject.GetFlamingIntensity() <= 0)
        {
            audioSource.Pause();
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
