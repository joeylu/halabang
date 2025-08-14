using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Febucci.UI;
using DG.Tweening;
using Halabang.Plugin;
using System;
using static UnityEngine.Rendering.DebugUI;

namespace Halabang.Plugin {
  public class TextMeshExtend : MonoBehaviour {
    [Flags]
    public enum UI_EFFECT { NULL, IMAGE_FILL, IMAGE_COLOR }
    public TextMeshProUGUI TargetText { get { getTarget(); return targetText; } }
    public bool IsShowing => isShowing(); //是否正在显示文字
    public bool IsTriggering { get; private set; } //是否正在触发文字显示中

    [Header("Behavior settings")]
    [Tooltip("渐进显示或隐藏文字")]
    [SerializeField] private float fadingTransitionDuration;
    [Header("Layout settings")]
    //[SerializeField] private UI_Dictionary.FONT_TYPE defaultFontType;
    [Tooltip("Center the text when it displays in one line, but align by default when multiple lines")]
    [SerializeField] private bool centerTextDynamic;
    [Tooltip("When ticked, this text mesh component will adjust an assigned rect transform heigh accordingly.")]
    [SerializeField] private RectTransform fitHeightOnRect;
    [Tooltip("When ticked, this text mesh component will expand its layout element component min height accordingly. The layout element should be attached itself or parent transform")]
    [SerializeField] private bool fitHeightOnLayoutElement;
    //[SerializeField] private LocalizeSettings localizeSettings;
    [SerializeField] private bool disableOnHidden;
    [Header("UI Effects")]
    [SerializeField] private UI_EFFECT targetEffects;
    [SerializeField] private Image targetImage;
    [SerializeField] private Color targetColorEffect;
    [SerializeField] private float targetFloatEffect;
    [SerializeField] private TweenSetting effectTweenerSetting = new TweenSetting();
    [Header("Development only")]
    public bool enableDebugger;

    private TextMeshProUGUI targetText;
    private LayoutElement layoutElement;
    private HorizontalAlignmentOptions originHorizontalAlignment;
    private Coroutine postRenderTransition;
    private bool isPendingPostRenderSet;
    private TextAnimator_TMP targetTextAnimator;
    private TypewriterByCharacter targetTypewriter;

    private Color originColorEffect;
    private float originFloatEffect;
    private Coroutine triggerTextTransition;
    private Tweener effects_UI_Tweener;

    private void Awake() {
      getTarget();
    }
    private void Start() {
      preRenderSet(targetText.text);
      if (postRenderTransition != null) StopCoroutine(postRenderTransition);
      postRenderTransition = StartCoroutine(postRenderSet());
    }
    private void OnEnable() {
      if (isPendingPostRenderSet) {
        isPendingPostRenderSet = false;
        if (postRenderTransition != null) StopCoroutine(postRenderTransition);
        postRenderTransition = StartCoroutine(postRenderSet());
      }
    }

