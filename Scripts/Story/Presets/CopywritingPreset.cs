using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Halabang.Editor;
using Halabang.UI;

namespace Halabang.Story {
  [CreateAssetMenu(fileName = "copywriting_", menuName = "Geminum/Story/Copywriting Preset")]
  public class CopywritingPreset : ScriptableObject {
    public int Language => (int)language;
    public string Title => title;
    public string Subtitle => subtitle;
    public string Brief => brief;
    public string Content => content;
    public UI_Icons_Setting Icons => icons;
    public UI_Images_Setting Images => images;
    public string Voice => voice;

    [Helpbox("Set addressable with caution, copywriting needs to find its proper group and has its own naming pattern !!!", HelpboxAttribute.MessageType.Warning)]
    [ReadOnly]
    [SerializeField] private string warning;

    [SerializeField] private GameSettings.GLOBAL_SETTING_LANGUAGE language;
    [SerializeField] private string contributor;

    [Header("Document")]
    [SerializeField] private string title;
    [SerializeField] private string subtitle;
    [TextArea(3,5)]
    [SerializeField] private string brief;
    [TextArea(10, 50)]
    [SerializeField] private string content;
    [SerializeField] private UI_Icons_Setting icons;
    [SerializeField] private UI_Images_Setting images;
    [SerializeField] private string voice;
  }
}
