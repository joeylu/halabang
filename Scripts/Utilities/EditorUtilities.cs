using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Halabang.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Halabang.Editor {
#if UNITY_EDITOR
  public class EditorUtilities {
    /// <summary>
    /// If the given asset is not found, create one
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static T CreateAsset<T>(string path, string filename) {
      object itemAsset = LoadAsset<T>(path, filename);
      if (itemAsset == null) {
        ScriptableObject newItemAsset = ScriptableObject.CreateInstance(typeof(T));
        AssetDatabase.CreateAsset(newItemAsset, path + "/" + filename);
        //AssetDatabase.SaveAssets();
        itemAsset = newItemAsset;
      }
      return (T)itemAsset;
    }
    //public static T LoadAsset<T>(GameSetting.FILE_PATH filePath) {
    //  return LoadAsset<T>(filePath.Description());
    //}
    public static T LoadAsset<T>(string path, string filename) {
      return LoadAsset<T>(path + "/" + filename);
    }
    public static T LoadAsset<T>(string filepath) {
      object itemAsset = AssetDatabase.LoadAssetAtPath(filepath, typeof(T));
      return (T)itemAsset;
    }
  }
#endif


  public class ReadOnlyAttribute : PropertyAttribute { }
  public class SortingLayerSelectorAttribute : PropertyAttribute {
    public bool UseDefaultSortingLayerFieldDrawer = false;
  }
  public class ObjectLayerSelectorAttribute : PropertyAttribute {
    public bool UseDefaultObjectSelectorDrawer = false;
  }
  public class TagSelectorAttribute : PropertyAttribute {
    public bool UseDefaultTagFieldDrawer = false;
  }
  public class ValueLockerAttribute : PropertyAttribute { }
  public class ValueToggleAttribute : PropertyAttribute { }
  public class ConditionalSelectorAttribute : PropertyAttribute { }
  public class StoryPlaceSelectorAttribute : PropertyAttribute { }
  public class StoryMomentSelectorAttribute : PropertyAttribute { }
  public class StoryActorSelectorAttribute : PropertyAttribute { }
  public class StoryDataRefreshAttribute : PropertyAttribute { }

  [AttributeUsage(AttributeTargets.Field, Inherited = true)]
  public class HelpboxAttribute : PropertyAttribute {
    public enum MessageType {
      None,
      Info,
      Warning,
      Error,
    }
    public readonly string text;

    // MessageType exists in UnityEditor namespace and can throw an exception when used outside the editor.
    // We spoof MessageType at the bottom of this script to ensure that errors are not thrown when
    // MessageType is unavailable.
    public readonly MessageType type;


    /// <summary>
    /// Adds a HelpBox to the Unity property inspector above this field.
    /// </summary>
    /// <param name="text">The help text to be displayed in the HelpBox.</param>
    /// <param name="type">The icon to be displayed in the HelpBox.</param>
    public HelpboxAttribute(string text, MessageType type = MessageType.Info) {
      this.text = text;
      this.type = type;
    }
  }
}