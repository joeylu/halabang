using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Halabang.Scene;
using Halabang.Audio;
using Halabang.Game;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;

namespace Halabang.Plugin {
  public class SequencerCommandBark : SequencerCommand {
    private DialogueActor currentActor;
    private TextMeshExtend inSceneTextComponent;
    AudioRequestor voiceRequestor;
    private float textDuration;
    private void Awake() {
      //Parameter 0 (float) to override life cycle for target text component, -1 require additional script to hide text manually, NOTE: not for this dialogue entry, bark entry should always comes with Continue() for instant bypass
      //field name NAME for text component name, if not presented, use the first one from current dialogue trigger
      //field name VOICE for additional voice play

      if (GetParameterAsFloat(0) != 0) {
        textDuration = GetParameterAsFloat(1);
      }
      StartCoroutine(playActorBark());
    }
    public void OnDestroy() {
      // Add your finalization code here. This is critical. If the sequence is cancelled and this
      // command is marked as "required", then only Awake() and OnDestroy() will be called.
      // Use it to clean up whatever needs cleaning at the end of the sequencer command.
      // If you don't need to do anything at the end, you can delete this method.
    }

    private IEnumerator playActorBark() {
      PixelCrushers.DialogueSystem.DialogueManager.instance.SetDialoguePanel(false, true); //while in transition, subtitle should be hidden
      //currentActor = GameManager.Instance._DialogueManger.CurrentDialogueController.GetDialogueActor(DialogueManager.CurrentConversationState.subtitle.speakerInfo.transform, DialogueManager.CurrentConversationState.subtitle.speakerInfo.Name);
      //if (currentActor == null) return;

      //IN SCENE TEXT
      //play text animation
      DialogueEntry currentEntry = PixelCrushers.DialogueSystem.DialogueManager.currentConversationState.subtitle.dialogueEntry;
      string targetTextName = Field.LookupValue(currentEntry.fields, DialogueSystemDictionary.FIELD_NAME_VALUE_NAME);
      inSceneTextComponent = GameManager.Instance._DialogueManger.CurrentDialogueController.GetInSceneTextComponent(targetTextName);
      string targetDialogueText = string.IsNullOrWhiteSpace(currentEntry.currentLocalizedMenuText) ? currentEntry.currentLocalizedDialogueText : currentEntry.currentLocalizedMenuText;
      inSceneTextComponent.TriggerText(targetDialogueText, textDuration);
      //play voice if any, this voice should not be waited until it is finished playing, the total wait duration should be fixed from timeout
      string targetVoiceAddress = Field.LookupValue(currentEntry.fields, DialogueSystemDictionary.FIELD_NAME_VALUE_VOICE);
      //Debug.Log("002 " + currentEntry.currentLocalizedMenuText + " : " + currentEntry.currentLocalizedDialogueText + " : " + inSceneTextComponent + " >>> " + targetDialogueText + " > " + targetVoiceAddress);
      //if voice address is presented, generate an audio requestor and play it
      if (string.IsNullOrWhiteSpace(targetVoiceAddress) == false) {
        //play voice
        yield return StartCoroutine(GameManager.Instance._SoundManager.PlayVoiceSequence(targetVoiceAddress));
        //wait one frame to allow actor voice call in AudioController is triggered
        yield return null;
        yield return new WaitForSeconds(1f);
      } else {
        float duration = targetDialogueText.Length / PixelCrushers.DialogueSystem.DialogueManager.instance.displaySettings.subtitleSettings.subtitleCharsPerSecond;
        yield return new WaitForSeconds(duration);
        yield return new WaitForSeconds(1f);
      }
      Stop();
    }
  }
}
