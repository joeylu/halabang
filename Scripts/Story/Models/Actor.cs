using Halabang.Game;
using Halabang.UI;
using Halabang.Utilities;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Halabang.Character;

namespace Halabang.Story {
  public class Actor {
    #region PROPERTIES_FROM_DATA
    public int ID { get; set; }
    public string Guid { get; set; }

    public string LocalizedName { get; set; }
    public ActorDictionary.ACTOR_GENDER Gender { get; set; }
    public Copywriting CurrentBasic { get; set; } //描述人物的基本（起始）信息，比如性别，年龄，喜好等等基础信息
    public Copywriting CurrentResume { get; set; } //描述人物的简历信息，比如工作经历，教育经历，技能等等
    public Copywriting CurrentRules { get; set; } //AI 对话时，需要限制AI扮演该角色的基础规则
    public Copywriting CurrentResponseRules { get; set; } //AI 对话时，需要限制AI回复的基础规则


    //public List<Place> Appearances { get; set; } = new List<Place>(); //Where this actor has appeared across times
    //public List<Moment> Timelines { get; set; } = new List<Moment>(); //When this actor has been across spaces
    public long AvatarItemID { get; set; }
    public bool Unlocked { get; set; } //player has ever met or heard this actor, set by story manager
    //visualize and UI properties
    #endregion
    #region RUNTIME_PROPERTIES
    /** Halabang values **/
    //public CharacterAct ActingAbility { get; set; } //Reference a runtime CharacterAct component in current scene's characters
    public CharacterBase CurrentEntity { get; set; } //Reference a runtime Character entity component in current scene's characters
    //public AvatarChipset AvatarChip => getAvatarChip();
    //public string AvatarDefaultEntityGUID => AvatarChip?.DefaultEntityID;
    /// <summary>
    /// when actor is an avatar, it must have default entity as its (home entity), return whether or not the avatar is at its default entity
    /// for none avatar actor, this property will always return false because it has no default entity value
    /// </summary>
    //public bool IsAvatarHome => ValidationHelper.IsGuid(AvatarDefaultEntityGUID) && AvatarDefaultEntityGUID.Equals(CurrentEntity.EntityGuid, StringComparison.OrdinalIgnoreCase);
    /** Halabang values **/
    /** Dialogue system values **/
    public bool IsPlayer { get; set; } //assigned by dialogue system
    //public HashSet<Conversation> Conversations { get; set; } = new HashSet<Conversation>();
    public PixelCrushers.DialogueSystem.Actor DialogueActor { get; set; }
    public DialogueActor DialogueActorController { get; set; } //Reference a runtime Dialogue Actor component in current scene's characters
    /** Dialogue system values **/
    /** Game data **/
    public string LastEntityGuid { get; set; } //The previous character entitty this actor was bound, such as an avatar actor
    public List<EntityChatRecord> EntityChatRecords { get; set; } = new List<EntityChatRecord>(); //record bound times with each entity
    /** Game data **/
    #endregion
    #region ASSET_REFERENCE_PROPERTIES
    public UI_DisplaySets DisplaySet { get; set; } = new UI_DisplaySets();
    #endregion
    #region CONSTANTS_PROPERTIES
    public bool IsCommonAI => Guid.Equals(ActorDictionary.COMMON_AI_GUID, StringComparison.OrdinalIgnoreCase);
    #endregion

    #region LOCAL_FIELDS
    private HashSet<Actor> currentActors;
    #endregion

    #region PUBLIC_METHODS
    public Actor() { }
    public Actor(ActorPreset preset) {
      ID = preset.ActorID;
      Guid = preset.ActorGuid;

      DialogueActor = DialogueManager.instance.masterDatabase.GetActor(ID);
      LocalizedName = getLocalizedName();
      CurrentBasic = CopywritingHelper.GetCurrentCopywriting(preset.ActorSettings);
      CurrentResume = CopywritingHelper.GetCurrentCopywriting(preset.ActorResume);
      CurrentRules = CopywritingHelper.GetCurrentCopywriting(preset.ActorRules);
      CurrentResponseRules = CopywritingHelper.GetCurrentCopywriting(preset.ResponseRules);
    }
    public Actor(ActorPresetData data) {
      //CAUTION, convert actor data must be executed AFTER all moment and place are initialized from their database
      //names are getting dynamically on call
      ID = data.ID;
      Guid = data.Guid;

      //icons and images
      if (data.SlotIcon != null) {
        UI_Icons_Setting icons = new UI_Icons_Setting();
        icons.Slot = new AssetReferenceSprite(data.SlotIcon[0]);
        icons.Slot.SubObjectName = data.SlotIcon[1];
        DisplaySet.SetIcons(icons);
      }
      if (data.PortraitImage != null) {
        UI_Images_Setting images = new UI_Images_Setting();
        images.Portrait = new AssetReferenceSprite(data.PortraitImage[0]);
        images.Portrait.SubObjectName = data.PortraitImage[1];
        DisplaySet.SetImages(images);
      }
      //AvatarActor = get chip from data.AvatarChip
      //if (data.Appearances != null) {
      //  foreach (string place_id in data.Appearances) {
      //    Place place = GameManager.instatnce.CurrentStoryManager.GetLocation(place_id);
      //    //Debug.Log(data.DefaultFullName + " has place " + place?.Name);
      //    if (place == null) continue;
      //    Appearances.Add(place);
      //  }
      //}
      //if (data.Timelines != null) {
      //  foreach (string moment_id in data.Timelines) {
      //    Moment moment = GameManager.instatnce.CurrentStoryManager.GetTimeline(moment_id);
      //    //Debug.Log(data.DefaultFullName + " has place " + place?.Name);
      //    if (moment == null) continue;
      //    Timelines.Add(moment);
      //  }
      //}
      AvatarItemID = data.AvatarChipItemID;
    }

