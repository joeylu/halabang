using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Halabang.Utilities;
using DG.Tweening;
using Unity.VisualScripting;

namespace Halabang.Scene {
  public class PlacerSprite : PlacerBase {
    public List<SpriteRenderer> CurrentSpriteRenderers { get; set; }

    [Tooltip("生成的 sprite 预制件")]
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private int defaultSortingOrder;
    [Tooltip("单个Sprite渐进显示时长")]
    [SerializeField] private float fadeInDuration;
    [Tooltip("单个Sprite渐出显示时长")]
    [SerializeField] private float fadeOutDuration;


    private bool hasDelayForEachSpriteVisibility;
    private void Awake() {
      hasDelayForEachSpriteVisibility = sequenceTweenerSetting.DurationValue > 0;
    }

    public void ShowSprites() {
      ShowPlacers();

      CurrentSpriteRenderers = new List<SpriteRenderer>();
      foreach (Transform place in places) {
        SpriteRenderer sr = place.AddComponent<SpriteRenderer>();
        sr.sprite = sprites.Random();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sequenceTweenerSetting.DurationValue > 0 ? 0 : 1);
        sr.sortingOrder = defaultSortingOrder;
        CurrentSpriteRenderers.Add(sr);
      }

      setSpritesVisible();
    }
    public void HideSprites() {
      if (CurrentSpriteRenderers == null || CurrentSpriteRenderers.Count == 0) return;

      setSpritesHidden();
    }

    private void setSpritesVisible() {
      float delay = 0;
      foreach (SpriteRenderer sr in CurrentSpriteRenderers) {
        sr.gameObject.SetActive(true);

        if (hasDelayForEachSpriteVisibility) {
          sr.DOFade(1, fadeInDuration)
            .SetDelay(delay);

          delay += sequenceTweenerSetting.DurationValue;
        }
      }
    }
    private void setSpritesHidden() {
      float delay = 0;
      Tweener lastTweener = null;

      for (int i = 1; i < CurrentSpriteRenderers.Count; i++) {
        if (i == CurrentSpriteRenderers.Count -1) {
          lastTweener = CurrentSpriteRenderers[i].DOFade(0, fadeOutDuration).
            SetDelay(delay)
            .OnComplete(() => HidePlacers())
            .OnKill(() => HidePlacers());
        } else {
          CurrentSpriteRenderers[i].DOFade(0, fadeInDuration)
            .SetDelay(delay);
        }
        if (ignoreSequenceTweenerOnHide == false) delay += sequenceTweenerSetting.DurationValue;
      }
    }
  }
}
