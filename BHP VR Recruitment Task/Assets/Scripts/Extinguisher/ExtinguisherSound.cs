using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguisherSound : MonoBehaviour
{
    [SerializeField] private Extinguisher extinguisher;

    private AudioSource audioSource;
    private bool test;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        extinguisher.OnStateChanged += Extinguisher_OnStateChanged;
    }

    private void Extinguisher_OnStateChanged(object sender, Extinguisher.OnStateChangedEventArgs e)
    {
        if (e.isSpraying)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Pause();
        }
    }
}
