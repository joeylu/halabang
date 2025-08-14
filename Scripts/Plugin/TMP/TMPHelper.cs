using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;
using UnityEngine;
using Unity.VisualScripting;
using Halabang.Utilities;
using Halabang.UI;
using TMPro;

namespace Halabang.Plugin {
  public class TMPHelper {
    public static IEnumerable<string> FontStyleTags(FontStyles styles) {
      //Debug.Log(styles + " has normal " + styles.HasFlag(FontStyles.Normal));
      //if (styles.HasFlag(FontStyles.Normal)) return null; //seems normal will always be true
      List<string> styleTags = new List<string>();
      if (styles.HasFlag(FontStyles.Bold)) styleTags.Add("b");
      if (styles.HasFlag(FontStyles.Italic)) styleTags.Add("i");
      if (styles.HasFlag(FontStyles.Underline)) styleTags.Add("u");
      if (styles.HasFlag(FontStyles.LowerCase)) styleTags.Add("lowercase");
      if (styles.HasFlag(FontStyles.UpperCase)) styleTags.Add("uppercase");
      if (styles.HasFlag(FontStyles.SmallCaps)) styleTags.Add("smallcaps");
      if (styles.HasFlag(FontStyles.Strikethrough)) styleTags.Add("s");
      if (styles.HasFlag(FontStyles.Superscript)) styleTags.Add("sup");
      if (styles.HasFlag(FontStyles.Subscript)) styleTags.Add("sub");

      return styleTags;
    }
  }
  public static class TMP_Ext {
    //public static string ColorTag(this string str, UI_Styling_Dictionary.COLOR_TYPE color) {
    //  str = str.TagWith(TMPDictionary.TextStylingTags.COLOR.Description(), "#" + color.GetColor().ToHexString());
    //  //Debug.Log(str + " : " + color.GetColor().ToHexString().Substring(0, 6));
    //  return str;
    //}
    //public static string StyleTag(this string str, FontStyles styles) {
    //  IEnumerable<string> styleTags = TMPHelper.FontStyleTags(styles);
    //  if (styleTags == null || styleTags.Count() == 0) return str;

    //  foreach (string tag in styleTags) {
    //    str = str.TagWith(tag);
    //  }
    //  return str;
    //}
    public static string FormatWith(this string str, TMPDictionary.TextFormatter formatter) {
      switch (formatter) {
        case TMPDictionary.TextFormatter.PARENTHESES:
          return $"({str})";
        case TMPDictionary.TextFormatter.BRACKETS:
          return $"[{str}]";
        case TMPDictionary.TextFormatter.BRACES:
          return string.Format("[{0}]", str);
        case TMPDictionary.TextFormatter.ANGLE:
          return $"<{str}>";
        default:
          return str;
      }
    }
    /// <summary>
    /// Extend the text mesh component set text method to accept a string as the only one parameter
    /// userful when passing a callback action
    /// </summary>
    /// <param name="targetTextComponent"></param>
    /// <param name="text"></param>
    public static void SetText(this TextMeshProUGUI targetTextComponent, string text) {
      targetTextComponent.SetText(text);
    }
  }

  public class TMPDictionary {
    public enum TextStylingTags {
      [Description("")] NULL,
      [Description("color")] COLOR
    }
    public enum TextFormatter {
      DEFAULT, 
      PARENTHESES, //()
      BRACKETS, //[]
      BRACES, //{}
      ANGLE //<>
    }
    //master.Name = string.Format("[{0}]", entity.Name);
    //master.Name = $"[{entity.Name}]";
    [Serializable]
    public struct SpriteAssetValue {
      public TMP_SpriteAsset SpriteAsset;
      public int AssetIndex;
    }
  }
}
