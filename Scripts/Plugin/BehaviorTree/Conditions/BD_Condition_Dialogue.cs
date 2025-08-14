using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using Halabang.Game;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("对话检查条件")]
  public class BD_Condition_Dialogue : Conditional {
    public enum CONDITION_NAME {
      NULL,
      IS_IN_CONVERSATION
    }

    public CONDITION_NAME condition;
    public Transform target;
    public bool targetBoolean;

    public override TaskStatus OnUpdate() {
      if (checkCondition()) {
        return TaskStatus.Success;
      } else {
        return TaskStatus.Failure;
      }
    }

    private bool checkCondition() {
      switch (condition) {
        case CONDITION_NAME.IS_IN_CONVERSATION:
          if (target && target != GameManager.Instance._DialogueManger.CurrentDialogueController.transform) return false; //若对话控件不匹配，总是返回 false
          if (GameManager.Instance._DialogueManger.CurrentDialogueController) {
            return GameManager.Instance._DialogueManger.CurrentDialogueController.IsTriggering == targetBoolean;
          } else {
            return !targetBoolean;
          }
        default:
          return false;
      }
    }
  }
}
