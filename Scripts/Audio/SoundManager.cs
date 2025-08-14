using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Halabang.Plugin;
using DG.Tweening;

namespace Halabang.Audio {
  public class SoundManager : MonoBehaviour {
    public enum AUDIO_STATE { STOPPED, PLAYING, PAUSED }
    public AudioTrigger CurrentPlayingVoice { get; private set; }
    public AudioTrigger CurrentPlayingBGM { get; private set; } 
    public List<AudioTrigger> VoiceTriggers { get; private set; } = new List<AudioTrigger>();

    [SerializeField] private AudioTrigger defaultBGM;

    private void Start() {
      if (defaultBGM != null) PlayBGM(defaultBGM);
    }

    public void RegisterVoiceTrigger(AudioTrigger voiceTrigger) {
      if (voiceTrigger == null) return;
      VoiceTriggers.Add(voiceTrigger);
    }
    public void PlayMusic(string targetName, float delay) {

    }
    public void StopMusic(string targetName) {

    }
    public void PlayBGM(AudioTrigger bgmTrigger) {
      StopBGM();
      CurrentPlayingBGM = bgmTrigger;
      CurrentPlayingBGM.Play();
    }
    public void StopBGM() {
      if (CurrentPlayingBGM) CurrentPlayingBGM.Stop();
    }
    public void SetBGMVolume(float volume, TweenSetting tweenSetting = null) {
      CurrentPlayingBGM.SetVolume(volume, tweenSetting);
    }
    public void ResetBGMVolume(TweenSetting tweenSetting = null) {
      CurrentPlayingBGM.ResetVolume(tweenSetting);
    }

    public IEnumerator PlayVoiceSequence(string targetName) {
      AudioTrigger voiceTrigger = getAudioTrigger(targetName, AudioDictionary.AUDIO_TYPE.VOICE);
      if (voiceTrigger == null) yield break;

      voiceTrigger.Play();
      CurrentPlayingVoice = voiceTrigger;
      yield return new WaitForSeconds(voiceTrigger.AudioLength);

      CurrentPlayingVoice = null;
    }
    public void StopVoice(string targetName) { 
    
    }

    private AudioTrigger getAudioTrigger(string triggerName, AudioDictionary.AUDIO_TYPE audioType) {
      switch (audioType) {
        case AudioDictionary.AUDIO_TYPE.VOICE:
          return VoiceTriggers.Where(r => r.AudioName.Equals(triggerName, System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
      }

      return null;
    }
  }
}
