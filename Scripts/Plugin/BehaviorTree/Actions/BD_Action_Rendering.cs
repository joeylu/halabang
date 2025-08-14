using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using UnityEngine.Rendering;
using DG.Tweening;
using Halabang.Game;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("")]
  public class BD_Action_Rendering : Action {
    public enum ACTION_NAME {
      NULL,
      VOLUME_TRANSISTION,
      RESUME_DEFAULT_VOLUME
    }

    public ACTION_NAME triggerAction;
    public Volume targetVolume;
    public TweenSetting tweenSetting = new TweenSetting();

    public override void OnStart() {
      callAction();
    }
    public override TaskStatus OnUpdate() {
      return TaskStatus.Success;
    }

    private void callAction() {
      switch (triggerAction) {
        case ACTION_NAME.VOLUME_TRANSISTION:
          DOTween.To(() => GameManager.Instance.CurrentSceneManager.CurrentVolume.weight, value => GameManager.Instance.CurrentSceneManager.CurrentVolume.weight = value, 0f, tweenSetting.Duration)
            .OnComplete(() => GameManager.Instance.CurrentSceneManager.CurrentVolume = targetVolume);
          DOTween.To(() => targetVolume.weight, value => targetVolume.weight = value, 1f, tweenSetting.Duration);
          break;
        case ACTION_NAME.RESUME_DEFAULT_VOLUME:
          GameManager.Instance.CurrentSceneManager.ResumeDefaultVolume(tweenSetting);
          break;
      }
    }
  }
}
