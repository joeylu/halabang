using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using Halabang.Character;

namespace Halabang.Game {
  public class GameData {
    //public GameSettings.GAME_SAVE_SLOT Slot { get; set; }
    public int GameDay { get; set; } = 1;
    public string Brief { get; set; }
    public string DialogueSystemData { get; set; }
    //public HashSet<ConversationEntry> ConversationLogger { get; set; }
    public IEnumerable<SceneData> PersistentScenesData { get; set; }
    public IEnumerable<UserItemLog> PersistentUserItemLogs { get; set; }
    public IEnumerable<QuestData> PersistentQuestsData { get; set; }
    public PlayerStoryLog PlayerStory { get; set; }
    /// <summary>
    /// Global inventory, use GUID to perserve each inventory items across scenes
    /// </summary>
  }
  [Serializable]
  public class GamePreferences {
    public GameSettings.GLOBAL_SETTING_LANGUAGE DefaultLanguage { get; set; }
    //gameplay
    public GameSettings.GLOBAL_SETTING_LANGUAGE CurrentLanguage { get; set; }
    public bool EnableSubtitle { get; set; } = true;
    //visual
    public GameSettings.GLOBAL_SETTING_RESOLUTION Resolution { get; set; }
    public FullScreenMode ScreenMode { get; set; }
    public float Brightness { get; set; }
    public bool EnableVsync { get; set; }
    public GameSettings.GLOBAL_SETTING_SHADOW Shadow { get; set; }
    //audio
    public float MixerMasterVolume { get; set; } = 1f;
    public float MixerMusicVolume { get; set; } = 1f;
    public float MixerSFXVolume { get; set; } = 1f;
    public float MixerVoiceVolume { get; set; } = 1f;
    //runtime
    //public GameSettings.GLOBAL_SETTING_LANGUAGE DefaultLanguage { get; set; }
  }
  [Serializable]
  public class SceneData {
    /// <summary>
    /// Indicate the last scene that player has played, paired for menu>continue button, value is set whenever save occur in save load manager
    /// </summary>
    public bool IsLastPlayedScene; 
    //scene general
    //public GameSettings.SCENE SceneIndex;
    //[Tooltip("Optional, if presented, the scene will active the target camera setting on initializing")]
    //public string CameraSetting;
    //public float[] CameraPosition;
    //public float[] CameraRotation;
    //public float CameraFadingDelay;
    //player data and player input related data
    public bool PlayerConstraintAxisZ;
    public CharacterData PlayerData;
    //scene savers
    public IEnumerable<SaverData> PersistentSavers; //all in scene component saved data (lights etc)
    public IEnumerable<PlayerZoneData> PersistentPlayerzones { get; set; } //all in scene player zone data
    public IEnumerable<CheckpointData> PersistentCheckpoints { get; set; } //all in scene checkpoint and examinable data
    public IEnumerable<PropertyData> PersistentProperties { get; set; } //all in scene property data (position, health etc)
    public IEnumerable<CharacterData> PersistentCharacters { get; set; } //all in scene character data (position, health etc)
    public IEnumerable<ActorRuntimeData> PersistentActors { get; set; } //all in scene actor data (entity paired, in scene chat records, etc)
    public IEnumerable<InventoryData> PersistentInventoryData { get; set; } //all in scene local inventory and its slot data
  }
  [Serializable]
  public class PlayerZoneData {
    public string Name;
    public string ID;
    public float StayLength; //total time in second that player has stayed in the zone
    public bool IsActive;
  }
  [Serializable]
  public class CheckpointData {
    public string Name;
    public string ID;
    public bool IsActive;
    public bool IsChecked;
  }
  [Serializable]
  public class QuestData {
    public string Name;
    public string Guid;
    //public GameQuestDictionary.QUEST_STATE QuestState;
    public string ChapterID;
  }
  [Serializable]
  public class InventoryData {
    public string Name;
    public string ID;
    public IEnumerable<SlotItemData> ItemCollection;
    public bool LockAfterItemCreation;
  }
  /// <summary>
  /// A data class to carry over additional information to override when setting up a slot, such as initial health value
  /// </summary>
  [Serializable]  
  public class SlotItemData {
    public string Name;
    public string SlotID;
    public long NoneInstanceID; //an item world id that can be identified for an instance
    public int Quantity;
    public long PreviousInstanceID; //when not 0, this item is a replacement of an existing instanced game item from inventory manager runtime item list
    //public GameItemDictionary.ITEM_STATE DefaultState;
    public int BoundPropertyHealth = -1;

