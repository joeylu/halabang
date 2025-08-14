using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;
using Halabang.Plugin;
//using Sonity;
//using Halabang.Scenes;

namespace Halabang.Audio {
  public class AudioRequestor {
    //public AsyncOperationHandle<SoundEvent> AddressableLoader { get; set; }
    //public SonityParameters ModifierValues { get; set; }
    public string AddressableName { get; set; }
    public AudioDictionary.AUDIO_TYPE AudioType { get; set; }
    public Transform Owner { get; set; }
    //public SoundEvent TargetSoundEvent { get; private set; }
    public Action CallbackOnLoaded { get; set; } //callback on audio is loaded and sound event is ready
    //runtime values
    public bool Is2D { get { return Owner == null; } }
    public float RemainingPlayableTime => getRemainingTime();
    public bool IsPaused => isPaused();
    public bool IsPlaying => isPlaying();
    public bool IsLoop => isLoop();

    /// <summary>
    /// Invoked when AddressableLoader is loaded and completed
    /// </summary>
    /// <param name="handler"></param>
    //public void OnCompleted(AsyncOperationHandle<SoundEvent> handler) {
    //  TargetSoundEvent = handler.Result;
    //  play();
    //  //Debug.Log(TargetSoundEvent.internals.data.soundMix.internals.soundEventModifier.volumeDecibel);
    //  if (CallbackOnLoaded != null) CallbackOnLoaded.Invoke();
    //}
    //public void Unload() {
    //  if (AddressableLoader.IsValid()) Addressables.Release(AddressableLoader);

    //  if (TargetSoundEvent != null) {
    //    TargetSoundEvent.UnloadAudioData();
    //    TargetSoundEvent = null;
    //  }
    //}
    public void Play() {
      play();
    }
    public void Pause() {
      pause();
    }
    public void Resume() {
      if (isPaused() == false) return;

      play();
    }

    private void play() {
      //if (AudioType == AudioDictionary.AUDIO_TYPE.MUSIC) {
      //  if (ModifierValues == null) {
      //    TargetSoundEvent.PlayMusic(false, true);
      //  } else {
      //    TargetSoundEvent.PlayMusic(false, true, ModifierValues.GetParameters());
      //  }
      //} else {
      //  if (Is2D) {
      //    if (ModifierValues == null) {
      //      TargetSoundEvent.Play2D();
      //    } else {
      //      TargetSoundEvent.Play2D(ModifierValues.GetParameters());
      //    }
      //  } else {
      //    if (ModifierValues == null) {
      //      TargetSoundEvent.Play(Owner);
      //    } else {
      //      TargetSoundEvent.Play(Owner, ModifierValues.GetParameters());
      //    }
      //  }
      //}
    }
    private void pause() {
      //if (isPlaying() == false) return;
      //if (Is2D) {
      //  TargetSoundEvent.Pause2D();
      //} else {
      //  TargetSoundEvent.Pause(Owner);
      //}
    }
    private bool isPlaying() {
      //if (TargetSoundEvent == null) return false;

      //switch (AudioType) {
      //  case AudioDictionary.AUDIO_TYPE.MUSIC:
      //    return TargetSoundEvent.GetMusicSoundEventState() != SoundEventState.NotPlaying;
      //  case AudioDictionary.AUDIO_TYPE.SFX:
      //  case AudioDictionary.AUDIO_TYPE.VOICE:
      //    if (Is2D) {
      //      return TargetSoundEvent.Get2DSoundEventState() != SoundEventState.NotPlaying;
      //    } else {
      //      return TargetSoundEvent.GetSoundEventState(Owner) != SoundEventState.NotPlaying;
      //    }
      //  default:
      //    return false;
      //}

      return false;
    }
    private bool isPaused() {
      //if (TargetSoundEvent == null) return false;

      //switch (AudioType) {
      //  case AudioDictionary.AUDIO_TYPE.MUSIC:
      //    return TargetSoundEvent.GetMusicSoundEventState() == SoundEventState.Paused;
      //  case AudioDictionary.AUDIO_TYPE.SFX:
      //  case AudioDictionary.AUDIO_TYPE.VOICE:
      //    if (Is2D) {
      //      return TargetSoundEvent.Get2DSoundEventState() == SoundEventState.Paused;
      //    } else {
      //      return TargetSoundEvent.GetSoundEventState(Owner) == SoundEventState.Paused;
      //    }
      //  default:
      //    return false;
      //}

      return false;
    }
    private bool isLoop() {
      //Debug.Log(AddressableName + " : " + TargetSoundEvent.internals.soundContainers.First().internals.data.loopEnabled);
      //return TargetSoundEvent.internals.soundContainers.Any(r => r.internals.data.loopEnabled);
      return false;
    }
    private float getRemainingTime() {
      //TO-DO: consider loop audio? find a way to determine and calculate

      //if (TargetSoundEvent == null) return 0;
      //if (IsPlaying == false) return 0;

      //if (Is2D) {
      //  return TargetSoundEvent.Get2DLastPlayedClipLength(true) - TargetSoundEvent.Get2DLastPlayedClipTimeSeconds(true);
      //} else {
      //  return TargetSoundEvent.GetLastPlayedClipLength(Owner, true) - TargetSoundEvent.GetLastPlayedClipTimeSeconds(Owner, true);
      //}

      return 0;
    }
  }

  [Serializable]
  public struct AudioRequestorSetting {
    [Header("Basic settings")]
    [Tooltip("When owner is empty, this audio is considered as 2D sound")]
    public Transform Owner;
    public string AddressableName;
    public AudioDictionary.AUDIO_TYPE AudioType;
    //[Tooltip("OnAudioCompletelyPlayed event can only be triggered If this sound event is not a loop")]
    //public bool Loop;
  }
  [Serializable]
  public struct AudioTriggerEvents {
    [Tooltip("When this audio loaded (completed) from addressable")]
    public UnityEvent<AudioTrigger> OnAudioLoaded;
  }
  [Serializable]
  public struct AudioOnHealthChange {
    [Tooltip("Play audio while property is healthy")]
    public AudioTrigger OnHealthy;
    [Tooltip("Play audio while property is healthy")]
    public AudioTrigger OnDamaged;
    [Tooltip("Play audio while property is healthy")]
    public AudioTrigger OnInjuried;
    [Tooltip("No loop, Play audio when property reaches perfect state")]
    public AudioTrigger OnPerfected;
  }


  [Serializable]
  public class AudioSettingOnStaminaStates {
    //TO-DO: use editor to either assign an audio trigger or audio name
    [SerializeField] public AudioTrigger onNormalIdle;
    [SerializeField] public AudioTrigger onTiredIdle;
    [SerializeField] public AudioTrigger onExhaustedIdle;
    [SerializeField] public AudioTrigger onNormalWalking;
    [SerializeField] public AudioTrigger onTiredWalking;
    [SerializeField] public AudioTrigger onExhaustedWalking;
    [SerializeField] public AudioTrigger onNormalRunning;
    [SerializeField] public AudioTrigger onTiredRunning;
    [SerializeField] public AudioTrigger onExhaustedRunning;
  }
}
