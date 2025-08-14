using UnityEditor;
using UnityEngine;
using Halabang.Utilities;

namespace Halabang.Editor {
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
    public static T LoadAsset<T>(GameSettings.FILE_PATH filePath) {
      return LoadAsset<T>(filePath.Description());
    }
    public static T LoadAsset<T>(string path, string filename) {
      return LoadAsset<T>(path + "/" + filename);
    }
    public static T LoadAsset<T>(string filepath) {
      object itemAsset = AssetDatabase.LoadAssetAtPath(filepath, typeof(T));
      return (T)itemAsset;
    }
  }
}
