using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCaller : MonoBehaviour
{
    public virtual void PlayAnimation()
    {
        Debug.LogError("AnimationCaller.PlayAnimation();");
    }

    public virtual void PlayExitAnimation()
    {
        //Debug.LogError("AnimationCaller.PlayExitAnimation);
    }
}
