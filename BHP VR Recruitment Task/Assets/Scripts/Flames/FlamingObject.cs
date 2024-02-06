using System;
using System.Collections;
using UnityEngine;

public class FlamingObject : MonoBehaviour, IHasProgress
{
    public event EventHandler OnNotExtinguishing;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnExtinguishingEventArgs> OnExtinguishing;
    public class OnExtinguishingEventArgs : EventArgs
    {
        public float flamingIntensity;
    }


    [SerializeField, Range(0f, 1f)] private float flamingIntensity = 1.0f;
    [SerializeField] private float fireRegenDelay = 0.5f;
    [SerializeField] private float fireRegenRate = 0.1f;

    private float startingFlameIntensity;
    private float timeNeededToExtinguish = 6f;
    private float currentExtinguishingTime = 0f;

    private float lastTimeExtinguished = 0f;
    private bool isBeingExtinguished = false;

    private short maxFlamingIntensity = 1;

    private void Update()
    {
        if(flamingIntensity > 0 && flamingIntensity < maxFlamingIntensity && Time.time - lastTimeExtinguished >= fireRegenDelay)
        {
            currentExtinguishingTime = 0;
            isBeingExtinguished = false;

            flamingIntensity += fireRegenRate * Time.deltaTime;

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = flamingIntensity
            });

            OnNotExtinguishing?.Invoke(this, EventArgs.Empty);
        }
    }

    public void TryExtinguish(float extinguishPower)
    {
        if (!isBeingExtinguished)
        {
            startingFlameIntensity = flamingIntensity;
            isBeingExtinguished = true;
        }

        lastTimeExtinguished = Time.time;

        //Current flaming intensity depends on flame intencity when we started extinguishing and time needed to stop the flame that depends on Intensity when we're starting to extinguish
        flamingIntensity = Mathf.Lerp(startingFlameIntensity, 0, (currentExtinguishingTime / Mathf.Lerp(0.01f, timeNeededToExtinguish, startingFlameIntensity)));
        currentExtinguishingTime += extinguishPower;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = flamingIntensity
        });
        
        OnExtinguishing?.Invoke(this, new OnExtinguishingEventArgs
        {
            flamingIntensity = flamingIntensity * 0.75f
        });
    }

    public float GetFlamingIntensity()
    {
        return flamingIntensity;
    }
}
