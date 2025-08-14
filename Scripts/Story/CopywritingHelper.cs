using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using Halabang.Utilities;
using Halabang.Scene;
using Newtonsoft.Json;
using Halabang.Game;

namespace Halabang.Story {
  public class CopywritingHelper {
    #region CONSTANTS
    private const string COPYWRITING_JSON_DATA_FILE_PATH = "Assets/Contents/copywriting_data.json";
    #endregion
    #region RUNTIME_METHODS
    //public static string GetText(string key_string) {
    //  if (GameManager.instatnce.CurrentStoryManager.ConstantCopywritingInstance == null) return string.Empty;

    //  CopywritingData data = GameManager.instatnce.CurrentStoryManager.ConstantCopywritingInstance.Where(r => r.Key.Equals(key_string)).FirstOrDefault();
    //  if (data == null) return string.Empty;
    //  if (data.Localize == null) {
    //    Debug.LogError("Unexpected error: " + key_string + " has no localized data");
    //    return string.Empty;
    //  }
    //  LocalizeText localize = data.Localize.Where(r => r.Language == (int)GameManager.instatnce.CurrentPreferences.CurrentLanguage).FirstOrDefault();
    //  if (localize != null) {
    //    return localize.Value;
    //  } else {
    //    return data.Localize.First().Value;
    //  }
    //}
    //public static string MaskText(string rawText) {
    //  if (string.IsNullOrEmpty(rawText)) return string.Empty;

    //  StringBuilder maskedText = new StringBuilder();
    //  for (int i = 0; i < rawText.Length; i++) {
    //    maskedText.Append(GameManager.instatnce.CurrentStoryManager.DefaultMaskedLetter);
    //  }
    //  return maskedText.ToString();
    //}
    public static Copywriting GetCurrentCopywriting(CopywritingPreset[] copywritingPresets) {
      if (copywritingPresets == null || copywritingPresets.Length == 0) return null;
      CopywritingPreset defaultPreset = copywritingPresets.Where(r => r.Language == (int)GameManager.Instance._SaveLoadManager.CurrentGamePreference.CurrentGameSetting.DefaultLanguage).FirstOrDefault();
      CopywritingPreset matchedPreset = copywritingPresets.Where(r => r.Language == (int)GameManager.Instance._SaveLoadManager.CurrentGamePreference.CurrentGameSetting.CurrentLanguage).FirstOrDefault();
      //default copywriting is either the default language one or first in the array when not found
      Copywriting defaultCopywriting = defaultPreset == null ? new Copywriting(copywritingPresets.First()) : new Copywriting(defaultPreset);
      //matched copywriting is either the matched language one or default
      Copywriting matchedCopywriting = matchedPreset == null ? defaultCopywriting : new Copywriting(matchedPreset);

      return matchedCopywriting;
    }
    #endregion
    #region DATABASE
    //public static List<CopywritingData> LoadJson() {
    //  JsonData<List<CopywritingData>> jsonData = new JsonData<List<CopywritingData>>();
    //  jsonData.FilePath = COPYWRITING_JSON_DATA_FILE_PATH;
    //  List<CopywritingData> copywritingData = jsonData.Load(); //load from the local file and try to deserialize, a default story data will return if load or deserialize is failed
    //  return copywritingData;
    //}
    //public static List<CopywritingData> LoadJsonFromAsset() {
    //  JsonData<List<CopywritingData>> jsonData = new JsonData<List<CopywritingData>>();
    //  //jsonData.FilePath = COPYWRITING_JSON_DATA_FILE_PATH;
    //  //List<CopywritingData> copywritingData = jsonData.Load(); //load from the local file and try to deserialize, a default story data will return if load or deserialize is failed
    //  List<CopywritingData> copywritingData = JsonConvert.DeserializeObject<List<CopywritingData>>(GameManager.instatnce.CurrentStoryManager.CopywritingData.text);
    //  return copywritingData;
    //}
    #endregion
  }

  public class CopywritingData {
    public string Key { get; set; }
    public LocalizeText[] Localize { get; set; }
  }
  public class LocalizeText {
    public int Language { get; set; }
    public string Value { get; set; }
  }
}
