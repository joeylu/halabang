using UnityEngine;
using Michsky.MUIP;
using Halabang.Plugin;
using BehaviorDesigner.Runtime;

namespace Halabang.Audio {
  public class SoundPlayerController : MonoBehaviour {
    public float CurrentVolume { get; private set; } = 1; //当前播放的音量

    [Header("基础设定")]
    [SerializeField] private SoundListController soundListController;
    [SerializeField] private SliderManager volumeSlider;
    [SerializeField] private ButtonManagerExt playButton;
    [SerializeField] private BehaviorTree playAction;
    [SerializeField] private ButtonManagerExt pauseButton;//tony修改
    [SerializeField] private BehaviorTree pauseAction;//tony修改
    [SerializeField] private ButtonManagerExt stopButton;
    [SerializeField] private BehaviorTree stopAction;
    [SerializeField] private ButtonManagerExt nextButton;
    [SerializeField] private BehaviorTree nextAction;
    [SerializeField] private ButtonManagerExt prevButton;
    [SerializeField] private BehaviorTree prevAction;

    private void Awake() {
      soundListController.CurrentPlayer = this;
    }
    private void Start() {
      volumeSlider.sliderEvent.AddListener(changeVolume);
      playButton.onClick.AddListener(() => playAction.EnableBehavior());
      pauseButton.onClick.AddListener(() => pauseAction.EnableBehavior());//tony修改
      stopButton.onClick.AddListener(() => stopAction.EnableBehavior());//tony修改
      nextButton.onClick.AddListener(() => nextAction.EnableBehavior());
      prevButton.onClick.AddListener(() => prevAction.EnableBehavior());
    }

    private void changeVolume(float volume) {
      Debug.Log(name + " change vol " + volume);
      CurrentVolume = volume;
      soundListController.ChangeVolume();
    }
  }
}