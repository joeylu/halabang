using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using Unity.VisualScripting;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("执行灯光相关的各种行为")]
  public class BD_Action_Light : Action {
    public enum ACTION_NAME {
      NULL,
      LIGHT_2D_TRANSITION,
    }

    [RequiredField]
    public ACTION_NAME triggerAction;
    public Transform targetLights;
    public bool includeChildren;

    public float targetValue;
    public float targetMultiplier = 1;

    public TweenSetting tweenSetting;

    private List<Light2D> lights2D;
    private List<Light> lights3D;

    public override void OnStart() {
      lights2D = targetLights.GetComponentsInChildren<Light2D>(true)?.ToList();
      lights3D = targetLights.GetComponentsInChildren<Light>(true)?.ToList();

      if (lights2D == null) lights2D =new List<Light2D>();
      if (lights3D == null) lights3D=new List<Light>();

      callAction();
    }

    private void callAction() {
      switch (triggerAction) {
        case ACTION_NAME.LIGHT_2D_TRANSITION:
          foreach (Light2D light2D in lights2D) {
            float toValue = (light2D.intensity + targetValue) * targetMultiplier;
            if (tweenSetting.Duration > 0) {
              DOTween.To(() => light2D.intensity, value => light2D.intensity = value, toValue, tweenSetting.Duration)
                .SetDelay(tweenSetting.Delay)
                .SetEase(tweenSetting.EaseType)
                .SetLoops(tweenSetting.LoopCycle, tweenSetting.LoopType);
            } else {
              light2D.intensity = toValue;
            }
          }
          break;
      }
    }
  }
}
