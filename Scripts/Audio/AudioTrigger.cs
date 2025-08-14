using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
//using Halabang.Scenes;
using Halabang.Plugin;
using Halabang.Utilities;
//using Sonity;
using DG.Tweening;
using Halabang.Game;
using Halabang.Plugin;
//using static Halabang.Scenes.DebugHelper;

namespace Halabang.Audio {
  public class AudioTrigger : MonoBehaviour {
    public string AudioName { get; private set; }
    public float AudioLength { get; private set; }
    public SoundManager.AUDIO_STATE AudioState { get; private set; }

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioDictionary.AUDIO_TYPE audioType;

    private float originalVolume;

    private void Awake() {
      if (source == null) source = GetComponent<AudioSource>();
      if (source == null) Debug.LogError(name + " 必须指定一个 audio source");

      AudioName = source.name;
      AudioLength = source.clip.length;
      AudioState = source.isPlaying ? SoundManager.AUDIO_STATE.STOPPED : SoundManager.AUDIO_STATE.PLAYING;

      originalVolume = source.volume;
    }
    private void Start() {
      switch (audioType) {
        case AudioDictionary.AUDIO_TYPE.VOICE:
          GameManager.Instance._SoundManager.RegisterVoiceTrigger(this);
          break;
      }
    }

    public void Play() {
      source.Play();
    }
    public void Stop() {
      source.Stop();
    }
    public void SetVolume(float volume, TweenSetting tweenSetting = null) {
      if (tweenSetting == null || tweenSetting.Duration <= 0) {
        source.volume = volume;
      } else {
        DOTween.To(() => source.volume, value => source.volume = value, volume, tweenSetting.Duration)
          .SetDelay(tweenSetting.Delay)
          .SetEase(tweenSetting.EaseType)
          .SetLoops(tweenSetting.LoopCycle, tweenSetting.LoopType);
      }
    }
    public void ResetVolume(TweenSetting tweenSetting = null) {
      if (tweenSetting == null || tweenSetting.Duration <= 0) {
        source.volume = originalVolume;
      } else {
        DOTween.To(() => source.volume, value => source.volume = value, originalVolume, tweenSetting.Duration)
          .SetDelay(tweenSetting.Delay)
          .SetEase(tweenSetting.EaseType)
          .SetLoops(tweenSetting.LoopCycle, tweenSetting.LoopType);
      }
    }
  }
}
