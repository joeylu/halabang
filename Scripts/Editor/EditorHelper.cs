using System;
using UnityEditor;
using UnityEngine;

namespace Halabang.Editor {
  /// <summary>
  /// This class contain custom drawer for ReadOnly attribute.
  /// https://www.patrykgalach.com/2020/01/20/readonly-attribute-in-unity-editor/
  /// </summary>
  [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
  public class ReadOnlyDrawer : PropertyDrawer {
    /// <summary>
    /// Unity method for drawing GUI in Editor
    /// </summary>
    /// <param name="position">Position.</param>
    /// <param name="property">Property.</param>
    /// <param name="label">Label.</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      // Saving previous GUI enabled value
      var previousGUIState = GUI.enabled;
      // Disabling edit for property
      GUI.enabled = false;
      // Drawing Property
      EditorGUI.PropertyField(position, property, label);
      // Setting old GUI enabled value
      GUI.enabled = previousGUIState;
    }
  }
  public class EditorMessage {
    public static readonly string SAVE_SUCCESS = "{0} is saved at " + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss" + Environment.NewLine + "{1}");
    public static readonly string SAVE_FAILED = "Unexpected error! {0} failed to save at " + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss") + Environment.NewLine + "{1}";
    public static readonly string SAVE_INVALID = "Unexpected error! failed to save {0} because of " + Environment.NewLine + "{1}";
    public static readonly string FUNCTION_NOT_IMPLEMENT = "{0} has not yet implemented, contact administrator." + Environment.NewLine + "{1}";
    public static readonly string RECORD_FOUND = "This {0} data record has been found from current database";
    public static readonly string RECORD_INVALID = "This {0} data record has unexpected error";
    public enum MESSAGE_TYPE { None, Info, Warning, Error, }
  }
  public class EditorLayout {
    [Flags]
    public enum TEXT_STYLE {
      DEFAULT = 0,
      COLOR_RED = 1,
      COLOR_GREEN = 2
    }

    public static readonly Color COLOR_WARNING = new Color(255, 173, 50);
    public static readonly Color COLOR_ERROR = new Color(255, 50, 50);
    public static readonly Color COLOR_SUCCESS = new Color(255, 22, 255);
    public static readonly Color COLOR_SECONDARY = new Color(157, 157, 157);

    public static GUIStyle TEXT_RED => getStyle(TEXT_STYLE.COLOR_RED);
    public static GUIStyle TEXT_GREEN => getStyle(TEXT_STYLE.COLOR_GREEN);

    private static GUIStyle getStyle(TEXT_STYLE style) {
      GUIStyle newStyle = new GUIStyle(EditorStyles.label);
      switch (style) {
        case TEXT_STYLE.COLOR_RED:
          newStyle.normal.textColor = Color.red;
          break;
        case TEXT_STYLE.COLOR_GREEN:
          newStyle.normal.textColor = Color.green;
          break;
      }
      return newStyle;
    }

    public static void PrefixGuidButton(string prefix_text, string button_text, Action onclick) {
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.PrefixLabel(prefix_text);
      if (GUILayout.Button(button_text)) {
        onclick.Invoke();
        //serializedProperty.stringValue = Guid.NewGuid().ToString(); //auto assign a guid
      }
      EditorGUILayout.EndHorizontal();
    }

    public static void HorizontalLine(Color color) {
      GUIStyle horizontalLine;
      horizontalLine = new GUIStyle();
      horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
      horizontalLine.margin = new RectOffset(0, 0, 4, 4);
      horizontalLine.fixedHeight = 1;

      var c = GUI.color;
      GUI.color = color;
      GUILayout.Box(GUIContent.none, horizontalLine);
      GUI.color = c;
    }
  }
}