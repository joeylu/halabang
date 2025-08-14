using Halabang.Game;
using Halabang.Plugin;
using UnityEngine;
using Halabang.Utilities;
using PixelCrushers.DialogueSystem;
using System.Collections.Generic;

namespace Halabang.Plugin {
  public class DialogueManager : MonoBehaviour {
    public DialogueTriggerController CurrentDialogueController { get; private set; }

    [Header("开发者选项")]
    [SerializeField] public bool enableDebugger;

    private DialogueEntry currentEntry; //cache each entry when a conversation is started

    private void Start() {
      //GameManager.instatnce.CurrentSaveLoadManager.GameSettingsHolder.OnLanguageChange.AddListener(onLanguageChanged);
      //defaultResponseTimeout = dialogueSystemController.displaySettings.inputSettings.responseTimeout;
      PixelCrushers.DialogueSystem.DialogueManager.instance.SetLanguage(GameManager.Instance._SaveLoadManager.CurrentGamePreference.CurrentGameSetting.DefaultLanguage.ToString());
    }

    public void TriggerDialogue(DialogueTriggerController trigger, float delay = 0) {
      CurrentDialogueController = trigger;
      CurrentDialogueController.Trigger.OnUse();
    }
    public void StopDialogue() {
      PixelCrushers.DialogueSystem.DialogueManager.instance.StopConversation();
    }

    private void onLanguageChanged() {
      //Debug.Log("Current language for dialogue system has changed to " + GameManager.instatnce.CurrentPreferences.CurrentLanguage.Description());
      //PixelCrushers.DialogueSystem.DialogueManager.instance.SetLanguage(GameManager.Instance.CurrentPreferences.CurrentLanguage.Description());
      PixelCrushers.DialogueSystem.DialogueManager.instance.SetLanguage(GameSettings.GLOBAL_SETTING_LANGUAGE.简体中文.Description().ToString());
    }