    /// <summary>
    /// NOTE: Unique ID Tools MUST BE EXECUTED!
    /// fill up all conversation entry value based on the given dialogue entry
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="dialogueEntry"></param>
    //public void GetDetailFromDialogueSystem(DialogueEntry dialogueEntry, PixelCrushers.DialogueSystem.Conversation dialogueConversation) {
    //  //fill up basics
    //  if (ID == 0) ID = dialogueEntry.ActorID;

    //  //define conversation
    //  Conversation conversation = Conversations.Where(r => r.ID == dialogueConversation.id).FirstOrDefault();
    //  if (conversation == null) {
    //    conversation = new Conversation();
    //    Conversations.Add(conversation);
    //  }
    //  //fill up all values
    //  conversation.GetDetailFromDialogueSystem(dialogueConversation);

    //  //define entry
    //  ConversationEntry conversationEntry = conversation.Entries.Where(r => r.ID == dialogueEntry.id).FirstOrDefault();
    //  if (conversationEntry == null) {
    //    conversationEntry = new ConversationEntry();
    //    conversation.Entries.Add(conversationEntry);
    //  }
    //  //fill up all values
    //  conversationEntry.GetDetailFromDialogueSystem(dialogueEntry);
    //}
    //entity related

    /// <summary>
    /// Return the total count that player has chatted with an actor in days (game calendar), multiple chat times in a single day only counts once
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    //public int EntityChatDaysCount(string entity) => EntityChatRecords.Where(r => r.ID.Equals(entity, StringComparison.OrdinalIgnoreCase)).GroupBy(g => g.AtGameDay).Count();
    /// <summary>
    /// Return the total count that player has ever chatted with an actor
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    //public int EntityChatsCount(string entity) => EntityChatRecords.Where(r => r.ID.Equals(entity, StringComparison.OrdinalIgnoreCase)).Count();
    /// <summary>
    /// Return the total count that player has chatted with an actor from current game day (cycle)
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    //public int EntityChatCountFromToday(string entity) => EntityChatRecords.Where(r => r.ID.Equals(entity, StringComparison.OrdinalIgnoreCase) && r.AtGameDay == GameManager.instatnce.CurrentCalendarManager.CurrentCommoneEraDays).Count();
    //public int EntityChatsHomeCount(bool home) {
    //  if (ValidationHelper.IsGuid(CurrentEntity?.EntityGuid) == false) {
    //    Debug.LogError("Unexpected error, getting chat count must has current entity");
    //    return 0;
    //  }
    //  //by default when avatar has no chat records ever, it sets to 1 day because the first chat at home does not consider returning
    //  //by default whenavatar has no chat records, it sets to 0 day because the first chat should never consider as away home chat
    //  if (EntityChatRecords.Count == 0) return home ? 1 : 0;
    //  int lastHomeIndex = EntityChatRecords.IndexOf(EntityChatRecords.FindLast(r => r.AtHome));
    //  int lastAwayIndex = EntityChatRecords.IndexOf(EntityChatRecords.FindLast(r => r.AtHome == false));
    //  if (home) {
    //    if (lastHomeIndex > lastAwayIndex) {
    //      return lastHomeIndex - lastAwayIndex; //return the differential chat counts since last away
    //    } else {
    //      return 0; //currently it is away, not at home
    //    }
    //  } else {
    //    EntityChatRecord lastAwayRecordOverEntity = EntityChatRecords.FindLast(r => r.AtHome == false && r.ID.Equals(CurrentEntity.EntityGuid, StringComparison.OrdinalIgnoreCase));
    //    if (lastAwayRecordOverEntity == null) return 0; //return 0 as this avatar has never chatted with the given entity
    //    int lastAwayIndexOverEntity = EntityChatRecords.IndexOf(lastAwayRecordOverEntity); //find the index of that chatted record with the given entity
    //    if (lastAwayIndexOverEntity == lastAwayIndex) {
    //      //if the last away chatted with the given entity is the last away chat
    //      EntityChatRecord lastNotAwayRecordOverEntity = EntityChatRecords.FindLast(r => r.ID.Equals(CurrentEntity.EntityGuid, StringComparison.OrdinalIgnoreCase) == false); //find the last chat record that is not with the given entity, regardless away or home
    //      if (lastNotAwayRecordOverEntity == null) {
    //        //if there is no not away records over given entity, meaning all chat records are with the given entity, return the list count
    //        return EntityChatRecords.Count;
    //      } else {
    //        //if there is a not away record over given entity found, return the differential
    //        return lastAwayIndexOverEntity - EntityChatRecords.IndexOf(lastNotAwayRecordOverEntity);
    //      }
    //    } else {
    //      //current it is at home, not away
    //      return 0;
    //    }
    //  }
    //}
    //misc
    public ActorRuntimeData ConvertToData() {
      ActorRuntimeData actorData = new ActorRuntimeData();
      actorData.ID = Guid;
      actorData.DialogueName = DialogueActor.Name;
      actorData.LastEntity = LastEntityGuid;
      actorData.EntityChatRecords = EntityChatRecords;

      return actorData;
    }
    #endregion
    #region LOCAL_METHODS
    //public AvatarChipset getAvatarChip() {
    //  if (AvatarItemID != 0) {
    //    GameItem avatarChipset = GameManager.instatnce.CurrentInventoryManager.GetGameItem(0, AvatarItemID);
    //    if (avatarChipset == null) {
    //      Debug.LogError("Unexpected error: actor preset with actor guid " + Guid + " is an avatar but its avatar chipset item is not found in item database: " + AvatarItemID);
    //    }
    //    if (avatarChipset != null && avatarChipset.ChipsetItem?.AvatarChip == null) {
    //      Debug.LogError("Unexpected error: avatarChipset " + avatarChipset.ID + " is an avatar but its avatar chipset data is null");
    //    }
    //    return avatarChipset?.ChipsetItem.AvatarChip;
    //  }
    //  return null;
    //}
    private string getLocalizedName() {
      if (DialogueActor == null) {
        Debug.LogError("Unexpected error: cannot find actor from current database: " + Guid);
        return string.Empty;
      }

      string displayName = Field.LookupLocalizedValue(DialogueActor.fields, DialogueSystemFields.DisplayName);
      return string.IsNullOrWhiteSpace(displayName) ? DialogueActor.localizedName : displayName;
    }
    #endregion
    #region STATIC_METHODS
    //public static IEnumerable<Actor> GetActors(ActorQuery query) {
    //  try {
    //    if (GameManager.instatnce.CurrentStoryManager.Actors == null) return null;

