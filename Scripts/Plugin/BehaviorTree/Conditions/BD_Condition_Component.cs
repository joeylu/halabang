using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("组件检查条件")]
  public class BD_Condition_Component : Conditional {
    public enum CONDITION_NAME {
      NULL,
      Enabled_ColliderPointer2D
    }

    public CONDITION_NAME condition;
    public Transform target;
    public bool enabled;


    public override void OnAwake() {
    }

    public override TaskStatus OnUpdate() {
      if (checkCondition()) {
        return TaskStatus.Success;
      } else {
        return TaskStatus.Failure;
      }
    }

    private bool checkCondition() {
      switch (condition) {
        case CONDITION_NAME.Enabled_ColliderPointer2D:
          ColliderPointer2D colliderPointer = target.GetComponent<ColliderPointer2D>();
          return colliderPointer.enabled == enabled;
        default:
          return false;
      }
    }
  }
}