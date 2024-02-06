using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoltAndSealVisual : AnimationCaller, IHasSprite
{
    private const string BOLT_AND_SEAL_UNLOCK = "BoltAndSealUnlock";

    public event EventHandler OnBoltUnlock;
    public event EventHandler<IHasSprite.OnSpriteChangedEventArgs> OnSpriteChanged;

    [SerializeField] private Rigidbody boltRigidbody;
    [SerializeField] private Rigidbody sealRigidbody;
    [SerializeField] private Sprite spriteTutorialImageNext;

    private string whatToClickNext = "Nozzle";

    private Animator animator;
    private BoxCollider boxCollider;

    private bool wasUnlockedBefore = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public override void PlayAnimation()
    {
        if(wasUnlockedBefore) return;

        wasUnlockedBefore = true;


        boxCollider.enabled = false;

        animator.SetTrigger(BOLT_AND_SEAL_UNLOCK);
    }

    public void OnAnimationEnd() //This function is called by the animation at the end
    {
        animator.enabled = false;
        boltRigidbody.isKinematic = false;
        sealRigidbody.isKinematic = false;

        transform.SetParent(null);

        OnBoltUnlock?.Invoke(this, EventArgs.Empty);

        OnSpriteChanged?.Invoke(this, new IHasSprite.OnSpriteChangedEventArgs
        {
            spriteNext = spriteTutorialImageNext,
            whatToClickNext = whatToClickNext,
            isLastSprite = false
        });
    }
}
