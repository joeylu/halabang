using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Halabang.Story;
using Halabang.Plugin;

namespace Halabang.Editor {
  [CustomEditor(typeof(ActorPreset))]
  public class ActorPresetEditor : UnityEditor.Editor {
    #region FIELDS
    private string saveNote;
    private string warningNote;
    private StringBuilder validationMsg = new StringBuilder();
    private ActorPresetData savedData;
    private ActorPresetData presetData = new ActorPresetData();
    //private List<KeyValuePair<int, string>> dialogueActors;
    //private string[] actorOptions;
    //private bool namesFolded;

    private SerializedProperty m_warning;
    private SerializedProperty m_actorID;
    //private SerializedProperty m_selectedActorID;
    //private SerializedProperty m_selectedActorName;
    private SerializedProperty m_guid;
    private SerializedProperty m_actorNameEn;
    private SerializedProperty m_actorNameCn;
    //private SerializedProperty m_middleName;
    //private SerializedProperty m_lastName;
    //private SerializedProperty m_nickName;
    //private SerializedProperty m_displayName;
    private SerializedProperty m_actorSettings;
    private SerializedProperty m_actorResume;
    private SerializedProperty m_actorRules;
    private SerializedProperty m_responseRules;
    //private SerializedProperty m_icons;
    //private SerializedProperty m_images;
    //private SerializedProperty m_refreshStoryData;
    //private SerializedProperty m_timelines;
    //private SerializedProperty m_places;
    //private SerializedProperty m_avatarChip;
    #endregion
    private void OnEnable() {
      m_warning = serializedObject.FindProperty("warning");
      m_actorID = serializedObject.FindProperty("actorID");
      //m_selectedActorID = serializedObject.FindProperty("selectedActorID");
      //m_selectedActorName = serializedObject.FindProperty("selectedActorName");
      m_guid = serializedObject.FindProperty("actorGuid");
      m_actorSettings = serializedObject.FindProperty("actorSettings");
      m_actorResume = serializedObject.FindProperty("actorResume");
      m_actorRules = serializedObject.FindProperty("actorRules");
      m_responseRules = serializedObject.FindProperty("responseRules");
      m_actorNameEn = serializedObject.FindProperty("actorNameEn");
      m_actorNameCn = serializedObject.FindProperty("actorNameCn");
      //m_lastName = serializedObject.FindProperty("lastName");
      //m_nickName = serializedObject.FindProperty("nickName");
      //m_displayName = serializedObject.FindProperty("displayName");
      //m_icons = serializedObject.FindProperty("icons");
      //m_images = serializedObject.FindProperty("images");
      //m_refreshStoryData = serializedObject.FindProperty("refreshStoryData");
      //m_timelines = serializedObject.FindProperty("timelines");
      //m_places = serializedObject.FindProperty("appearances");
      //m_avatarChip = serializedObject.FindProperty("avatarChip");

      ActorPreset targetPreset = target as ActorPreset;
      //gather all existing actors from dialogue databases, currently all actor basic settings must be created from there
      //dialogueActors = StoryEditorHelper.GetUnassignActors(true);
      //TO-DO: MUST check whether this actor from dialogue database has been bound with anyother story actor, if yes, do not add to the list
      //actorOptions = dialogueActors.Select(r => r.Value).ToArray();

      //preset should only be set to addressable when initialized from script
      //therefore, when preset is not addressable, meaning it has never been initialized and saved to the database
      //if (AddressableEditorHelper.IsAssetAddressable(target)) {
      //  savedData = StoryEditorHelper.ExistingActor(m_actorID.intValue); //saved data should be loaded at this point
      //  if (savedData == null) warningNote = string.Format(EditorMessage.SAVE_INVALID, target.GetType().Name, "Unexpected error, Actor was set addressable but cannot be found from its record in existing actor list by its ID " + m_actorID.intValue);
      //}
    }
    private void updateData() {
      ActorPreset thisPreset = (ActorPreset)target;
      savedData = new ActorPresetData();
      savedData.ID = m_actorID.intValue;
      savedData.Guid = DialogueEditorHelper.GetActorGuid(m_actorID.intValue);

      savedData.DisplayNameEN = DialogueEditorHelper.GetActorName(m_actorID.intValue, GameSettings.GLOBAL_SETTING_LANGUAGE.ENGLISH);
      savedData.DisplayNameCNS = DialogueEditorHelper.GetActorName(m_actorID.intValue, GameSettings.GLOBAL_SETTING_LANGUAGE.简体中文);
      savedData.DisplayNameCNT = DialogueEditorHelper.GetActorName(m_actorID.intValue, GameSettings.GLOBAL_SETTING_LANGUAGE.繁體中文);

      //now check the data model
      //presetData.ID = m_actorID.intValue;
      //presetData.Guid = m_guid.stringValue;
      //presetData.FirstName = m_firstName.stringValue;
      //presetData.MiddleName = m_middleName.stringValue;
      //presetData.LastName = m_lastName.stringValue;
      //presetData.NickName = m_nickName.stringValue;

      //if (thisPreset.Copywritings != null) {
      //  List<string> copywritings = new List<string>();
      //  foreach (CopywritingPreset copywritingPreset in thisPreset.Copywritings) {
      //    string validateMsg = StoryEditorHelper.ValidateCopywriting(copywritingPreset);
      //    if (validateMsg.Length == 0) {
      //      copywritings.Add(copywritingPreset.GetAddressableAssetEntry().address);
      //    } else {
      //      validationMsg.Append(validateMsg + Environment.NewLine);
      //    }
      //  }
      //  presetData.CopywritingIDs = copywritings?.ToArray();
      //}

      //presetData.SlotIcon = new string[2] { thisPreset.Icons.Slot.AssetGUID, thisPreset.Icons.Slot.SubObjectName };
      //presetData.ConsoleIcon = new string[2] { thisPreset.Icons.Console.AssetGUID, thisPreset.Icons.Console.SubObjectName };
      //presetData.BarkIcon = new string[2] { thisPreset.Icons.Bark.AssetGUID, thisPreset.Icons.Bark.SubObjectName };
      //presetData.NotificationIcon = new string[2] { thisPreset.Icons.Notification.AssetGUID, thisPreset.Icons.Notification.SubObjectName };
      //presetData.BackgroundImage = new string[2] { thisPreset.Images.Background.AssetGUID, thisPreset.Images.Background.SubObjectName };
      //presetData.HeaderImage = new string[2] { thisPreset.Images.Header.AssetGUID, thisPreset.Images.Header.SubObjectName };
      //presetData.DocumentImage = new string[2] { thisPreset.Images.Document.AssetGUID, thisPreset.Images.Document.SubObjectName };
      //presetData.PortraitImage = new string[2] { thisPreset.Images.Portrait.AssetGUID, thisPreset.Images.Portrait.SubObjectName };
      //if (thisPreset.AvatarChip.ItemAssetFile != null) presetData.AvatarChipItemID = thisPreset.AvatarChip.ConvertToItemStack().ID.NumericID;
      //presetData.Appearances = new List<string>();
      //presetData.Timelines = new List<string>();

      //if (m_places.arraySize > 0) {
      //  for (int i = 0; i < m_places.arraySize; i++) {
      //    //find existing data by address name from array is not guaranteed
      //    PlaceData existingPlace = StoryEditorHelper.ExistingPlace(m_places.GetArrayElementAtIndex(i).stringValue);
      //    //theoratically, an existing place should be found by now, if not, there is a problem either the database needs refresh or the data is corrupted
      //    if (existingPlace != null) {
      //      presetData.Appearances.Add(existingPlace.ID);
      //    } else {
      //      validationMsg.Append("cannot found appearance data with addressable name " + m_places.GetArrayElementAtIndex(i).stringValue + ", try to refresh the story data and try it again" + Environment.NewLine);
      //    }
      //  }
      //}
      //if (m_timelines.arraySize > 0) {
      //  for (int i = 0; i < m_timelines.arraySize; i++) {
      //    //find existing data by address name from array is not guaranteed
      //    MomentData existingMoment = StoryEditorHelper.ExistingMoment(m_timelines.GetArrayElementAtIndex(i).stringValue);
      //    //theoratically, an existing place should be found by now, if not, there is a problem either the database needs refresh or the data is corrupted
      //    if (existingMoment != null) {
      //      presetData.Timelines.Add(existingMoment.ID);
      //    } else {
      //      validationMsg.Append("cannot found timeline data with addressable name " + m_places.GetArrayElementAtIndex(i).stringValue + ", try to refresh the story data and try it again" + Environment.NewLine);
      //    }
      //  }
      //}

      //validationMsg.Append(presetData.IsValid());
    }
    public override void OnInspectorGUI() {
      serializedObject.Update();
      EditorGUILayout.PropertyField(m_warning, GUIContent.none);
      EditorGUILayout.Space(5);
      EditorLayout.HorizontalLine(EditorLayout.COLOR_WARNING);
      EditorGUILayout.Space(5);

      //once this preset was initialized
      EditorGUILayout.PropertyField(m_actorID);
      EditorGUILayout.PropertyField(m_guid);
      //EditorGUILayout.PropertyField(m_selectedActorName);
      //namesFolded = !EditorGUILayout.BeginFoldoutHeaderGroup(namesFolded, "Actor Names");
      EditorGUILayout.PropertyField(m_actorNameEn);
      EditorGUILayout.PropertyField(m_actorNameCn);
      //EditorGUILayout.PropertyField(m_middleName);
      //EditorGUILayout.PropertyField(m_lastName);
      //EditorGUILayout.PropertyField(m_nickName);
      //EditorGUILayout.EndFoldoutHeaderGroup();
      EditorGUILayout.PropertyField(m_actorSettings);
      EditorGUILayout.PropertyField(m_actorResume);
      EditorGUILayout.PropertyField(m_actorRules);
      EditorGUILayout.PropertyField(m_responseRules);
      //EditorGUILayout.PropertyField(m_avatarChip);
      //EditorGUILayout.PropertyField(m_icons);
      //EditorGUILayout.PropertyField(m_images);
      //EditorGUILayout.PropertyField(m_refreshStoryData);
      //EditorGUILayout.PropertyField(m_timelines);
      //EditorGUILayout.PropertyField(m_places);

      EditorGUILayout.Space(10);

      if (GUILayout.Button("Update")) {
        updateData();

        m_actorID.intValue = savedData.ID;
        m_guid.stringValue = savedData.Guid;
        m_actorNameEn.stringValue = savedData.DisplayNameEN;
        m_actorNameCn.stringValue = savedData.DisplayNameCNS;

        //if (validationMsg.Length == 0) {
        //  if (setAddressable()) {
        //    string errorMsg = StoryEditorHelper.Save(presetData);
        //    saveNote = errorMsg.Length > 0 ? string.Format(EditorMessage.SAVE_FAILED, target.GetType().Name, errorMsg) : string.Format(EditorMessage.SAVE_SUCCESS, target.GetType().Name, m_guid.stringValue);
        //  }
        //} else {
        //  warningNote = validationMsg.ToString();
        //}
      }


      EditorGUILayout.Space(10);
      //if (savedData != null) {
      //  if (GUILayout.Button("Delete from Database", EditorStyles.toolbarButton)) {
      //    warningNote = string.Format(EditorMessage.FUNCTION_NOT_IMPLEMENT, "Deleteing", "DO NOT delete the scriptable object file directly");
      //  }
      //}
      //print the saving msg below the button
      EditorGUILayout.Space(20);
      GUILayout.Label(saveNote);
      EditorGUILayout.Space(10);
      GUILayout.Label(warningNote, EditorLayout.TEXT_RED);

      serializedObject.ApplyModifiedProperties();
    }
    //private bool setAddressable() {
    //  updateData();
    //  var entry = AddressableEditorHelper.CreateAssetEntry(target, Halabang.GameSettings.ADDRESSABLE_GROUP_STORY_ACTOR, StoryHelper.GetActorUniqueName(presetData));
    //  if (entry == null) saveNote = string.Format(EditorMessage.SAVE_FAILED, target.GetType().Name, "Failed to set this preset addressable");
    //  return entry != null;
    //}
  }
}
