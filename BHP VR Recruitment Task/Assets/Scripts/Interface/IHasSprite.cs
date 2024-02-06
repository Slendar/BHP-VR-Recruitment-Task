using System;
using TMPro;
using UnityEngine;

public interface IHasSprite
{
    public event EventHandler<OnSpriteChangedEventArgs> OnSpriteChanged;
    public class OnSpriteChangedEventArgs : EventArgs
    {
        public Sprite spriteNext;
        public string whatToClickNext;
        public bool isLastSprite;
    }
}
