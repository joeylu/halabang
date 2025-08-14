using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Halabang {
  public class GameSettings {
    #region PROPERTIES
    #endregion
    #region SERIALIZED_FIELDS
    #endregion
    #region LOCAL_FIELDS
    #endregion
    #region UNITY_METHODS
    #endregion
    #region PUBLIC_METHODS
    #endregion
    #region LOCL_METHODS
    #endregion

    //Default Name
    public const string PROJECT_NAME = "CatWoman";
    public const float DEFAULT_TIMEOUT = 5f;
    public const float SHORT_TIMEOUT = 2.5f;
    public const float LONG_TIMEOUT = 15f;
    public const float UI_TIMEOUT = 0.5f;

    //Layers
    public const string LAYER_PLAYER = "Player";
    public const string LAYER_CONTROLLER = "Controller";
    public const string LAYER_PLATFORM = "Platform";
    public const string LAYER_PARTICLE = "TransparentFX";
    public const string LAYER_PROPERTY = "Property";
    public const string LAYER_ITEM = "Item";
    public const string LAYER_AGENT = "Agent";
    public const string LAYER_DEFAULT = "Default";
    public const string LAYER_COLLISIONER = "Collisioner";
    public const string LAYER_UI = "UI";
    public const string LAYER_PENETRABLE = "Penetrable";

    //General Tags
    public const string TAG_DEFAULT = "Untagged";
    public const string TAG_CAMERA_MAIN = "MainCamera";
    public const string TAG_PLAYER = "Player";
    public const string TAG_LIGHT = "Light";
    public const string TAG_DUMMY = "Dummy";
    public const string TAG_TARGET = "Target";
    //Priority Tags to determine sorting orders in any case
    public const string TAG_UNIQUE = "Unique";
    public const string TAG_AGENT = "Agent";
    public const string TAG_LEADER = "Leader";
    public const string TAG_CAMERAMAN = "Cameraman";
    public const string TAG_BACKGROUND = "Background";


    //constant gameobjects names
    public const string GO_MANAGERS = "Managers";
    public const string MANAGER_GAME = "GameManager";
    public const string MANAGER_LIGHTING = "LightingManager";
    public const string MANAGER_POSTPROCESSING = "PostProcessingManager";
    public const string MANAGER_CONTROLLER = "InSceneControllers";
    public const string MANAGER_SCENE = "SceneManager";
    public const string MANAGER_GIZMOS = "GizmosManager";
    public const string MANAGER_DIALOGUE= "DialogueManager";

    public const string SPAWN_POINTS = "SpawnPoints";
    public const string CANVAS = "Canvas";
    public const string ANY_CAMERA = "**ANY CAMERA**";

    //body and bone names
    public const string GO_CHARACTER_BODY_LOD = "body_LOD";
    public const string GO_CHARACTER_OUTFIT_LOD = "outfit_LOD";
    public const string GO_CHARACTER_HAIR_LOD = "hair_LOD";
    public const string GO_CHARACTER_BODY = "Body";
    public const string GO_CHARACTER_SPINE_1 = "spine1";
    public const string GO_CHARACTER_SPINE_2 = "spine2";
    public const string GO_CHARACTER_SPINE_3 = "spine3";
    public const string GO_CHARACTER_HEAD = "Head";
    public const string GO_CHARACTER_NECK = "Neck";

    //scene global events
    public enum GLOBAL_WEATHER_EVENT { CALM, BREEZE, WINDY, RAIN, STORM, SNOW, SNOW_STORM }
    public const float MAX_GLOBAL_WEATHER_CALM = 1;
    public const float MAX_GLOBAL_WEATHER_BREEZE = 3;
    public const float MAX_GLOBAL_WEATHER_WINDY = 10;

    //animations
    public const string ANIMATOR_DEFAULT_PARAM = "GROUND_IDLE";
    public const string IN_TRANSITION_CLIPNAME = "inTransitionDuration";
    public const string ANIMATOR_DEFAULT_ENTRY_STATE = "Empty";
    public const string ANIMATOR_PARAM_EMPTY = "EMPTY";

    //UI
    public enum MENU_STATE { MENU_OPEN, MENU_CLOSE, MENU_COUNTING, MENU_CLICKED, MENU_PRESSED, MENU_HOVER, MENU_IDLE };

    //dialogues
    public const string DIALOGUE_KEYWORD_MENU_HIDDEN_TRIGGER = "HIDDEN_CHOICE_TRIGGER";
    public const string DIALOGUE_KEYWORD_GAMEBASE_TODAY = "TODAY";
    public const string DIALOGUE_KEYWORD_ONCE_ONLY = "ONCE_ONLY";
    public const string DIALOGUE_KEYWORD_RANDOM_NEXT_ENTRY = "RANDOM_NEXT_ENTRY"; //Dialogue system has its own randomNextEntry() but only works in NPC entry, TO-DO: check dialogue system native solution
    public const string DIALOGUE_VARIABLE_SCENE_INDEX = "SceneIndex";
    public const string DIALOGUE_VARIABLE_SPAWN_POINT = "SpawnPointName";
    public const string DIALOGUE_CONVERSATION_UPDATER = "ConversationUpdater";

    //item
    public const string ITEM_PUBLIC_KEY = "PUBLIC_KEY";
    public const string ITEM_PRIVATE_KEY = "PRIVATE_KEY";

    //Other
    public const string IGNORE_GUID = "a29223e4-6683-4ef6-9f9c-4518f02b7a64";
    public const float MIN_PRECISION_CHECK = 0.001f;
    public const float MIN_CLOSE_CHECK = 0.02f;
    public const float MIN_LOOSE_CHECK = 0.05f;
    public const float MIN_NEAR_CHECK = 0.1f;
    public const float MIN_WIDE_CLOSE_CHECK = 1f;
    public const float MIN_TIMING_CHECK = 0.01f;
    public const float MIN_NEAR_TIMING_CHECK = 0.5f;
    public const string DEFAULT_CONTRIBUTOR = "Anonymous";

    //Hierarchy
    public const string HIERARCHY_LOD = "_LOD";

    //File or folder paths
    public const string SAVE_FILE_PATH = "GameData";
    public const string SAVE_FILE_HEADER = "user_data_";
    public const string INVENTORY_RESOURCE_PATH = "Assets/Resources";
    public const string INVENTORY_ITEM_PATH = "Assets/Resources/Items";
    public const string INVENTORY_QUEST_PATH = "Assets/Resources/Quests";

    //Addressables
    public enum ADDRESSABLE_CHAPTER_GROUP {
      [Description("Story_Chapter_Shared")] STORY_CHAPTER_SHARED,
      [Description("Story_Chapter_LBA")] STORY_CHAPTER_LITTLE_BLACK_ANT,
      [Description("Story_Chapter_SFF")] STORY_CHAPTER_SPACE_FIREFLY,
    }
    public const string ADDRESSABLE_GROUP_STORY_CHATS_LITTLE_BLACK_ANT = "Story_Chats_LBA";
    public const string ADDRESSABLE_GROUP_STORY_DATA = "Story_Data";
    public const string ADDRESSABLE_GROUP_STORY_ACTOR = "Story_Actors";
    public const string ADDRESSABLE_GROUP_COPYWRITING = "Copywritings";

    //Calendar dates
    public static DateTime CryoCeneFirstDate = DateTime.Parse("3000-01-01");
    public static DateTime MinmeiFirstDate = DateTime.Parse("2930-01-01");
    public static DateTime ShrikeFirstDate = DateTime.Parse("2800-01-01");

    //global setting
    public enum GLOBAL_SETTING_LANGUAGE {
      [Description("en")] ENGLISH, [Description("zh-CN")] 简体中文, [Description("zh-TW")] 繁體中文
    }
    public enum GLOBAL_SETTING_RESOLUTION {
      P720_30, P720_60, P1080_30, P1080_60, P2K_30, P2K_60
    }
    public enum GLOBAL_SETTING_SHADOW {
      [Description("generic_none")] NONE, [Description("generic_low")] 低质量, [Description("generic_high")] 高质量
    }
    public const string GLOBAL_SETTING_SLOT = "global_slot";
    public const string CURRENT_SLOT_NAME = "current_slot"; //store current saved slot name

    [JsonConverter(typeof(StringEnumConverter))]
    public enum GAME_SAVE_SLOT {
      [Description("new_chapter")] NEW_CHAPTER,
      [Description("save_slot_first")] SAVE_SLOT_1,
      [Description("save_slot_second")] SAVE_SLOT_2,
      [Description("save_slot_third")] SAVE_SLOT_3,
      [Description("save_slot_forth")] SAVE_SLOT_4,
      [Description("save_slot_fifth")] SAVE_SLOT_5,
      [Description("save_slot_sixth")] SAVE_SLOT_6,
      [Description("save_slot_seventh")] SAVE_SLOT_7,
      [Description("save_slot_seventh")] SAVE_SLOT_BACKUP
    }
    public enum SCENE {
      [Description("entry_scene")] ENTRY,
      [Description("demo_scene")] DEMO,
      [Description("main_menu")] MAIN_MENU,
      [Description("little_black_ant")] LITTLE_BLACK_ANT,
      [Description("little_black_ant")] LITTLE_BLACK_ANT_LIBRARY_FLASHBACK,
      [Description("captain_halloway")] CAPTAIN_HALLOWAY,
      [Description("test_scene_joey")] JOEY_TEST,

    }
    public static IEnumerable<string> GAME_SLOTS_ALL() {
      List<string> slots = new List<string>();
      foreach (var slot in Enum.GetNames(typeof(GAME_SAVE_SLOT))) {
        slots.Add(slot);
      }
      return slots;
    }

    //Register reference names for reflection
    //CAUTION: must add new ref at last line of collection, otherwise, it will effect all previous dropdown selection in editor scene
    public enum REGISTERED_REF {
      NULL,
      //built-ins
      [Description("UnityEngine.Material, UnityEngine")] UNITYENGINE_MATERIAL,
      [Description("UnityEngine.Material, UnityEngine")] UNITYENGINE_MATERIAL_SHARED,      
      [Description("UnityEngine.Transform, UnityEngine")] UNITYENGINE_TRANSFORM,
      [Description("UnityEngine.ParticleSystem")] UNITYENGINE_PARTICLE_SYSTEM,
      [Description("UnityEngine.Light, UnityEngine")] UNITYENGINE_LIGHT,
      [Description("UnityEngine.SkinnedMeshRenderer, UnityEngine")] UNITYENGINE_SKIN_MESH_RENDERER,
      [Description("UnityEngine.RenderSettings, UnityEngine")] UNITYENGINE_RENDER_SETTINGS,
      //plugins
      [Description("DistantLands.Cozy.CozyWeather")] COZY_WEATHER_SYSTEM,
      [Description("LPWAsset.LowPolyWaterScript")] LOWPOLY_WATER,
      [Description("VLB.VolumetricDustParticles")] VOLUMETRIC_DUST_PARTICLES,
      [Description("VLB.VolumetricLightBeamSD")] VOLUMETRIC_LIGHT_BEAM_SD,
      [Description("VLB.VolumetricLightBeamHD")] VOLUMETRIC_LIGHT_BEAM_HD,
      [Description("MirzaBeig.Scripting.Effects.TurbulenceParticleAffector")] AFFECTOR_TURBULENCE,
      [Description("Beautify.Universal.Beautify")] BEAUTIFY_SETTING,
    }
    public enum FILE_PATH {
      [Description("Assets/Settings/Databases/catwoman_global.asset")] DATABASE_DIALOGUE_GLOBAL
    }
    public enum FOLDER_PATH {

    }
  }
}