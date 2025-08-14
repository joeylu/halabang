using Halabang.UI;
using UnityEngine;

namespace Halabang.Story {
  public class Copywriting {
    public GameSettings.GLOBAL_SETTING_LANGUAGE Language { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Brief { get; set; }
    public string Content { get; set; }
    public UI_Icons_Setting Icons { get; set; }
    public UI_Images_Setting Images { get; set; }
    public string VoiceID { get; set; }
    public Sprite DefaultIcon { get; set; } //in case Icons are empty, pass a default icon sprite as parameter at runtime
    public Sprite DefaultImaage { get; set; } //in case Images are empty, pass a default image sprite as parameter at runtime
    public UI_DisplaySets DisplaySet => getDisplaySet();
    private UI_DisplaySets displaySet;

    public Copywriting() { }
    public Copywriting(CopywritingPreset preset) {
      Language = (GameSettings.GLOBAL_SETTING_LANGUAGE)preset.Language;
      Title = preset.Title;
      Subtitle = preset.Subtitle;
      Brief = preset.Brief;
      Content = preset.Content;
      Icons = preset.Icons;
      Images = preset.Images;
      VoiceID = preset.Voice;
    }

    private UI_DisplaySets getDisplaySet() {
      if (displaySet != null) return displaySet;

      displaySet = new UI_DisplaySets();
      displaySet.SetIcons(Icons);
      displaySet.SetImages(Images);

      return displaySet;
    }
  }
}