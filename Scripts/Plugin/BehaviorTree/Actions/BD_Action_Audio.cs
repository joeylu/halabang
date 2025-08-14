using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using Halabang.Audio;
using Halabang.Game;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("执行音乐音效相关的各种行为")]
  public class BD_Action_Audio : Action {
    public enum ACTION_NAME {
      NULL,
      PLAY_AUDIO,
      STOP_AUDIO,
      PAUSE_AUDIO,
      TWEEN_TRIGGER_VOLUME,
      TWEEN_BGM_VOLUME,
      RESET_TRIGGER_VOLUME,
      RESET_BGM_VOLUME,
      PLAY_BGM,
    }

    public ACTION_NAME triggerAction;
    public AudioTrigger audioTrigger;
    public float targetValue;
    public TweenSetting tweenSetting;


    public override void OnStart() {
      callAction();
    }
    public override TaskStatus OnUpdate() {
      return TaskStatus.Success;
    }
    private void callAction() {
      switch (triggerAction) {
        case ACTION_NAME.TWEEN_TRIGGER_VOLUME:
          audioTrigger.SetVolume(targetValue, tweenSetting);
          break;
        case ACTION_NAME.TWEEN_BGM_VOLUME:
          GameManager.Instance._SoundManager.SetBGMVolume(targetValue, tweenSetting);
          break;
        case ACTION_NAME.RESET_TRIGGER_VOLUME:
          audioTrigger.ResetVolume(tweenSetting);
          break;
        case ACTION_NAME.RESET_BGM_VOLUME:
          GameManager.Instance._SoundManager.ResetBGMVolume(tweenSetting);
          break;
        case ACTION_NAME.PLAY_BGM:
          GameManager.Instance._SoundManager.PlayBGM(audioTrigger);
          break;
        case ACTION_NAME.PLAY_AUDIO:
          audioTrigger.Play();
          break;
      }
    }
  }
}
