using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Linq;
using Halabang.Plugin;

namespace Halabang.Editor {
  public class DialogueEditorHelper {
    public static string GetActorName(int actorID, GameSettings.GLOBAL_SETTING_LANGUAGE language) {
      string name = string.Empty;

      Actor actor = GetActor(actorID);
      if (actor == null) return name;

      string defaultName = actor.fields.Where(r => r.title == DialogueSystemDictionary.FIELD_NAME_QUEST_DISPLAY_NAME).First().value;
      switch (language) {
        case GameSettings.GLOBAL_SETTING_LANGUAGE.简体中文:
          name = actor.fields.Where(r => r.title == DialogueSystemDictionary.FIELD_NAME_QUEST_DISPLAY_NAME_CNS).First().value;
          break;
        case GameSettings.GLOBAL_SETTING_LANGUAGE.繁體中文:
          name = actor.fields.Where(r => r.title == DialogueSystemDictionary.FIELD_NAME_QUEST_DISPLAY_NAME_CNT).First().value;
          break;
      }

      return string.IsNullOrEmpty(name) ? defaultName : name;
    }
    public static string GetActorGuid(int actorID) {
      DialogueDatabase targetDatabase = GetGlobalDialogueDB();
      return targetDatabase.GetActor(actorID)?.fields.Where(r => r.title == DialogueSystemDictionary.FIELD_NAME_ACTOR_GUID).First().value;
    }
    public static Actor GetActor(int actorID) {
      DialogueDatabase targetDatabase = GetGlobalDialogueDB();
      return targetDatabase.GetActor(actorID);
    }
    public static DialogueDatabase GetGlobalDialogueDB() {
      return EditorUtilities.LoadAsset<DialogueDatabase>(GameSettings.FILE_PATH.DATABASE_DIALOGUE_GLOBAL);
    }
  }
}
