using System;
using UnityEngine;

public class Extinguisher : MonoBehaviour, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public bool isSpraying;
    }

    [SerializeField] private Transform nozzleParticleSystem;
    [SerializeField] private HandleVisual handleVisual;

    [Space(10)]
    [SerializeField] private LayerMask hitableLayerMask;
    [SerializeField] private float extinguisherFuelMax = 10f;
    [SerializeField] private float extinguishPowerPerSecond = 1.0f;

    private float currentExtinguisherFuel;
    private ParticleSystem.EmissionModule nozzleEmission;

    private void Start()
    {
        nozzleEmission = nozzleParticleSystem.GetComponent<ParticleSystem>().emission;
        ScrollbarUI_SINGLE.Instance.OnScrollbarChange += ScrollbarUI_SINGLE_OnScrollbarChange;

        currentExtinguisherFuel = extinguisherFuelMax;
    }

    private void ScrollbarUI_SINGLE_OnScrollbarChange(object sender, ScrollbarUI_SINGLE.OnScrollbarChangeEventArgs e)
    {
        Vector3 newPosition = new Vector3(transform.position.x, Mathf.Lerp(0, 1.5f, e.scrollbarValue), transform.position.z);
        transform.position = newPosition;
    }

    private void Update()
    {
        if (handleVisual.GetIsPressed())
        {
            FireExtinguisher();

            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                isSpraying = IsSpraying()
            });
        }
        else
        {
            nozzleEmission.enabled = false;

            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                isSpraying = IsSpraying()
            });
        }
    }

    private void FireExtinguisher()
    {
        currentExtinguisherFuel -= Time.deltaTime;
        if (currentExtinguisherFuel > 0)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)currentExtinguisherFuel / extinguisherFuelMax
            });

            nozzleEmission.enabled = true;

            if (Physics.Raycast(nozzleParticleSystem.position, nozzleParticleSystem.forward, out RaycastHit hit, 100f, hitableLayerMask))
            {
                if (hit.collider != null &&  hit.collider.TryGetComponent(out FlamingObject flamingObject))
                {
                    flamingObject.TryExtinguish(extinguishPowerPerSecond * Time.deltaTime);
                }
            }
        }
        else
        {
            currentExtinguisherFuel = 0;
            nozzleEmission.enabled = false;
        }
    }

    private bool IsSpraying()
    {
        return handleVisual.GetIsPressed() && currentExtinguisherFuel > 0;
    }

    private void OnDestroy()
    {
        ScrollbarUI_SINGLE.Instance.OnScrollbarChange -= ScrollbarUI_SINGLE_OnScrollbarChange;
        Destroy(gameObject);
    }
}
