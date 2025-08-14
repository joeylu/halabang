using Halabang.Plugin;
using UnityEngine;
using UnityEngine.Rendering;
using Halabang.Character;
using UnityEngine.Events;
using System.Collections;
using Halabang.Scene;
using DG.Tweening;

namespace Halabang.Game {
  public class SceneManager : MonoBehaviour {
    //public DebuggerManager _DebuggerManager { get; private set; }
    public CharacterPlayer CurrentPlayer { get; private set; }
    public Volume CurrentVolume { get; set; }
    public bool IsPrerequisiteLoaded { get; private set; }
    public bool IsLoaded { get; private set; }
    public CameraManager CurrentCameraManager { get; private set; }
    public CharacterManager CurrentCharaterManager { get; private set; }

    [Header("默认设置")]
    [SerializeField] private CharacterPlayer player;
    [SerializeField] private Volume defaultVolume;
    [Header("事件")]
    [Tooltip("当场景加载完毕")]
    [SerializeField] private UnityEvent onLoaded;
    [Tooltip("当场景在本次游戏中第一次加载")]
    [SerializeField] private UnityEvent onFirstLoaded;

    [Header("开发者选项")]
    [Tooltip("当作第一次加载")]
    [SerializeField] private bool asFirstLoaded;

    private void Awake() {
      GameManager.Instance.RegisterSceneManager(this);

      //_DebuggerManager = GetComponentInChildren<DebuggerManager>();

      if (defaultVolume == null) Debug.LogError("场景管理器必须指定一个默认后期Volume");
      //if (player == null) Debug.LogError("场景管理器必须指定一个默认玩家");

      CurrentVolume = defaultVolume;
      CurrentCameraManager = GetComponentInChildren<CameraManager>();
      CurrentCharaterManager = GetComponentInChildren<CharacterManager>();
      CurrentPlayer = player;
    }
    private IEnumerator Start() {
      IsPrerequisiteLoaded = true;

      yield return new WaitUntil(() => GameManager.Instance);
      yield return new WaitUntil(() => GameManager.Instance._SaveLoadManager);
      yield return new WaitUntil(() => GameManager.Instance._SaveLoadManager.IsLoading == false);

      if (asFirstLoaded) {
        onFirstLoaded.Invoke();
      } else {
        onLoaded.Invoke();
      }
      IsLoaded = true;
    }


    public void ResumeDefaultVolume(TweenSetting tweenSetting) {
      DOTween.To(() => GameManager.Instance.CurrentSceneManager.CurrentVolume.weight, value => GameManager.Instance.CurrentSceneManager.CurrentVolume.weight = value, 0f, tweenSetting.Duration)
        .OnComplete(() => GameManager.Instance.CurrentSceneManager.CurrentVolume = defaultVolume);
      DOTween.To(() => defaultVolume.weight, value => defaultVolume.weight = value, 1f, tweenSetting.Duration);
    }
  }
}
