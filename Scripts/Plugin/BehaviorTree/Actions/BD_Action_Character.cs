using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using Halabang.Game;
using Halabang.Character;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("角色行为")]
  public class BD_Action_Character : Action {
    public enum ACTION_TYPE {
      Null, MoveToward, MoveStop
    }
    public enum UI_TYPE {
      Null, Bark
    }

    public ACTION_TYPE action;
    public UI_TYPE UI;
    public bool isCurrentPlayer;
    public CharacterBase targetCharacter;
    public Transform targetTransform;
    public string targetString;
    public float targetFloat;

    private CharacterBase baseCharacter;

    public override void OnStart() {
      if (isCurrentPlayer) {
        baseCharacter = GameManager.Instance.CurrentSceneManager.CurrentPlayer;
      } else {
        baseCharacter = targetCharacter;
      }

      callAction();
      callUI();
    }
    public override TaskStatus OnUpdate() {
      return TaskStatus.Success;
    }

    private void callAction() { 
      switch (action) {
        case ACTION_TYPE.MoveToward:
          if (baseCharacter.IsCharacter2D) {
            baseCharacter._CharacterGround2D.Move(targetTransform);
          }
          break;
        case ACTION_TYPE.MoveStop:
          if (baseCharacter.IsCharacter2D) {
            baseCharacter._CharacterGround2D.Stop();
          }
          break;
      }
    }
    private void callUI() {
      switch (UI) {
        case UI_TYPE.Bark:
          baseCharacter._CharacterUI.Bark(targetString, targetFloat);
          break;
      }
    }
  }
}