    public void SetText(string value) {
      //if (localizeSettings.TextType != UI_Dictionary.TEXT_TYPE.STATIC_TEXT && value.IsLocalizeKey() == false) {
      //  Debug.LogError("Unexpected error: cannot set text because it is not a static text type of " + name + " and value is not a localize key");
      //  return;
      //}

      string text = value;
      //if (enableDebugger) Debug.Log(name + " is setting text with " + value + " > " + value.IsLocalizeKey());
      //if (value.IsLocalizeKey()) {
      //  text = value.GetLocalizedValue();
      //}

      getTarget();
      preRenderSet(text); //define alignments first

      if (targetTypewriter) {
        targetTypewriter.ShowText(text);
      } else {
        targetText.SetText(text);
      }

      if (fadingTransitionDuration > 0) {
        targetText.DOFade(string.IsNullOrWhiteSpace(value) ? 0 : 1, fadingTransitionDuration);
      }

      if (gameObject.activeInHierarchy) {
        if (postRenderTransition != null) StopCoroutine(postRenderTransition);
        postRenderTransition = StartCoroutine(postRenderSet());
      } else {
        //sometimes SetText is called while this component is not yet active, hold the coroutine until it is activated
        isPendingPostRenderSet = true;
      }

      if (targetEffects != UI_EFFECT.NULL) ShowImageEffect();
    }
    public void TriggerText(string value, float wait) {
      if (triggerTextTransition != null) StopCoroutine(triggerTextTransition);
      triggerTextTransition = StartCoroutine(triggerText(value, wait));      
    }
    public void HideText() {
      getTarget();

      if (targetTypewriter) {
        targetTypewriter.StartDisappearingText();
      } else {
        targetText.SetText("");
      }

      if (fadingTransitionDuration > 0) {
        targetText.DOFade(0, fadingTransitionDuration);
      }

      if (targetEffects != UI_EFFECT.NULL) HideImageEffect();
    }
    public void SetColor(Color color, TweenSetting tweenSetting = null) {
      if (tweenSetting == null || tweenSetting.DurationValue == 0) {
        targetText.color = color;
      } else {
        targetText.DOColor(color, tweenSetting.DurationValue)
          .SetEase(tweenSetting.EaseType)
          .SetDelay(tweenSetting.Delay)
          .SetLoops(tweenSetting.LoopCycle, tweenSetting.LoopType);
      }
    }

    public void ShowImageEffect() {
      setImageEffect(true);
    }
    public void HideImageEffect() {
      setImageEffect(false);
    }
    private void setImageEffect(bool show) {
      if (targetImage == null) return;

      effects_UI_Tweener = null;

      if (targetEffects.HasFlag(UI_EFFECT.IMAGE_FILL)) {
        float effectFloat = show ? targetFloatEffect : originFloatEffect;
        if (effectTweenerSetting.DurationValue == 0) {
          targetImage.fillAmount = effectFloat;
        } else {
          effects_UI_Tweener = targetImage.DOFillAmount(effectFloat, effectTweenerSetting.DurationValue);
        }
      }
      if (targetEffects.HasFlag(UI_EFFECT.IMAGE_COLOR)) {
        Color effectFloat = show ? targetColorEffect : originColorEffect;
        if (effectTweenerSetting.DurationValue == 0) {
          targetImage.color = effectFloat;
        } else {
          effects_UI_Tweener = targetImage.DOColor(effectFloat, effectTweenerSetting.DurationValue);
        }
      }

      if (effects_UI_Tweener != null) {
        effects_UI_Tweener.SetDelay(effectTweenerSetting.Delay)
          .SetEase(effectTweenerSetting.EaseType)
          .SetLoops(effectTweenerSetting.LoopCycle, effectTweenerSetting.LoopType);
      }      
    }

