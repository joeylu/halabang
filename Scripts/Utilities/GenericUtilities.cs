using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;
using System.IO;
using UnityEngine;

namespace Halabang.Utilities {
  public class GenericUtilities {
    public static bool RandomChance(float chanceOfSuccess = 0.5f) {
      float fRand = UnityEngine.Random.Range(0.0f, 1.0f);
      return fRand <= chanceOfSuccess;
    }
    public static bool FileExists(string loadPath) {
      return File.Exists(loadPath);
    }
    public static string LoadFromFile(string loadPath) {
      string loadData = "";
      try {
        if (File.Exists(loadPath)) {
          using (FileStream stream = new FileStream(loadPath, FileMode.Open)) {
            using (StreamReader reader = new StreamReader(stream)) {
              loadData = reader.ReadToEnd();
            }
          }
        } else {
          Debug.Log("File is not found to load for " + loadPath);
        }
      } catch (Exception ex) {
        Debug.LogError("Loading game data failed for " + loadPath + ": " + ex.Message);
      }
      return loadData;
    }
    public static void SaveToFile(string savePath, string serializedJson) {
      try {
        //create the directory if it is not existed
        Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        //stringfy game data
        string saveData = serializedJson;
        //write it to the file
        using (FileStream stream = new FileStream(savePath, FileMode.Create)) {
          using (StreamWriter writer = new StreamWriter(stream)) {
            writer.Write(saveData);
          }
        }
      } catch (Exception ex) {
        Debug.LogError("Saving game data failed for " + savePath + ": " + ex.Message);
      }
    }
    public static string ConvertSpriteToBase64(Sprite sprite) {
      // Check if the texture is readable
      if (!sprite.texture.isReadable) {
        Debug.LogError("Sprite's texture is not readable. Please enable 'Read/Write Enabled' in the Texture Import Settings.");
        return null;
      }

      // Create a new Texture2D from the sprite's texture and rect
      Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
      Color[] pixels = sprite.texture.GetPixels((int)sprite.rect.x,
                                                (int)sprite.rect.y,
                                                (int)sprite.rect.width,
                                                (int)sprite.rect.height);
      texture.SetPixels(pixels);
      texture.Apply();

      // Encode the texture to PNG bytes
      byte[] imageBytes = texture.EncodeToPNG();

      // Convert the byte array to Base64 string
      string base64String = Convert.ToBase64String(imageBytes);

      // Clean up the temporary texture
      GameObject.DestroyImmediate(texture);

      return base64String;
    }
  }

  public static class GenericExt {
    public static void DestroyChildren(this Transform root) {
      if (root.childCount == 0) return;

      for (int i = root.childCount - 1; i >= 0; i--) {
        UnityEngine.Object.Destroy(root.GetChild(i).gameObject);
      }
    }
  }
  #region ENUM_ATTRIBUTES
  public static class EnumExt {
    public static string Description(this Enum value) {
      // variables  
      var enumType = value.GetType();
      var field = enumType.GetField(value.ToString());
      var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

      // return  
      return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
    }
    public static T[] FlagsToString<T>(this Enum value) {
      IEnumerable<T> enums = DataHelper.EnumStringList<T>();
      if (enums == null) return null;
      return enums.ToArray();
    }
  }
  #endregion
}
