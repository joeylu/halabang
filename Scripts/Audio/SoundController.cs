using Halabang.Game;
using System;
using UnityEngine;

namespace Halabang.Audio {
  public class SoundController : MonoBehaviour {
    public Song CurrentSong { get; set; }
    public SoundListController CurrentSoundListController { get; set; }

    [SerializeField] private AudioSource audioSource;


    private void Awake() {
      if (audioSource == null) Debug.LogError(name + " 必须指定一个 Audio Source");

      CurrentSong = new Song();
      CurrentSong.ID = Guid.NewGuid().ToString();
      CurrentSong.Name = "歌曲1";
      CurrentSong.Source = audioSource; ;
    }

    public void Play() {
      GameManager.Instance._SoundManager.StopBGM();
      CurrentSong.Source.Play();
      ChangeVolume();
    }
    public void Stop() {
      CurrentSong.Source.Stop();
    }
    public void Pause() {
      CurrentSong.Source.Pause();
    }
    public void ChangeVolume() {
      CurrentSong.Source.volume = CurrentSoundListController.CurrentPlayer.CurrentVolume;
    }
  }
}
