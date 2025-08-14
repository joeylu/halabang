using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using Halabang.Audio;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("执行歌曲播放相关的各种行为")]
  public class BD_Action_MusicPlayer : Action {
    public enum ACTION_NAME {
      NULL,
      PLAY,
      STOP,
      PAUSE,
      PREV_SOUND,
      NEXT_SOUND,
      VOLUME
    }

    public ACTION_NAME triggerAction;
    [Tooltip("对应的歌曲控件")]
    public SoundListController targetListController;


    public override void OnStart() {
      callAction();
    }
    public override TaskStatus OnUpdate() {
      return TaskStatus.Success;
    }
    private void callAction() { 
      switch (triggerAction) {
        case ACTION_NAME.PLAY:
          targetListController.Play();
          break;
        case ACTION_NAME.STOP:
          targetListController.Stop();
          break;
        case ACTION_NAME.PAUSE:
          targetListController.Pause();
          break;
        case ACTION_NAME.PREV_SOUND:
          targetListController.Prev();
          break;
        case ACTION_NAME.NEXT_SOUND:
          targetListController.Next();
          break;
      }
    }
  }
}
