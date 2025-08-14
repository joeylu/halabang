using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Halabang.Scenes;
using Halabang.Audio;
using Halabang.Game;
using Halabang.Utilities;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;

namespace Halabang.Plugin {
  public class SequencerCommandTransition : SequencerCommand {
    //private Story.Actor currentActor;
    private bool hasStopped; //a trigger indicate this sequence has been stopped successfully (fully played), therefore, destroy needn't call the safety trigger
    private AudioRequestor voiceRequestor;
    private bool isChained; //indicate whether this tranistion is chained from last transition (if any)

    private void Start() {
      //Parameter(0) is a float of exit duration seconds, started after all voice is played
      //Parameter(1) is a string of voice address, when presented, it will be played fully before stop

      //In transition, player should not be interrupt it at all
      //TO-DO: calling pause input on player might not be possible as it might be already paused, maybe a more global call to disable all input method is needed to implement

      //var actorGuid = DialogueHelper.GetActorFromDialogueEntry(DialogueManager.CurrentConversationState.subtitle.dialogueEntry, false); //it is an avatar action, it assumes that current dialogue entry actor is the avatar actor
      //currentActor = GameManager.instatnce.CurrentCharacterManager.GetActor(actorGuid);
      //terminate sequence if actor cannot be found
      //if (currentActor == null) {
      //  Debug.LogError("Cannot sync middle text with actor when actor is null");
      //  Stop();
      //}
      //terminate sequence if parameters are missing
      if (GetParameter(0) == null || GetParameterAsFloat(0) < 0) {
        Debug.LogError("Dialogue Sequence Transition requires a zero or positive float for exiting duration");
        Stop();
      }

      startTransition();
    }
    public void OnDestroy() {
      if (hasStopped == false) stopTransition(true);
    }

    private void startTransition() {
      //enter a specific transition type (i.e. black screen)
      StartCoroutine(startTransitionSequence());
    }
    private IEnumerator startTransitionSequence() {
      PixelCrushers.DialogueSystem.DialogueManager.instance.SetDialoguePanel(false, true); //while in transition, subtitle should be hidden
      //Debug.Log(currentActor.FullName + " started");
      GameManager.Instance._UIManager.ShowMiddleText(PixelCrushers.DialogueSystem.DialogueManager.CurrentConversationState.subtitle.dialogueEntry.subtitleText);
      yield return new WaitForSeconds(GameSetting.STANDARD_DURATION);
      //if voice address is presented, generate an audio requestor and play it
      if (string.IsNullOrWhiteSpace(GetParameter(1)) == false) {
        //play voice
        yield return StartCoroutine(GameManager.Instance._SoundManager.PlayVoiceSequence(GetParameter(1)));
        //wait one frame to allow actor voice call in AudioController is triggered
        yield return null;
        //finally, wait until voice is fully played, then fade out text
        //Debug.Log("xxxxxxxxxx " + voiceRequestor.AddressableName + " : " + voiceRequestor.RemainingPlayableTime + " > " + voiceRequestor.IsPlaying);
      }
      //Debug.Log("end of current transition voice");
      //complete current transitions
      stopTransition(false);
      //wait fixed exit duration toward next dialogue entry
      yield return new WaitForSeconds(GameSetting.STANDARD_DURATION);
      //wait dynaminc exit duration toward next dialogue entry
      yield return new WaitForSeconds(GetParameterAsFloat(0));
      yield return null;

      //DialogueManager.instance.SetDialoguePanel(true, true);
      Stop();
    }
    private void stopTransition(bool forceExit) {
      hasStopped = true;
      //Debug.Log(DialogueManager.CurrentConversationState.subtitle.dialogueEntry.conversationID + " : " + DialogueManager.CurrentConversationState?.subtitle.dialogueEntry.id + " stopped");
      GameManager.Instance._UIManager.HideMiddleText();
      //reset voice handler
      if (forceExit && string.IsNullOrWhiteSpace(GetParameter(1)) == false) {
        GameManager.Instance._SoundManager.StopVoice(GetParameter(0));
        voiceRequestor = null;
      }
    }
  }
}