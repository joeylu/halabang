using Halabang.Game;
using Halabang.Story;
using UnityEngine;

namespace Halabang.Character {
  [RequireComponent(typeof(CharacterAnimator))]
  [RequireComponent(typeof(CharacterGround2D))]
  public class CharacterBase : MonoBehaviour {
    public enum CHARACTER_ACTION_STATE {
      Null, Ground
    }
    public enum CHARACTER_BEHAVIOR_STATE {
      Neutral, Positive, Negative
    }

    public CharacterAnimator _CharacterAnimator { get; private set; }
    public CharacterGround2D _CharacterGround2D { get; private set; }
    public CharacterEmotion _CharacterEmotion { get; private set; }
    public CharacterAI _CharacterAI { get; private set; }
    public CharacterUI _CharacterUI { get; private set; }
    public CHARACTER_ACTION_STATE CurrentActionState { get; private set; }
    public CHARACTER_BEHAVIOR_STATE CurrentBehaviorState { get; private set; }
    public Actor CurrentActor => getActor();
    public bool IsCharacter2D { get; private set; }

    [Header("基础设定")]
    [Tooltip("角色内容设定")]
    [SerializeField] private ActorPreset actorPreset;

    private Actor currentActor;

    private void Awake() {
      //基础组件检查
      _CharacterAnimator = GetComponent<CharacterAnimator>();
      if (_CharacterAnimator == null) Debug.LogError(name + " 必须指定一个Character Animator");

      getActor();

      //角色能力组件初始化
      getCharacterAbility();

      //非必须组件
      _CharacterEmotion = GetComponent<CharacterEmotion>();
      _CharacterAI = GetComponent<CharacterAI>();
      _CharacterUI = GetComponent<CharacterUI>();
    }
    private void Start() {


      GameManager.Instance.CurrentSceneManager.CurrentCharaterManager.RegisterCharacter(this);
    }

    public void ChangeActionState(CHARACTER_ACTION_STATE actionState) {
      CurrentActionState = actionState;
      changeStates();
    }
    public void ChangeEmotionState() {
      changeStates();
    }

    private Actor getActor() {
      if (currentActor != null) return currentActor;
      if (actorPreset) currentActor = new Actor(actorPreset);

      return currentActor;
    }
    private void changeStates() {
      if (_CharacterEmotion && 
        (_CharacterEmotion.CurrentState == CharacterEmotion.EMOTION_STATE.Happy ||
        _CharacterEmotion.CurrentState == CharacterEmotion.EMOTION_STATE.Trust ||
        _CharacterEmotion.CurrentState == CharacterEmotion.EMOTION_STATE.Suprise ||
        _CharacterEmotion.CurrentState == CharacterEmotion.EMOTION_STATE.Expect ||
        _CharacterEmotion.CurrentState == CharacterEmotion.EMOTION_STATE.Calm
        )) {
        //如果表情是积极的，则设置行为状态为积极
        CurrentBehaviorState = CHARACTER_BEHAVIOR_STATE.Positive;
      } else if (_CharacterEmotion &&
        (_CharacterEmotion.CurrentState == CharacterEmotion.EMOTION_STATE.Fear ||
        _CharacterEmotion.CurrentState == CharacterEmotion.EMOTION_STATE.Doubt ||
        _CharacterEmotion.CurrentState == CharacterEmotion.EMOTION_STATE.Disgust ||
        _CharacterEmotion.CurrentState == CharacterEmotion.EMOTION_STATE.Sad ||
        _CharacterEmotion.CurrentState == CharacterEmotion.EMOTION_STATE.Anger
        )) {
        //如果表情是消极的，则设置行为状态为消极
        CurrentBehaviorState = CHARACTER_BEHAVIOR_STATE.Negative;
      } else {
        //如果表情是中性的，则设置行为状态为积极
        CurrentBehaviorState = CHARACTER_BEHAVIOR_STATE.Positive;
      }
      //执行更新
      _CharacterAnimator.ChangeAnimator();
    }
    private void getCharacterAbility() {
      _CharacterGround2D = GetComponent<CharacterGround2D>();

      IsCharacter2D = _CharacterGround2D;
    }
  }
}