    public bool IsVirtual() {
      return false;
      //return SlotID.Equals(GameSettings.IGNORE_GUID, StringComparison.OrdinalIgnoreCase);
    }
  }
  [Serializable]
  public class PropertyData {
    public string Name;
    public string ID;
    public bool IsAwaken;
    public int Health;
    public int Stamina;
  }
  [Serializable]
  public class CharacterData {
    public string Name;
    public string EntityID;
    public string ActorID;
    //public CharacterMotion.MOTION_STATE MotionState;
    public float[] Position; //newton json has problem to serialize vector3
    public float[] Rotation;
    public int Health = -1;
    public int Stamina = -1;
    public bool WalkingLocked;
    public bool RunningLocked;

    public static CharacterData ConvertData(CharacterBase character) {
      CharacterData data = new CharacterData();
      data.Name = character.name;
      //data.EntityID = character.EntityGuid;
      //data.ActorID = character.ActorGuid;
      //if (character._CharacterMotion != null) {
      //  data.MotionState = character._CharacterMotion.CurrentMotionState;
      //} else {
      //  //there is only one possibility that _CharacterMotion is null, that is when this character has never been awaked from current scene
      //  data.MotionState = CharacterMotion.MOTION_STATE.GROUNDING;
      //}
      //data.Position = character.transform.position.ConvertToFloat();
      //data.Rotation = character.transform.rotation.ConvertToFloat();
      //if (character._CharacterHealth) data.Health = character._CharacterHealth.CurrentObjectHealth; //sometimes a inactive character has not yet initialized its health, so it should not be checked
      //if (character._CharacterStamina) data.Stamina = character._CharacterStamina.CurrentStamina;

      //data.WalkingLocked = character._CharacterMotion._CharacterGround.LockWalking;
      //data.RunningLocked = character._CharacterMotion._CharacterGround.LockRunning;

      return data;
    }
  }
  [Serializable]
  public class ActorRuntimeData {
    public string DialogueName;
    public string ID;
    public string LastEntity;
    public List<EntityChatRecord> EntityChatRecords; //use list for character manager reference display in inspector
  }
  [Serializable]
  public class EntityChatRecord {
    public string ID; //entity guid
    public int AtGameDay; //date in game calendar
    public bool AtHome; //whether or not this chat was triggered while this entity with its bound entity (default entity)
  }
  [Serializable]
  public class PlayerStoryLog {
    public string[] UnmaskedParagraphs; //paragraphs that player has played
    public string[] CheckedRemarks; //remark data that player has checked
    public string[] UnlockedRecalls; //recalls data that player has earned
    public int[] KnownActors; //actors data that player has met
    public ConsoleChatLog[] ConsoleChatLogs;
  }
  [Serializable]
  public class ConsoleChatLog {
    public string ChatperID; //the chapter where this log is belonged, TO-DO: for a chat group that is across the scene, use duplicate chat preset for now
    public string ChatGroupID;
    public int LastVisibleIndex; //the last visible chat record index
    public List<ConsoleChatRecordLog> RecordsRead; //all records that user has played;

