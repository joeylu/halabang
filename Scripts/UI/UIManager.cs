using UnityEngine;
using DG.Tweening;
using Halabang.Audio;
using Halabang.Plugin;

namespace Halabang.UI {
  public class UIManager : MonoBehaviour {
    public TransitionManager _TransitionManager { get; private set; } 

    [Header("UI 音效设置")]
    [SerializeField] private UI_SFX defaultSFX;
    [Header("居中文字设置")]
    [SerializeField] private CanvasGroup middleTextPanel;
    [SerializeField] private TextMeshExtend middleText;
    [SerializeField] private float middleTextTransitionDuration;

    private void Awake() {
      _TransitionManager = GetComponentInChildren<TransitionManager>();
    }

    public void ShowMiddleText(string text) {
      middleText.SetText(text);
      if (middleTextTransitionDuration > 0 ) {
        middleTextPanel.DOFade(1, middleTextTransitionDuration);
      }
    }
    public void HideMiddleText() {
      if (middleTextTransitionDuration > 0) {
        middleTextPanel.DOFade(0, middleTextTransitionDuration)
          .OnComplete(() => middleText.SetText(""));
      } else {
        middleText.SetText("");
      }
    }

    public void PlaySFX(AudioDictionary.UI_SFX_ACTION action, AudioDictionary.UI_SFX_THEME  them = AudioDictionary.UI_SFX_THEME.DEFAULT) {
      switch (action) {
        case AudioDictionary.UI_SFX_ACTION.HOVER:
          defaultSFX.ButtonHover.Play();
          break;
        case AudioDictionary.UI_SFX_ACTION.CLICK:
          defaultSFX.ButtonClick.Play();
          break;
      }
    }
  }
}