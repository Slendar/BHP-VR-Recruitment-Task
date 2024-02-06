using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NozzleVisual : AnimationCaller, IHasSprite
{
    public const string PLACE_NOZZLE = "PlaceNozzle";

    public event EventHandler<IHasSprite.OnSpriteChangedEventArgs> OnSpriteChanged;
    public event EventHandler OnNozzlePlaced;

    [SerializeField] private BoltAndSealVisual boltAndSealVisual;
    [SerializeField] private Sprite spriteTutorialImageNext;

    private string whatToClickNext = "Handle";

    private Animator animator;
    private BoxCollider boxCollider;

    private bool wasPlacedBefore = false;
    private bool isExtinguisherUnlocked = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();

        boltAndSealVisual.OnBoltUnlock += BoltAndSealVisual_OnBoltUnlock;    
    }

    private void BoltAndSealVisual_OnBoltUnlock(object sender, System.EventArgs e)
    {
        isExtinguisherUnlocked = true;
    }

    public override void PlayAnimation()
    {
        if (wasPlacedBefore) return;


        if (isExtinguisherUnlocked)
        {
            wasPlacedBefore = true;

            boxCollider.enabled = false;

            animator.SetTrigger(PLACE_NOZZLE);
        }
    }

    public void OnAnimationEnd() //This function is called by the animation at the end
    {
        animator.enabled = false;

        OnNozzlePlaced?.Invoke(this, EventArgs.Empty);

        OnSpriteChanged?.Invoke(this, new IHasSprite.OnSpriteChangedEventArgs
        {
            spriteNext = spriteTutorialImageNext,
            whatToClickNext = whatToClickNext,
            isLastSprite = false
        });
    }

    private void OnDestroy()
    {
        boltAndSealVisual.OnBoltUnlock -= BoltAndSealVisual_OnBoltUnlock;
    }
}
