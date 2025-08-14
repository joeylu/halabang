using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using Halabang.Game;
using DG.Tweening;
using TransitionScreenPackage;
using UnityEngine.UI;
using Michsky.UI;
using Michsky.UI.MTP;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("")]
  public class BD_Action_UI : Action {
    public enum ACTION_NAME {
      NULL,
      CANVAS_GROUP_TRANSISTION,
      TOGGLE_CANVAS_RAYCASTER,
      TOGGLE_TRANSITION_EFFECT,
      SHOW_TRANSITION_EFFECT,
      HIDE_TRANSITION_EFFECT,
      SHOW_MOTION_TITLE,
      HIDE_MOTION_TITLE,
      SHOW_CANVAS_GROUP,
      HIDE_CANVAS_GROUP
    }

    public ACTION_NAME triggerAction;
    public RectTransform currentUI;
    public RectTransform targetUI;
    public TransitionManager.TransitionType transitionType;
    public int targetInt;
    public TweenSetting tweenSetting;

    private CanvasGroup currentCG;
    private CanvasGroup targetCG;
    private GraphicRaycaster raycaster;
    private StyleManager motionTitle;
    private TransitionScreenManager transitionScreen = null;

    public override void OnStart() {
      if (triggerAction == ACTION_NAME.CANVAS_GROUP_TRANSISTION) {
        currentCG = currentUI.GetComponent<CanvasGroup>();
        targetCG = targetUI.GetComponent<CanvasGroup>();
      }
      if (triggerAction == ACTION_NAME.TOGGLE_CANVAS_RAYCASTER) {
        raycaster = targetUI.GetComponent<GraphicRaycaster>();
      }
      if (triggerAction == ACTION_NAME.SHOW_MOTION_TITLE || triggerAction == ACTION_NAME.HIDE_MOTION_TITLE) {
        motionTitle = targetUI.GetComponent<StyleManager>();
      }

      callAction();
    }
    public override TaskStatus OnUpdate() {
      return TaskStatus.Success;
    }

    private void callAction() {
      if (targetUI) {
        transitionScreen = targetUI.GetComponent<TransitionScreenManager>();
      }
      //transitionScreen = targetUI.GetComponent<TransitionScreenManager>();

      switch (triggerAction) {
        case ACTION_NAME.CANVAS_GROUP_TRANSISTION:
          targetCG.blocksRaycasts = true;
          targetCG.interactable = true;
          DOTween.To(() => targetCG.alpha, value => targetCG.alpha = value, 1f, tweenSetting.Duration);
          break;
        case ACTION_NAME.TOGGLE_CANVAS_RAYCASTER:
          raycaster.enabled = !raycaster.enabled;
          break;
        case ACTION_NAME.TOGGLE_TRANSITION_EFFECT:
          GameManager.Instance._UIManager._TransitionManager.ToggleTransition(targetInt, transitionType);
          break;
        case ACTION_NAME.SHOW_TRANSITION_EFFECT:
          transitionScreen.Reveal();
          break;
        case ACTION_NAME.HIDE_TRANSITION_EFFECT:
          transitionScreen.Hide();
          break;
        case ACTION_NAME.SHOW_MOTION_TITLE:
          motionTitle.PlayIn();
          break;
        case ACTION_NAME.HIDE_MOTION_TITLE:
          motionTitle.PlayOut();
          break;
        case ACTION_NAME.SHOW_CANVAS_GROUP:
          targetCG = targetUI.GetComponent<CanvasGroup>();
          DOTween.To(() => targetCG.alpha, value => targetCG.alpha = value, 1f, tweenSetting.Duration);
          break;
        case ACTION_NAME.HIDE_CANVAS_GROUP:
          targetCG = targetUI.GetComponent<CanvasGroup>();
          DOTween.To(() => targetCG.alpha, value => targetCG.alpha = value, 0f, tweenSetting.Duration);
          break;
      }
    }
  }
}
