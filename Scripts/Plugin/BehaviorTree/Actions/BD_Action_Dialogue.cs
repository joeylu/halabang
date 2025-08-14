using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using Halabang.Audio;
using Halabang.Plugin;
using Halabang.Game;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("执行对话相关的各种行为")]
  public class BD_Action_Dialogue : Action {
    public enum ACTION_NAME {
      NULL,
      PLAY_DIALOGUE,
      STOP_DIALOGUE,
      PAUSE_DIALOGUE
    }

    [RequiredField]
    public ACTION_NAME triggerAction;
    public DialogueTriggerController dialogueTrigger;

    public override void OnStart() {
      callAction();
    }

    private void callAction() {
      switch (triggerAction) {
        case ACTION_NAME.PLAY_DIALOGUE:
          dialogueTrigger.OnUse();
          break;
        case ACTION_NAME.STOP_DIALOGUE:
          GameManager.Instance._DialogueManger.StopDialogue();
          break;
      }
    }
  }
}

