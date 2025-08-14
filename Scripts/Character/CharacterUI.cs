using UnityEngine;
using Halabang.Plugin;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Halabang.Utilities;

namespace Halabang.Character {
  public class CharacterUI : MonoBehaviour {
    public bool IsBarking { get; private set; } //是否正在显示对话泡
    public bool IsDeciding { get; private set; } //是否正在显示选项泡

    [Header("基础设定")]
    [SerializeField] private Canvas mainCanvas;
    [Header("对话泡设定")]
    [Tooltip("对应的对话泡父系Canvas Group")]
    [SerializeField] private CanvasGroup barkCG;
    [Tooltip("对应的对话泡Text组件")]
    [SerializeField] private TextMeshExtend barkText;
    [Tooltip("对话泡完全显示后，停留 x 秒后隐藏")]
    [SerializeField] private float barkStayDuration = 1;
    [Tooltip("对话泡显示隐藏缓动设置")]
    [SerializeField] TweenSetting barkTweenSetting = new TweenSetting();
    [Header("选项泡设定")]
    [Tooltip("对应的选项泡父系Canvas Group")]
    [SerializeField] private CanvasGroup menuCG;
    [Tooltip("对应的选项泡父系")]
    [SerializeField] private RectTransform menuHolder;
    [Tooltip("对应的选项泡按钮预制件")]
    [SerializeField] private ButtonManagerExt menuButtonPrefab;
    [Tooltip("选项泡完全显示后，停留 x 秒后隐藏，0为不自动隐藏")]
    [SerializeField] private float menuResponseTime = 0;
    [Tooltip("选项泡显示隐藏缓动设置")]
    [SerializeField] TweenSetting menuTweenSetting = new TweenSetting();


    private CharacterBase character;
    private Coroutine hideBarkTransition;
    private Coroutine hideMenuOnTimeoutTransition;
    private List<ButtonManagerExt> currentMenuOptions;

    private void Awake() {
      character = GetComponent<CharacterBase>();
    }

    /// <summary>
    /// 激活对话泡
    /// </summary>
    /// <param name="text">对话内容</param>
    /// <param name="wait">等待 x 秒后隐藏对话泡，-1 则不隐藏</param>
    public void Bark(string text, float wait = 0) {
      IsBarking = true;

      if (barkText == null || barkCG == null) return;
      wait = wait > 0 ? wait : barkStayDuration; //如果小于0，则使用默认停留时间

      barkCG.DOFade(1, barkTweenSetting.Duration)
        .SetDelay(barkTweenSetting.Delay)
        .SetEase(barkTweenSetting.EaseType)
        .OnComplete(() => {
          barkCG.blocksRaycasts = true;
          barkText.TriggerText(text, wait);
          if (hideBarkTransition != null) StopCoroutine(hideBarkTransition);
          hideBarkTransition = StartCoroutine(hideBark());
        });
    }
    private IEnumerator hideBark() {
      yield return null;
      //Debug.Log(name + " 正在触发文字显示");
      while (barkText.IsTriggering) {
        yield return null;
      }
      //Debug.Log(name + " 正在完成文字显示触发过程");
      barkCG.DOFade(0, barkTweenSetting.Duration)
        .SetDelay(barkTweenSetting.Delay)
        .SetEase(barkTweenSetting.EaseType)
        .OnComplete(() => {
          barkCG.blocksRaycasts = false;
          IsBarking = false;
        });
    }
    /// <summary>
    /// 激活选项对话泡
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pairs"></param>
    /// <param name="callback"></param>
    public void ShowMenu<T>(List<KeyValuePair<T, string>> pairs, Action<T> callback) {
      if (pairs == null) return;

      currentMenuOptions = new List<ButtonManagerExt>();
      foreach (KeyValuePair<T, string> pair in pairs) {
        ButtonManagerExt optionBtn = Instantiate(menuButtonPrefab, menuHolder.transform);
        optionBtn.name = "option btn";
        optionBtn.TargetButton.SetText(pair.Value);
        if (callback != null) optionBtn.onClick.AddListener(() => { callback.Invoke(pair.Key); }); //添加回调事件
      }

      menuCG.DOFade(1, menuTweenSetting.Duration)
        .SetDelay(menuTweenSetting.Delay)
        .SetEase(menuTweenSetting.EaseType)
        .OnComplete(() => {
          menuCG.interactable = true;
          menuCG.blocksRaycasts = true;
          if (menuResponseTime > 0) {
            if (hideMenuOnTimeoutTransition != null) StopCoroutine(hideMenuOnTimeoutTransition);
            hideMenuOnTimeoutTransition = StartCoroutine(hideMenuOnTimeoutSequence());
          }
        });
    }
    public void HideMenu() {
      menuCG.DOFade(0, menuTweenSetting.Duration)
        .SetDelay(menuTweenSetting.Delay)
        .SetEase(menuTweenSetting.EaseType)
        .OnStart(() => { menuHolder.DestroyChildren(); })
        .OnComplete(() => {
          menuCG.interactable = false;
          menuCG.blocksRaycasts = false;
        });
    }

    private IEnumerator hideMenuOnTimeoutSequence() {
      if (menuResponseTime <= 0) yield break;
    }
  }
}
