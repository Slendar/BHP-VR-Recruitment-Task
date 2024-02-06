using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireVisual : MonoBehaviour
{
    [SerializeField] private FlamingObject flamingObject;

    [SerializeField] private ParticleSystem[] fireParticleSystems;

    [Space(15)]
    [SerializeField] private Transform steamParticleSystem;

    private ParticleSystem.EmissionModule steamEmission;

    private List<ParticleSystem> allFireParticleSystems;
    private float[] startingEmmisionRate;

    private void Start()
    {
        allFireParticleSystems = new List<ParticleSystem>();
        steamEmission = steamParticleSystem.GetComponent<ParticleSystem>().emission;

        steamEmission.enabled = false;

        flamingObject.OnExtinguishing += FlamingObject_OnExtinguishing;
        flamingObject.OnNotExtinguishing += FlamingObject_OnNotExtinguishing;


        //Asign all children (particle systems) of main Fire parent to list. For later emission rates assigning 
        for(int i = 0; i < fireParticleSystems.Length; i++)
        {
            for(int j = 0; j < fireParticleSystems[i].transform.childCount; j++)
            {
                allFireParticleSystems.Add(fireParticleSystems[i].transform.GetChild(j).GetComponent<ParticleSystem>());
            }
        }

        //Assign all Emission Rates to it's particle systems. For changing it when the fire is being extinguished
        startingEmmisionRate = new float[allFireParticleSystems.Count];

        for(int i = 0; i < allFireParticleSystems.Count; i++)
        {
            startingEmmisionRate[i] = allFireParticleSystems[i].emission.rateOverTime.constant;
        }
    }


    private void FlamingObject_OnExtinguishing(object sender, FlamingObject.OnExtinguishingEventArgs e)
    {
        ChangeEmissionRate(e.flamingIntensity);

        steamEmission.enabled = true;
    }
   
    private void FlamingObject_OnNotExtinguishing(object sender, System.EventArgs e)
    {
        float flamingIntensity = flamingObject.GetFlamingIntensity();

        ChangeEmissionRate(flamingIntensity);

        steamEmission.enabled = false;
    }

    private void ChangeEmissionRate(float flamingIntensity)
    {
        for(int i = 0; i < allFireParticleSystems.Count; i++)
        {
            ParticleSystem.EmissionModule emission = allFireParticleSystems[i].emission;
            emission.rateOverTime = flamingIntensity * startingEmmisionRate[i];
        }
    }

    private void OnDestroy()
    {
        flamingObject.OnExtinguishing -= FlamingObject_OnExtinguishing;
        flamingObject.OnNotExtinguishing -= FlamingObject_OnNotExtinguishing;

        allFireParticleSystems.Clear();
    }
}
