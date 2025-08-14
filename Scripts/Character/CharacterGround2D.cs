using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Halabang.Character {
  public class CharacterGround2D : MonoBehaviour {
    public enum MOVEMENT_STATE { Idle, Walking, Running }

        public bool IsMoving => CurrentState == MOVEMENT_STATE.Walking;
    public MOVEMENT_STATE CurrentState { get; private set; }

    [SerializeField] private float speed;

    private CharacterBase character;
    private Rigidbody2D _rigidbody2D;
    private Coroutine moveTransition;


    private void Awake() {
      character = GetComponent<CharacterBase>();
      _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void Start() {
      //初始化人物状态
      character.ChangeActionState(CharacterBase.CHARACTER_ACTION_STATE.Ground);
    }


    public void Move(Transform target) {
      Move(target.position);
    }
    public void Move(Vector2 target) {
      if (Vector2.Distance(transform.position, target) < 0.05f) return;

      if (moveTransition != null) StopCoroutine(moveTransition);
      moveTransition = StartCoroutine(moveSequence(target));
    }
    public void Stop() {
      if (moveTransition != null) StopCoroutine(moveTransition);
      //_rigidbody2D.linearVelocity = Vector2.zero;
      CurrentState = MOVEMENT_STATE.Idle;
      character.ChangeActionState(CharacterBase.CHARACTER_ACTION_STATE.Ground);
    }

    private IEnumerator moveSequence(Vector2 target) {
      Vector2 movingDirection = target - (Vector2)transform.position;

      CurrentState = MOVEMENT_STATE.Walking;
      character.ChangeActionState(CharacterBase.CHARACTER_ACTION_STATE.Ground);

      while (Vector2.Distance(transform.position, target) > 0.05f) {
        movingDirection = target - (Vector2)transform.position;
        character.transform.position = Vector2.MoveTowards(character.transform.position, target, speed * Time.deltaTime);
        //_rigidbody2D.linearVelocity = movingDirection * speed;
        yield return null;
      }

      Stop();
    }
  }
}
