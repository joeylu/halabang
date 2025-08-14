﻿using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using DG.Tweening;
using System.Linq;
using Halabang.Scene;
using UnityEngine.UI;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("图像变换行为")]
  public class BD_Action_Image : Action {
    public enum ACTION_TYPE {
      Null,
      Sprite_Color,
      Sprite_Sort_Offset,
      Sprite_Mask_Range_Offset,
      Sprite_Sort_Layer,
      Sprite_Placer_Show,
      Sprite_Placer_Hide,
      Image_Color,
      Image_Fill
    }

    public ACTION_TYPE action;
    public Transform target;
    public bool includeChildren;
    public Color targetColor;
    public int targetInt;
    public float targetFloat;
    public string targetString;
    public TweenSetting tweenSetting = new TweenSetting();

    private List<SpriteRenderer> targetSprites = new List<SpriteRenderer>();
    private List<Image> targetImages = new List<Image>();

    public override void OnStart() {
      switch (action) {
        case ACTION_TYPE.Sprite_Color:
        case ACTION_TYPE.Sprite_Sort_Offset:
        case ACTION_TYPE.Sprite_Sort_Layer:
          callSpriteAction();
          break;
        case ACTION_TYPE.Sprite_Mask_Range_Offset:
          callSpriteMaskAction();
          break;
        case ACTION_TYPE.Sprite_Placer_Show:
        case ACTION_TYPE.Sprite_Placer_Hide:
          callSpritePlacingAction();
          break;
        case ACTION_TYPE.Image_Color:
        case ACTION_TYPE.Image_Fill:
          callImageAction();
          break;
      }
    }
    public override TaskStatus OnUpdate() {
      return TaskStatus.Success;
    }

    private void callSpriteAction() {
      SpriteRenderer[] sprites = target.GetComponentsInChildren<SpriteRenderer>(true);
      if (sprites != null) {
        if (includeChildren) {
          targetSprites = sprites.ToList();
        } else {
          targetSprites.Add(sprites[0]);
        }
      }
      foreach (SpriteRenderer sr in targetSprites) {
        switch (action) {
          case ACTION_TYPE.Sprite_Color:
            //Debug.Log("dsdfasdfaf: " + sr.transform.parent.name + " > " + sr.color.a + " >>> " + targetColor.a);
            if (sr == null || targetColor == null || tweenSetting == null) Debug.LogError(FriendlyName + " : " + sr + " : " + targetColor + " : " + tweenSetting);
            sr.DOColor(targetColor, tweenSetting.Duration)
              //.SetUpdate(UpdateType.Late)
              .SetDelay(tweenSetting.Delay)
              .SetLoops(tweenSetting.LoopCycle, tweenSetting.LoopType)
              .SetEase(tweenSetting.EaseType);
            break;
          case ACTION_TYPE.Sprite_Sort_Offset:
            sr.sortingOrder += targetInt;
            break;
          case ACTION_TYPE.Sprite_Sort_Layer:
            sr.sortingLayerName = targetString;
            break;
        }
      }
    }
    private void callSpriteMaskAction() {
      SpriteMask mask = target.GetComponent<SpriteMask>();
      if (mask) {
        switch (action) {
          case ACTION_TYPE.Sprite_Mask_Range_Offset:
            if (mask.isCustomRangeActive) {
              mask.frontSortingOrder += targetInt;
              mask.backSortingOrder += targetInt;
            }
            break;
        }
      }
    }
    private void callSpritePlacingAction() {
      PlacerSprite placer = target.GetComponent<PlacerSprite>();
      if (placer == null) { return; }

      switch (action) {
        case ACTION_TYPE.Sprite_Placer_Show:
          placer.ShowSprites();
          break;
        case ACTION_TYPE.Sprite_Placer_Hide:
          placer.HideSprites();
          break;
      }
    }
    private void callImageAction() {
      Image[] images = target.GetComponentsInChildren<Image>(true);
      if (images != null) {
        if (includeChildren) {
          targetImages = images.ToList();
        } else {
          targetImages.Add(images[0]);
        }
      }
      foreach (Image image in targetImages) {
        Tweener tweener = null;
        switch (action) {
          case ACTION_TYPE.Image_Color:
            tweener = image.DOColor(targetColor, tweenSetting.Duration);
            break;
          case ACTION_TYPE.Image_Fill:
            tweener = image.DOFillAmount(targetFloat, tweenSetting.Duration);
            break;
        }
        if (tweener != null) tweener
              .SetDelay(tweenSetting.Delay)
              .SetLoops(tweenSetting.LoopCycle, tweenSetting.LoopType)
              .SetEase(tweenSetting.EaseType);
      }
    }
  }
}