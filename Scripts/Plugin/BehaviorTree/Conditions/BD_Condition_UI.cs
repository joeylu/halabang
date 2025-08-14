using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using Halabang.Game;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("UI检查条件")]
  public class BD_Condition_UI : Conditional {
    public enum CONDITION_NAME {
      NULL,
      IS_IN_TRANSITION
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
        case CONDITION_NAME.IS_IN_TRANSITION:
          return GameManager.Instance._UIManager._TransitionManager.IsTransitioning == targetBoolean;
        default:
          return false;
      }
    }
  }
}