    public ConsoleChatLog() { }
    //public ConsoleChatLog(ChatGroup chatGroup) {
    //  ChatGroupID = chatGroup.ID;
    //  ChatperID = chatGroup.ChapterID;
    //  LastVisibleIndex = chatGroup.CurrentVisibleIndex;

    //  RecordsRead = new List<ConsoleChatRecordLog>();
    //  foreach (ChatRecord record in chatGroup.Records) {
    //    ConsoleChatRecordLog recordLog = new ConsoleChatRecordLog();
    //    recordLog.RecordIndex = record.Index;
    //    recordLog.RecordSelectedIndex = record.SelectedResponse;
    //    RecordsRead.Add(recordLog);
    //  }
    //}
  }
  [Serializable]
  public class ConsoleChatRecordLog {
    public int RecordIndex; //the index from the chat group
    public int RecordSelectedIndex; //the response index that has selected from player
  }

  [Serializable]
  public struct InventoryItemDataReference {
    public string Name;
    public string UniqueID;
    public IEnumerable<string> Items;
  }

  [Serializable]
  public class UserItemLog {
    public string Name;
    public string ActorID;
    public int Categories;
    public long NoneInstanceID; //an item world id that can be identified for an instance
    public long InstanceID; //when not 0, this item is an instanced item
    public bool Marked; //if true, this item has been checked from player inventory data
    public bool Viewed; //if true, this item has been checked from player story data
  }

  [Serializable]
  public class SaverData {
    public string Name;
    public string ID;
    public DialogueTriggerData DefaultDialogueTriggerData;
    public LightData DefaultLightData;
    public ReflectionProbeData DefaultReflectionData;
    public LightBeamData DefaultLightBeamData;
    public ParticleData DefaultParticleData;
    public VisualEffectData DefaultVisualEffectData;
  }
  [Serializable]
  public class LightData {
    public string Name;
    public bool Activated;
    public float[] ColorValue;
    public float Intensity = -1f;
    public float Range = -1f;
  }
  [Serializable]
  public class ReflectionProbeData {
    public string Name;
    public bool Activated;
  }
  [Serializable]
  public class DialogueTriggerData {
    public string Name;
    public string ID;
    public bool IsTriggered;
  }
  [Serializable]
  public class LightBeamData {
    public bool IsHD;
    public string Name;
    public bool Activated;
  }
  [Serializable]
  public class ParticleData {
    public string Name;
    public bool Activated;
  }
  [Serializable]
  public class VisualEffectData {
    public string Name;
    public bool Activated;
  }
  [Serializable]
  public class SceneLoaderSetting {
    public bool IsSceneDefault { get; set; } //indicate this setting was from saved file or from scene manager default setting
    public IEnumerable<SaverData> PersistentSavers { get; set; }

    [Header("Scene")]
    //public GameSettings.SCENE SceneIndex;
    public string TransitionText;

    [Header("Player")]
    //public Vector3 PlayerPosition;
    //public Quaternion PlayerRotation;
    //public CharacterMotion.MOTION_STATE PlayerMotionState = CharacterMotion.MOTION_STATE.GROUNDING;
    [Tooltip("Should player z axis to be locked by default")]
    public bool ConstraintAxisZ;
    public CharacterData PlayerData;

    public SceneLoaderSetting() { }
    public static SceneData ConvertToSceneData(SceneLoaderSetting setting) {
      SceneData rawData = new SceneData();
      //scene
      //rawData.SceneIndex = setting.SceneIndex;
      //rawData.CameraSetting = setting.CameraSetting?.name;
      //rawData.CameraPosition = setting.CameraPlacement.Position.ConvertToFloat();
      //rawData.CameraRotation = setting.CameraPlacement.Rotation.ConvertToFloat();
      //player
      rawData.PlayerConstraintAxisZ = setting.ConstraintAxisZ;
      rawData.PlayerData = setting.PlayerData;
      //scene savers
      rawData.PersistentSavers = setting.PersistentSavers;

      return rawData;
    }
  }
}
