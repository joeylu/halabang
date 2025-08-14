using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using Halabang.Utilities;
using Halabang.Audio;
using Newtonsoft.Json;
using Halabang.Game;

namespace Halabang.Game {
  public class PersistentGameSettings : MonoBehaviour {
    #region PROPERTIES
    private const string fileName = "game_settings";
    public GamePreferences CurrentGameSetting { get; set; }
    public bool IsLoaded { get; private set; }
    #endregion
    #region SERIALIZED_FIELDS
    [Header("Default gameplay settings")]
    [SerializeField] private GameSettings.GLOBAL_SETTING_LANGUAGE defaultLanguage = GameSettings.GLOBAL_SETTING_LANGUAGE.ENGLISH;
    [SerializeField] private bool defaultSubtitleEnabled = true;
    [Header("Default visual settings")]
    [SerializeField] private GameSettings.GLOBAL_SETTING_RESOLUTION defaultResolution = GameSettings.GLOBAL_SETTING_RESOLUTION.P1080_60;
    [SerializeField] private FullScreenMode defaultScreenMode = FullScreenMode.ExclusiveFullScreen;
    [Range(0, 1)][SerializeField] private float defaultBrightness = 0.5f;
    [SerializeField] private bool defaultVsyncEnable = true;
    [Header("Default audio settings")]
    [Range(0.01f, 1f)][SerializeField] private float defaultMixerMaster = 1f;
    [Range(0.01f, 1f)][SerializeField] private float defaultMixerMusic = 1f;
    [Range(0.01f, 1f)][SerializeField] private float defaultMixerSFX = 1f;
    [Range(0.01f, 1f)][SerializeField] private float defaultMixerVoice = 1f;

