using UnityEngine;

namespace Halabang.Character {
  public class CharacterEmotion : MonoBehaviour {
    public enum EMOTION_STATE { Calm, Happy, Sad, Fear, Disgust, Anger, Suprise, Trust, Expect, Doubt }
    public EMOTION_STATE CurrentState { get; private set; }

    private CharacterBase character;

    private void Awake() {
      character = GetComponent<CharacterBase>();
    }
    private void Start() {
      //初始化人物状态
      character.ChangeEmotionState();
    }

    public void ChangeEmotion(EMOTION_STATE emotion) {
      Debug.Log(name + " > " + CurrentState + " : " + emotion);
      if (CurrentState == emotion) return; //如果情绪没有变化则不处理
      CurrentState = emotion;
      character.ChangeEmotionState();
    }
  }
}
