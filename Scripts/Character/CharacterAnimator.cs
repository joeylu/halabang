using UnityEngine;

namespace Halabang.Character {
  public class CharacterAnimator : MonoBehaviour {

    [SerializeField] private Animator targetAnimator;

    private CharacterBase character;
    private AnimatorControllerParameter[] parameters;

    private void Awake() {
      character = GetComponent<CharacterBase>();
      if (targetAnimator == null) Debug.LogError(name + " 必须指定一个动画控制器");

      parameters = targetAnimator.parameters;
    }

    public void ChangeAnimator() {
      //重置所有Character state参数及子级参数为 false
      resetAnimator();
      //更新当前角色的动作参数
      setActionState();
      //更新当前角色的行为参数
      setBehaviorState();
      //更新当前角色的表情参数
      setEmotionState();
    }

    private void setActionState() {
      //激活当前Character state参数为 true
      targetAnimator.SetBool(character.CurrentActionState.ToString(), true);
      //激活当前Character state的子级参数
      //Debug.Log(character + " : " + character.CurrentState + " > " + character._CharacterGround2D.CurrentState);
      switch (character.CurrentActionState) {
        case CharacterBase.CHARACTER_ACTION_STATE.Ground:
          targetAnimator.SetBool(character._CharacterGround2D.CurrentState.ToString(), true);
          break;
      }
    }
    private void setBehaviorState() {
      targetAnimator.SetBool(character.CurrentBehaviorState.ToString(), true);
    }
    private void setEmotionState() {
      //Debug.Log("changing emation to " + character._CharacterEmotion.CurrentState.ToString());
      targetAnimator.SetBool(character._CharacterEmotion.CurrentState.ToString(), true);
    }
    private void resetAnimator() {
      if (parameters == null) return;
      foreach (AnimatorControllerParameter parameter in parameters) { 
        switch(parameter.type) {
          case AnimatorControllerParameterType.Bool:
            targetAnimator.SetBool(parameter.name, false);
            break;
        }
      }
    }
  }
}