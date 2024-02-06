using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleVisual : AnimationCaller, IHasSprite
{
    private const string IS_PRESSED = "IsPressed";

    public event EventHandler<IHasSprite.OnSpriteChangedEventArgs> OnSpriteChanged;

    [SerializeField] private NozzleVisual nozzleVisual;

    private Animator animator;

    private bool isExtinguisherReadyToFire = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        nozzleVisual.OnNozzlePlaced += NozzleVisual_OnNozzlePlaced;
    }

    private void NozzleVisual_OnNozzlePlaced(object sender, System.EventArgs e)
    {
        isExtinguisherReadyToFire = true;
    }

    public override void PlayAnimation()
    {
        if (isExtinguisherReadyToFire)
        {
            animator.SetBool(IS_PRESSED, true);

            OnSpriteChanged?.Invoke(this, new IHasSprite.OnSpriteChangedEventArgs
            {
                isLastSprite = true
            });
        }
    }

    public override void PlayExitAnimation()
    {
        animator.SetBool(IS_PRESSED, false);
    }

    public bool GetIsPressed()
    {
        return animator.GetBool(IS_PRESSED);
    }

    private void OnDestroy()
    {
        nozzleVisual.OnNozzlePlaced -= NozzleVisual_OnNozzlePlaced;
    }
}