    //    return GameManager.instatnce.CurrentStoryManager.Actors
    //      .Where(r => (query.StorylineID < 1) || (r.Timelines.Any(timeline => (int)timeline.CalendarType.Storyline() == query.StorylineID && r.Appearances.Any(location => (int)location.Location.Storyline() == query.StorylineID))))
    //      .Where(r => ValidationHelper.IsGuid(query.LocationID) == false || r.Appearances.Any(a => a.ID.Equals(query.LocationID, StringComparison.OrdinalIgnoreCase)))
    //      .Where(r => (query.GenerationID <= 0) || r.Timelines.Any(timeline => timeline.Generation() != null));

    //  } catch (Exception ex) {
    //    Debug.LogError("Unexpected error while getting actor list from query " + ex.Message);
    //    return null;
    //  }
    //}
    #endregion
  }

  public class ActorDictionary {
    public enum ACTOR_TYPE { PLAYER, NPC, PROPERTY }
    public enum ACTOR_ACTION { NULL, GREETING, GOODBYE }
    public enum ACTOR_GENDER { NULL, UNIDENTIFIED, MALE, FEMALE }
    public enum ACTOR_EMOTION { 
      NEUTRAL, 
      HAPPY, 
      VERY_HAPPY, 
      SAD, 
      VERY_SAD 
    }
    //constants
    public const string COMMON_AI_GUID = "0fee5e8d-2502-4f59-ba41-d0e77a2b3c4c";
  }
  public class ActorQuery {
    public int StorylineID = -1;
    public string LocationID;
    public int GenerationID;
    public int Gender = (int)ActorDictionary.ACTOR_GENDER.NULL;
  }
}
