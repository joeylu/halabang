using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using Halabang.Game;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("场景检查条件")]
  public class BD_Condition_Scene : Conditional {
    public enum CONDITION_NAME {
      NULL,
      IS_SWITCHING_SCENE,
      IS_ADDING_SCENE
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
        case CONDITION_NAME.IS_SWITCHING_SCENE:
          return GameManager.Instance._SaveLoadManager.IsLoading == targetBoolean;
        case CONDITION_NAME.IS_ADDING_SCENE:
          return GameManager.Instance._SaveLoadManager.IsAddingScene == targetBoolean;
        default:
          return false;
      }
    }
  }
}
