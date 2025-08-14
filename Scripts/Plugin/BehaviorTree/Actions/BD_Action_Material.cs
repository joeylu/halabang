using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using System.Collections.Generic;
using UnityEngine.Rendering;
using DG.Tweening;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("执行材质贴图相关的各种行为")]
  public class BD_Action_Material : Action {
    public enum ACTION_NAME {
      NULL,
      SET_MATERIAL_KEYWORD,
      SET_MATERIAL_INTEGER,
      SET_MATERIAL_FLOAT,
      SET_MATERIAL_COLOR,
      SET_MATERIAL_TEXTURE,
    }

    public ACTION_NAME action;
    public Renderer targetRender;
    public bool includeChildren;
    public string targetString;
    public bool targetBoolean;
    public float targetNumber;
    public Color targetColor;
    public Vector3 targetVector;
    public Texture targetTexture;
    public TweenSetting tweenerSetting = new TweenSetting();

    private List<Material> materials = new List<Material>();

    public override void OnStart() {
      if (targetRender == null) Debug.LogError(FriendlyName + " 必须指定一个Renderer");
      if (includeChildren) {
        Renderer[] renderers = targetRender.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers) {
          materials.Add(renderer.material);
        }
      } else {
        materials.Add(targetRender.material);
      }
        callAction();
    }
    public override TaskStatus OnUpdate() {
      return TaskStatus.Success;
    }
    private void callAction() {
      foreach (Material material in materials) {
        Tweener tweener = null;
        switch (action) {
          case ACTION_NAME.SET_MATERIAL_KEYWORD:
            LocalKeyword keyword = new LocalKeyword(material.shader, targetString);
            material.SetKeyword(keyword, targetBoolean);
            break;
          case ACTION_NAME.SET_MATERIAL_INTEGER:
            tweener = DOTween.To(() => material.GetInteger(targetString), x => material.SetInteger(targetString, x), (int)targetNumber, tweenerSetting.DurationValue);
            break;
          case ACTION_NAME.SET_MATERIAL_FLOAT:
            tweener = DOTween.To(() => material.GetFloat(targetString), x => material.SetFloat(targetString, x), targetNumber, tweenerSetting.DurationValue);
            break;
          case ACTION_NAME.SET_MATERIAL_COLOR:
            tweener = DOTween.To(() => material.GetColor(targetString), x => material.SetColor(targetString, x), targetColor, tweenerSetting.DurationValue);
            break;
          case ACTION_NAME.SET_MATERIAL_TEXTURE:
            material.SetTexture(targetString, targetTexture);
            break;
        }

        if (tweener != null) {
          tweener.SetDelay(tweenerSetting.Delay)
            .SetLoops(tweenerSetting.LoopCycle, tweenerSetting.LoopType)
            .SetEase(tweenerSetting.EaseType);
        }
      }
    }
  }
}