    private void getTarget() {
      if (enableDebugger) Debug.Log("initialize " + name + " : " + transform.parent.name);
      if (targetText != null) return;
      targetText = GetComponent<TextMeshProUGUI>();
      if (targetText == null) Debug.LogError(name + " must pair with a valid text mesh pro GUI");
      targetTextAnimator = GetComponent<TextAnimator_TMP>();
      targetTypewriter = GetComponent<TypewriterByCharacter>();
      //if (enableDebugger) Debug.Log(targetTextAnimator?.animationLoop + " : " + targetTypewriter?.enabled);

      if (fitHeightOnLayoutElement) {
        layoutElement = GetComponent<LayoutElement>();
        if (layoutElement == null) layoutElement = transform.parent.GetComponent<LayoutElement>();
        if (layoutElement == null) Debug.LogError(name + " is ticked with enableContentFitterHeight but has no layout element component attached to itself or its parent");
      }
      originHorizontalAlignment = targetText.horizontalAlignment;
      //if (enableDebugger) Debug.Log("current reference " + GameManager.instatnce.CurrentPreferences);
      //if (GameManager.instatnce.CurrentPreferences != null) setLanguage(); //initialized language at awake at once, only if the game preference has set, otherwise, let the event listener to handle language change
      //GameManager.instatnce.CurrentSaveLoadManager.GameSettingsHolder.OnLanguageChange.AddListener(setLanguage);
      if (enableDebugger) Debug.Log(name + " is initialized");
    }
    private bool isShowing() {
      bool isShowingText = false;
      //如果有打字机效果，并企鹅正在显示文字，回传false
      if (targetTypewriter && targetTypewriter.isShowingText) isShowingText = true;
      //如果有文本淡入，并且文本淡入尚未完成，回传false
      if (fadingTransitionDuration > 0 && TargetText.color.a < 1f) isShowingText = true;
      //如果有UI效果，并且UI效果正在播放，回传false
      if (effects_UI_Tweener != null && effects_UI_Tweener.IsActiveAndPlaying()) isShowingText = true;

      //Debug.Log(name + " 正在显示文章 " + isShowingText);
      return isShowingText;
    }
    private bool isHiding() {
      bool isHidingText = false;
      //Debug.Log(name + " 正在隐藏文字 " + isHidingText);
      //如果有打字机效果，并企鹅正在显示文字，回传false
      if (targetTypewriter && targetTypewriter.isHidingText) isHidingText = true;
      //如果有文本淡入，并且文本淡入尚未完成，回传false
      if (fadingTransitionDuration > 0 && TargetText.color.a > 0f) isHidingText = true;
      //如果有UI效果，并且UI效果正在播放，回传false
      if (effects_UI_Tweener != null && effects_UI_Tweener.IsActiveAndPlaying()) isHidingText = true;

      //Debug.Log(name + " 完成隐藏文字 " + isHidingText);
      return isHidingText;
    }

    //private void setLanguage() {
    //  if (enableDebugger) Debug.Log("current is " + GameManager.instatnce.CurrentPreferences.CurrentLanguage);
    //  UIHelper.SetFont(targetText, defaultFontType);
    //  UIHelper.SetText(localizeSettings.TextType, localizeSettings.TextKey, targetText.SetText);
    //}
    private void preRenderSet(string text) {
      //setAlignments
      if (centerTextDynamic) targetText.horizontalAlignment = targetText.GetTextInfo(text).lineCount == 1 ? HorizontalAlignmentOptions.Center : targetText.horizontalAlignment = originHorizontalAlignment;
    }
    private IEnumerator postRenderSet() {
      yield return null; //one frame after text are set and rendered
      //setContentFitter
      if (fitHeightOnRect || fitHeightOnLayoutElement) {
        //Debug.Log(name + " is set text with prefered height " + targetText.preferredHeight + " : " + targetText.rectTransform.sizeDelta);
        if (targetText.rectTransform.sizeDelta != Vector2.zero && GetComponentInParent<LayoutGroup>() == false) Debug.LogError(name + " fit content height is ticked but this rect transform has offset values, there will be conflict while calculating the preferredHeight");
        if (fitHeightOnRect) {
          fitHeightOnRect.sizeDelta = new Vector2(fitHeightOnRect.sizeDelta.x, targetText.preferredHeight);
        }
        if (fitHeightOnLayoutElement && layoutElement) {
          layoutElement.minHeight = targetText.preferredHeight;
        }
      }
    }
    private IEnumerator triggerText(string value, float wait) {
      IsTriggering = true;

      //开始显示文字
      SetText(value);
      yield return null;
      //如果有打字机效果，等打字机完成文字显示
      //Debug.Log(name + " 触发中: 正在显示文字");
      while (isShowing()) {
        yield return null;
      }

      //等待一段时间后隐藏文字
      yield return new WaitForSeconds(wait);

      //开始隐藏文字
      HideText();
      yield return null;
      //Debug.Log(name + " 触发中: 正在隐藏文字");
      while (isHiding()) {
        yield return null;
      }
      yield return null;

      IsTriggering = false;
    }
  }
}
