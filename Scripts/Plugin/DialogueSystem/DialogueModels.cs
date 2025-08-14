using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
//using Halabang.Story;

namespace Halabang.Plugin {
  [Serializable]
  public class DialogueSlotDataVariable {
    public class Variable {
      public int SceneIndex { get; set; }
      public int SpawnPointName { get; set; }
      //public ConversationEntry[] ConversationUpdater { get; set; }
    }
  }

  [Serializable]
  public class DialogueVariable {
    public string Name;
    //public GeneralSettings.VALUE_TYPE ValueType;
    public bool ValueBool;
    public float ValueNumber;
    public string ValueString;
  }

  public class DialogueSystemDictionary {
    public const string FIELD_NAME_QUEST_STATE = "State";
    //public const string FIELD_NAME_QUEST_TRACKABLE = "Trackable";
    //public const string FIELD_NAME_QUEST_ABANDONEABLE = "Abandonable";
    public const string FIELD_NAME_QUEST_DISPLAY_NAME = "Display Name";
    public const string FIELD_NAME_QUEST_DISPLAY_NAME_CNS = "Display Name zh-CN";
    public const string FIELD_NAME_QUEST_DISPLAY_NAME_CNT = "Display Name zh-TW";
    public const string FIELD_NAME_QUEST_GROUP = "Group";
    public const string FIELD_NAME_ACTOR_GUID = "ActorGuid";
    public const string FIELD_NAME_STORY_ACTOR = "IsStoryActor";
    //public const string FIELD_NAME_ITEM_ACTOR_BIND = "BindActor";
    public const string FIELD_NAME_VALUE_NAME = "Name";
    public const string FIELD_NAME_VALUE_VOICE = "Voice";
    public const string FIELD_NAME_VALUE_MUSIC = "Music";
    public const string FIELD_NAME_VALUE_BEHAVIOR = "Behavior";
    public const string FIELD_NAME_VALUE_POSITION = "Position";
    public const string FIELD_NAME_VALUE_ROTATION = "Rotation";

    public const string ACTOR = "Actor"; //inheirted from dialogue entry nameof Actor
    public const string CONVERSANT = "Conversant"; //inheirted from dialogue entry nameof Conversant

    public const string MARKUP_FORCE = "[f]";

    public const float DEFAULT_IN_SCENE_TEXT_DURATION = 10f;
  }
}
