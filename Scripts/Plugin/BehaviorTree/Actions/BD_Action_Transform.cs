using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using DG.Tweening;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("Transform变换行为")]
  public class BD_Action_Transform : BehaviorDesigner.Runtime.Tasks.Action {
    [Flags]
    public enum ACTION_TYPE {
      Null = 1, 
      Scale = 2, 
      Position = 4, 
      Rotation = 8
    }

    public ACTION_TYPE action;
    public Transform target;
    public bool includeChildren;
    public bool isWorldPosition;
    public Vector3 targetPosition;
    public bool isWorldRotation;
    public Vector3 targetRotation;
    public Vector3 targetScale;
    public TweenSetting tweenSetting;

    private List<Transform> targetTransforms = new List<Transform>();

    public override void OnStart() {
      callAction();
    }
    public override TaskStatus OnUpdate() {
      return TaskStatus.Success;
    }

    private void callAction() {
      Transform[] transforms = target.GetComponentsInChildren<Transform>(true);
      if (transforms != null) {
        if (includeChildren) {
          targetTransforms = transforms.ToList();
        } else {
          targetTransforms.Add(transforms[0]);
        }
      }

      foreach (Transform t in targetTransforms) {
        if (action.HasFlag(ACTION_TYPE.Position)) {
          Tweener positionTweener;
          if (isWorldPosition) {
            positionTweener = t.DOMove(targetPosition, tweenSetting.Duration);
          } else {
            positionTweener = t.DOLocalMove(targetPosition, tweenSetting.Duration);
          }
          positionTweener
            .SetDelay(tweenSetting.Delay)
            .SetLoops(tweenSetting.LoopCycle, tweenSetting.LoopType)
            .SetEase(tweenSetting.EaseType);
        }
        if (action.HasFlag(ACTION_TYPE.Rotation)) {
          Tweener rotationTweener;
          if (isWorldRotation) {
            rotationTweener = t.DORotate(targetRotation, tweenSetting.Duration);
          } else {
            rotationTweener = t.DOLocalRotate(targetRotation, tweenSetting.Duration);
            Debug.Log(targetRotation + " : " + rotationTweener.Duration(false));
          }
          rotationTweener
            .SetDelay(tweenSetting.Delay)
            .SetLoops(tweenSetting.LoopCycle, tweenSetting.LoopType)
            .SetEase(tweenSetting.EaseType);
        }
        if (action.HasFlag(ACTION_TYPE.Scale)) {
          t.DOScale(targetScale, tweenSetting.Duration)
            .SetDelay(tweenSetting.Delay)
            .SetLoops(tweenSetting.LoopCycle, tweenSetting.LoopType)
            .SetEase(tweenSetting.EaseType);
        }
      }
    }
  }
}
