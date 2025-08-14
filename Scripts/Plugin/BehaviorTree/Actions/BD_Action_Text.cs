using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using Halabang.Plugin;
using UnityEngine.UI;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("")]
  public class BD_Action_Text : Action {
    public enum ACTION_NAME {
      NULL,
      SHOW_TEXT,
      HIDE_TEXT,
      SET_TEXT_COLOR
    }

    public ACTION_NAME triggerAction;
    public TextMeshExtend targetText;
    public string value;
    public Color targetColor;
    public TweenSetting tweenSettings = new TweenSetting();

    public override void OnStart() {
      callAction();
    }
    public override TaskStatus OnUpdate() {
      return TaskStatus.Success;
    }

    private void callAction() {
      switch (triggerAction) {
        case ACTION_NAME.SHOW_TEXT:
          //此处 Duration 指的是 text 显示完成后等待 x 秒后隐藏
          targetText.SetText(value);
          break;
        case ACTION_NAME.HIDE_TEXT:
          targetText.HideText();
          break;
        case ACTION_NAME.SET_TEXT_COLOR:
          targetText.SetColor(targetColor, tweenSettings);
          break;
      }    
    }
  }
}