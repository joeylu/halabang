using UnityEngine;
using System;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using System.Collections.Generic;
using System.Linq;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("组件激活行为")]
  public class BD_Action_Component : BehaviorDesigner.Runtime.Tasks.Action {
    [Flags]
    public enum ACTION_TYPE {
      Null = 1,
      ColliderPointer2D = 2
    }

    public ACTION_TYPE action;
    public Transform target;
    public bool includeChildren;
    public bool enable;

    private List<Transform> targetTransforms = new List<Transform>();

    public override void OnStart() {
      callAction();
    }
    public override TaskStatus OnUpdate() {
      return TaskStatus.Success;
    }

    private void callAction() {
      Transform[] transforms = target.GetComponentsInChildren<Transform>();
      if (transforms != null) {
        if (includeChildren) {
          targetTransforms = transforms.ToList();
        } else {
          targetTransforms.Add(transforms[0]);
        }
      }

      foreach (Transform t in targetTransforms) {
        if (action.HasFlag(ACTION_TYPE.ColliderPointer2D)) {
          ColliderPointer2D colliderPointer = t.GetComponent<ColliderPointer2D>();
          Collider2D collider2D = t.GetComponent<Collider2D>();
          //if (collider2D) collider2D.enabled = enable;  //DO NOT disable collider, determination of mouse position out of collider is required
          if (colliderPointer) colliderPointer.enabled = enable;
        }
      }
    }
  }
}
