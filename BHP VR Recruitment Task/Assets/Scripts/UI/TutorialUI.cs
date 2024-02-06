using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI whatToClickText;

    [SerializeField] private GameObject boltAndSealVisual;
    [SerializeField] private GameObject nozzleVisual;
    [SerializeField] private GameObject handleVisual;

    private IHasSprite hasSpriteBoltAndSealVisual;
    private IHasSprite hasSpriteNozzleVisual;
    private IHasSprite hasSpriteHandleVisual;

    private void Start()
    {
        hasSpriteBoltAndSealVisual = boltAndSealVisual.GetComponent<IHasSprite>();
        if (hasSpriteBoltAndSealVisual == null)
        {
            Debug.LogError("GameObject" + boltAndSealVisual + "does not have component that implements IHasProgress!");
        }

        hasSpriteNozzleVisual = nozzleVisual.GetComponent<IHasSprite>();
        if (hasSpriteNozzleVisual == null)
        {
            Debug.LogError("GameObject" + nozzleVisual + "does not have component that implements IHasProgress!");
        }

        hasSpriteHandleVisual = handleVisual.GetComponent<IHasSprite>();
        if (hasSpriteHandleVisual == null)
        {
            Debug.LogError("GameObject" + handleVisual + "does not have component that implements IHasProgress!");
        }

        hasSpriteBoltAndSealVisual.OnSpriteChanged += IHasSprite_OnSpriteChanged;
        hasSpriteNozzleVisual.OnSpriteChanged += IHasSprite_OnSpriteChanged;
        hasSpriteHandleVisual.OnSpriteChanged += IHasSprite_OnSpriteChanged;
    }

    private void IHasSprite_OnSpriteChanged(object sender, IHasSprite.OnSpriteChangedEventArgs e)
    {
        image.sprite = e.spriteNext;
        whatToClickText.text = e.whatToClickNext;
        if (e.isLastSprite)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        hasSpriteBoltAndSealVisual.OnSpriteChanged -= IHasSprite_OnSpriteChanged;
        hasSpriteNozzleVisual.OnSpriteChanged -= IHasSprite_OnSpriteChanged;
        hasSpriteHandleVisual.OnSpriteChanged -= IHasSprite_OnSpriteChanged;
    }
}
