using System;
using UnityEngine;

namespace Halabang.Audio {
  public class  Song {
    public string ID { get; set; }
    public string Name { get; set; }
    public AudioSource Source { get; set; }
  }

  [Serializable]
  public class UI_SFX {
    public AudioDictionary.UI_SFX_THEME Theme;
    public AudioTrigger ButtonHover;
    public AudioTrigger ButtonClick;
  }

  [Serializable]
  public class AudioDictionary {
    public enum AUDIO_TYPE { NULL, VOICE, BGM, SFX, AMBIENCE }
    public enum MIXER_NAME {
      MASTER_VOLUME,
      MUSIC_VOLUME,
      SFX_VOLUME,
      VOICE_VOLUME
    }
    public enum UI_SFX_THEME { NULL, DEFAULT } //DO NOT REORDER
    public enum UI_SFX_ACTION { NULL, HOVER, CLICK }
  }
}