    [HideInInspector] public UnityEvent OnLanguageChange;
    #endregion
    #region LOCAL_FIELDS
    #endregion
    #region UNITY_METHODS
    private void Awake() {
      //construct defaults for other components such as UI Helper, etc to read, but not apply any settings, let the load setting to handle
      CurrentGameSetting = defaultPreferences();
    }
    #endregion
    #region LOAD_SAVE_METHODS
    public void LoadSetting() {
      GamePreferences preferences = null;
      //load preference from saved file if any
      try {
        string filePath = Path.Combine(Application.persistentDataPath, GameSettings.SAVE_FILE_PATH, fileName);
        //first, try to load the given slot if any
        if (GenericUtilities.FileExists(filePath)) {
          string jsonResult = GenericUtilities.LoadFromFile(filePath);
          if (string.IsNullOrWhiteSpace(jsonResult) == false) {
            preferences = JsonConvert.DeserializeObject<GamePreferences>(jsonResult);
          }
        }
      } catch (Exception ex) {
        Debug.Log("Unexpected error: loading game setting preferences has failed" + Environment.NewLine + ex.Message);
      }
      if (preferences == null) preferences = defaultPreferences();
      ApplySetting(preferences);
      IsLoaded = true;
    }
    public void SaveSetting() {
      string serializedData = JsonConvert.SerializeObject(CurrentGameSetting, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
      try {
        string filePath = Path.Combine(Application.persistentDataPath, GameSettings.SAVE_FILE_PATH, fileName);
        GenericUtilities.SaveToFile(filePath, serializedData);
      } catch (Exception ex) {
        Debug.Log("Unexpected error: saving game setting preferences has failed" + Environment.NewLine + ex.Message);
      }
    }
    private GamePreferences defaultPreferences() {
      GamePreferences setting = new GamePreferences();
      setting.DefaultLanguage = defaultLanguage;
      setting.CurrentLanguage = defaultLanguage;
      setting.EnableSubtitle = defaultSubtitleEnabled;
      setting.ScreenMode = defaultScreenMode;
      setting.Resolution = defaultResolution;
      setting.Brightness = defaultBrightness;
      setting.EnableVsync = defaultVsyncEnable;
      setting.MixerMasterVolume = defaultMixerMaster;
      setting.MixerMusicVolume = defaultMixerMusic;
      setting.MixerSFXVolume = defaultMixerSFX;
      setting.MixerVoiceVolume = defaultMixerVoice;
      return setting;
    }
    #endregion
    #region RUNTIME_SETTER
    public void ApplySetting(GamePreferences preference) {
      //gameplay
      SetLanguage(preference.CurrentLanguage);
      //GameManager.instatnce.DialogueBasic.EnableSubtitle(preference.);
      //visual
      SetScreenMode(preference.ScreenMode);
      SetResolution(preference.Resolution);
      SetBrightness(preference.Brightness);
      SetShadow(preference.Shadow);
      SetVsync(preference.EnableVsync);
      //audio
      //GameManager.instatnce.CurrentAudioManager.SetMixerVolume(AudioDictionary.MIXER_NAME.MASTER_VOLUME, preference.MixerMasterVolume);
      //GameManager.instatnce.CurrentAudioManager.SetMixerVolume(AudioDictionary.MIXER_NAME.MUSIC_VOLUME, preference.MixerMusicVolume);
      //GameManager.instatnce.CurrentAudioManager.SetMixerVolume(AudioDictionary.MIXER_NAME.SFX_VOLUME, preference.MixerSFXVolume);
      //GameManager.instatnce.CurrentAudioManager.SetMixerVolume(AudioDictionary.MIXER_NAME.VOICE_VOLUME, preference.MixerVoiceVolume);
    }
    public void SetLanguage(int languageIndex) {
      if (languageIndex < 0 || languageIndex > System.Enum.GetValues(typeof(GameSettings.GLOBAL_SETTING_LANGUAGE)).Length) {
        Debug.LogError("Unexpected error: invalid index to get language enum value: " + languageIndex);
        return;
      }
      SetLanguage((GameSettings.GLOBAL_SETTING_LANGUAGE)languageIndex);
    }
    public void SetLanguage(GameSettings.GLOBAL_SETTING_LANGUAGE language) {
      if (CurrentGameSetting.CurrentLanguage != language) {
        CurrentGameSetting.CurrentLanguage = language;
        SaveSetting();
        OnLanguageChange.Invoke();
      }
    }
    public void SetSubtitleEnable(bool enable) {
      CurrentGameSetting.EnableSubtitle = enable;
      SaveSetting();
      Debug.Log("TO-DO: switch subtitle enable in basic dialogue controller");
    }
    public void SetAudioMixerVolume(AudioDictionary.MIXER_NAME mixer, float vol) {
      //GameManager.Instance.CurrentAudioManager.SetMixerVolume(mixer, vol);
      //switch (mixer) {
      //  case AudioDictionary.MIXER_NAME.MASTER_VOLUME:
      //    CurrentGameSetting.MixerMasterVolume = vol;
      //    break;
      //  case AudioDictionary.MIXER_NAME.MUSIC_VOLUME:
      //    CurrentGameSetting.MixerMusicVolume = vol;
      //    break;
      //  case AudioDictionary.MIXER_NAME.SFX_VOLUME:
      //    CurrentGameSetting.MixerSFXVolume = vol;
      //    break;
      //  case AudioDictionary.MIXER_NAME.VOICE_VOLUME:
      //    CurrentGameSetting.MixerVoiceVolume = vol;
      //    break;
      //}
      //SaveSetting();
    }
    public void SetResolution(int resolution) {
      if (Enum.GetValues(typeof(GameSettings.GLOBAL_SETTING_RESOLUTION)).Length <= resolution) {
        Debug.LogError("Unexpected error, the given resolution index " + resolution + " is out of GameSettings.GLOBAL_SETTING_RESOLUTION range");
        return;
      }

      SetResolution((GameSettings.GLOBAL_SETTING_RESOLUTION)resolution);
    }
    public void SetResolution(GameSettings.GLOBAL_SETTING_RESOLUTION resolution) {
      if (CurrentGameSetting.Resolution != resolution) {
        switch (resolution) {
          case GameSettings.GLOBAL_SETTING_RESOLUTION.P720_30:
            //RefreshRate refreshRate = new RefreshRate();
            Screen.SetResolution(1280, 720, (FullScreenMode)CurrentGameSetting.ScreenMode, new RefreshRate() { numerator = 30, denominator = 1 });
            break;
          case GameSettings.GLOBAL_SETTING_RESOLUTION.P720_60:
            Screen.SetResolution(1280, 720, (FullScreenMode)CurrentGameSetting.ScreenMode, new RefreshRate() { numerator = 60, denominator = 1 });
            break;
          case GameSettings.GLOBAL_SETTING_RESOLUTION.P1080_30:
            Screen.SetResolution(1920, 1080, (FullScreenMode)CurrentGameSetting.ScreenMode, new RefreshRate() { numerator = 30, denominator = 1 });
            break;
          case GameSettings.GLOBAL_SETTING_RESOLUTION.P1080_60:
            Screen.SetResolution(1920, 1080, (FullScreenMode)CurrentGameSetting.ScreenMode, new RefreshRate() { numerator = 60, denominator = 1 });
            break;
          case GameSettings.GLOBAL_SETTING_RESOLUTION.P2K_30:
            Screen.SetResolution(3840, 2160, (FullScreenMode)CurrentGameSetting.ScreenMode, new RefreshRate() { numerator = 30, denominator = 1 });
            break;
          case GameSettings.GLOBAL_SETTING_RESOLUTION.P2K_60:
            Screen.SetResolution(3840, 2160, (FullScreenMode)CurrentGameSetting.ScreenMode, new RefreshRate() { numerator = 60, denominator = 1 });
            break;
          default:
            Screen.SetResolution(1920, 1080, (FullScreenMode)CurrentGameSetting.ScreenMode, new RefreshRate() { numerator = 60, denominator = 1 });
            break;
        }
        CurrentGameSetting.Resolution = resolution;
        SaveSetting();
      }
    }
    public void SetScreenMode(int screenMode) {
      if (Enum.GetValues(typeof(FullScreenMode)).Length <= screenMode) {
        Debug.LogError("Unexpected error, the given screen mode index " + screenMode + " is out of FullScreenMode range");
        return;
      }
      SetScreenMode((FullScreenMode)screenMode);
    }
    public void SetScreenMode(FullScreenMode screenMode) {
      Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
      if (CurrentGameSetting.ScreenMode != screenMode) {
        //Screen.fullScreenMode = screenMode;
        CurrentGameSetting.ScreenMode = screenMode;
        SaveSetting();
      }
    }
    public void SetBrightness(float value) {
      if (Mathf.Approximately(CurrentGameSetting.Brightness, value) == false) {
        Screen.brightness = value;
        //Debug.Log("setting screen brightness to " + Screen.brightness + " : " + value);
        CurrentGameSetting.Brightness = value;
        SaveSetting();
      }
    }
    public void SetShadow(int shadow) {
      if (Enum.GetValues(typeof(GameSettings.GLOBAL_SETTING_SHADOW)).Length <= shadow) {
        Debug.LogError("Unexpected error, the given shadow index " + shadow + " is out of GameSettings.GLOBAL_SETTING_SHADOW range");
        return;
      }

      SetShadow((GameSettings.GLOBAL_SETTING_SHADOW)shadow);
    }
    public void SetShadow(GameSettings.GLOBAL_SETTING_SHADOW shadow) {
      if (CurrentGameSetting.Shadow != shadow) {
        CurrentGameSetting.Shadow = shadow;
        SaveSetting();
      }
    }
    public void SetVsync(bool enable) {
      if (CurrentGameSetting.EnableVsync != enable) {
        QualitySettings.vSyncCount = enable ? 1 : 0;
        CurrentGameSetting.EnableVsync = enable;
        SaveSetting();
      }
    }
    public void SetMasterVolume() {
      //AudioSettings.m
      SaveSetting();
    }
    #endregion

#if UNITY_EDITOR
    //[Header("In Editor development only")]
    //[Tooltip("Mimic the apply action in setting panel from main menu")]
    //[SerializeField] private bool updateSettingInPlayMode;

    //private void OnValidate() {
    //  if (updateSettingInPlayMode) {
    //    updateSettingInPlayMode = false;

    //    GamePreferences preferences = CurrentGameSetting.CloneShallowCopy();
    //    preferences.CurrentLanguage = defaultLanguage;
    //    preferences.Brightness = defaultBrightness;
    //    preferences.EnableVsync = defaultVsyncEnable;
    //    ApplySetting(preferences);
    //  }
    //}
#endif
  }
}