    #region DIALOGUE_SYSTEM_METHODS
    private void OnPrepareConversationLine(DialogueEntry entry) {
      currentEntry = entry;
    }
    private void OnConversationStart() {
      //if (CurrentDialogueController.Settings.MenuOnly) DialogueManager.instance.displaySettings.inputSettings.responseTimeout = 0f;
      //dialogue
      //Debug.Log(DialogueManager.instance.lastConversationStarted + " ----- ");
      //triggerControllers.ForEach(r => Debug.Log(r.dialogueTriggerSetting.Trigger.conversation));
      //no movement if set
      //if (currentDialogueController.Settings.noMovementInputInConversation) {
      //  GameManager.instatnce.CurrentSceneManager.PlayerController.LockMovement();
      //}
      //detect whether or not a response menu
      //if (currentDialogueController.Settings.MenuOnly == false) {
      //  //sett audio mixer to dialogue environment
      //  GameManager.instatnce.CurrentAudioManager.TransitSnapshotDialogue();
      //}
      //set cursor on dialogue
      //GameManager.instatnce.CurrentCursorManager.ActivateCursor(transform, true);
      CurrentDialogueController.IsTriggering = true;
      CurrentDialogueController.OnConversationStart.Invoke();
    }
    private void OnConversationLine(Subtitle subtitle) {
      //DialogueManager.conversationView.displaySettings.inputSettings.responseTimeout = DialogueManager.instance.displaySettings.GetResponseTimeout();

      //if (currentActors == null) currentActors = new List<Story.Actor>();

      //if (subtitle.dialogueEntry.id > 0) {
      //  //if current dialogue node actor is this actor, act as a talker
      //  dialogueEntryActor = GameManager.instatnce.CurrentCharacterManager.Actors.Where(r => r.ID == subtitle.dialogueEntry.ActorID).FirstOrDefault();
      //  //Debug.Log(subtitle.dialogueEntry.ActorID + " : " + dialogueEntryActor?.DisplayName);
      //  if (dialogueEntryActor != null) {
      //    if (!currentActors.Any(r => r.ID == dialogueEntryActor.ID)) {
      //      currentActors.Add(dialogueEntryActor);
      //    }
      //    if (currentDialogueController.Settings.MenuOnly == false && currentDialogueController.Settings.Stateless == false && dialogueEntryActor.ActingAbility) dialogueEntryActor.ActingAbility.Speak(); //trigger speaking logics if it is a character
      //  }
      //  //if current dialogue node conversant is this actor, act as a listener
      //  dialogueEntryConversant = GameManager.instatnce.CurrentCharacterManager.Actors.Where(r => r.ID == subtitle.dialogueEntry.ConversantID).FirstOrDefault();
      //  if (dialogueEntryConversant != null) {
      //    if (!currentActors.Any(r => r.ID == dialogueEntryConversant.ID)) {
      //      currentActors.Add(dialogueEntryConversant);
      //    }
      //    if (currentDialogueController.Settings.MenuOnly == false && currentDialogueController.Settings.Stateless == false && dialogueEntryConversant?.ActingAbility) dialogueEntryConversant.ActingAbility.Listen(); //trigger listening logics if it is a character
      //  }
      //}
      //Debug.Log("line: " + currentActors.Count);

      //logics when random next entry is presented
      //Dialogue system has its own randomNextEntry() but only works in NPC entry, TO-DO: check dialogue system native solution
      //if (Field.LookupBool(subtitle.dialogueEntry.fields, GameSettings.DIALOGUE_KEYWORD_RANDOM_NEXT_ENTRY) == true) {
      //  if (DialogueManager.currentConversationState.pcResponses.Length > 1) { //the next responses are from player, and has more than 1 responses
      //    System.Random rnd = new System.Random();
      //    DialogueManager.currentConversationState.pcResponses = DialogueManager.currentConversationState.pcResponses.OrderBy(x => rnd.Next()).ToArray();
      //    for (int i = 0; i < DialogueManager.currentConversationState.pcResponses.Length; i++) {
      //      DialogueManager.currentConversationState.pcResponses[i].formattedText.forceAuto = i == 0;
      //    }
      //  } else if (DialogueManager.currentConversationState.npcResponses.Length > 1) { //the next responses are from NPC, and has more than 1 responses
      //    System.Random rnd = new System.Random();
      //    DialogueManager.currentConversationState.npcResponses = DialogueManager.currentConversationState.npcResponses.OrderBy(x => rnd.Next()).ToArray();
      //  }
      //}
    }
    private void OnConversationLineEnd(Subtitle subtitle) {
      //if (Field.LookupBool(subtitle.dialogueEntry.fields, GameSettings.DIALOGUE_KEYWORD_MENU_HIDDEN_TRIGGER) == true) {
      //  //Debug.Log("this is a turned off hidden link, turning it back on");
      //  subtitle.dialogueEntry.conditionsString = FALSE;
      //}

      currentEntry = null;
    }
    private void OnConversationLineCancelled(Subtitle subtitle) {
      currentEntry = null;
    }
    private void OnConversationCancelled(Transform actor) {
      conversationFinish();
    }
    private void OnConversationEnd() {
      conversationFinish();
    }
    private void conversationFinish() {
      //if (currentActors != null) {
      //  foreach (Story.Actor actor in currentActors) {
      //    actor.ActingAbility?.StopTalking(); //trigge conversation stopping logics if it is a character, talk acts only uses upper body
      //    GameManager.instatnce.CurrentStoryManager.SetActorUncovered(actor.ID); //set this actor to known actor to player
      //  }
      //}

      //if (currentDialogueController.Settings.MenuOnly) DialogueManager.instance.displaySettings.inputSettings.responseTimeout = defaultResponseTimeout;
      onConversationEndSequence();
    }
    private void onConversationEndSequence() {
      if (CurrentDialogueController == null) {
        Debug.LogError("currentDialogueController is null while trying to end conversation");
        return;
      }
      //voice has already stopped, resume audios first
      //GameManager.instatnce.CurrentAudioManager.TransitSnapshotDefault();
      //if (currentDialogueController.Settings.endExaminAfterConversation) {
      //  if (GameManager.instatnce.CurrentSceneManager.CurrentPlayer._CharacterExamin.IsExamining) {
      //    GameManager.instatnce.CurrentSceneManager.PlayerController.TriggerExamin(false);
      //  }
      //}
      //if (currentDialogueController.Settings.noMovementInputInConversation) {
      //  GameManager.instatnce.CurrentSceneManager.PlayerController.ResumeMovement();
      //}
      for (int i = 0; i < CurrentDialogueController.OnConversationEnd.GetPersistentEventCount(); i++) {
        if (CurrentDialogueController.OnConversationEnd.GetPersistentMethodName(i).Contains(nameof(CurrentDialogueController.OnUse))) {
          Debug.LogError("A dialogue controller end conversation event cannot call another dialogue controller OnUse because it will keep the current lastConversationStarted name and in an infinity loop");
        }
      }
      CurrentDialogueController.OnConversationEnd.Invoke();
      CurrentDialogueController.Clear();
      CurrentDialogueController = null;
      //Debug.Log("end of conversation");
      //currentActors = null;
      //finally, hide dialogue cursor
      //GameManager.instatnce.CurrentCursorManager.ActivateCursor(transform, false);

    }
    #endregion
  }
}