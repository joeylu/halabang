using UnityEngine;
using System.Collections;
using System.Linq;
//using Halabang.Scenes;
using Halabang.Audio;
using Halabang.Game;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;
using Halabang.Utilities;
//using static Halabang.Scenes.DebugHelper;

namespace Halabang.Plugin {
  public class SequencerCommandVoiceWait : SequencerCommand {
    private DialogueActor currentActor;
    private bool hasStopped; //a trigger indicate this sequence has been stopped successfully (fully played), therefore, destroy needn't call the safety trigger
    private AudioRequestor voiceRequestor;

    private void Awake() {
      // Add your initialization code here. You can use the GetParameter***() and GetSubject()
      // functions to get information from the command's parameters. You can also use the
      // Sequencer property to access the SequencerCamera, CameraAngle, Speaker, Listener,
      // SubtitleEndTime, and other properties on the sequencer. If IsAudioMuted() is true, 
      // the player has muted audio.
      //
      // If your sequencer command only does something immediately and then finishes,
      // you can call Stop() here and remove the Update() method:
      //
      // Stop();
      //
      // If you want to use a coroutine, use a Start() method in place of or in addition to this method.

      //Parameter 0 (string) is addressable name of the voice
      //Parameter 1 (float)
      //  show/hide a scene text, -1 hide, 0 show, >0 duration
      //  text object name from Name field, text value is from menu text
      //  if not found in scene, an virtual text object will be created along with local position/rotation from the field
      if (GetParameter(0) == null) {
        Debug.LogError("Dialogue Sequence VoiceWait requires an addressable name to play");
        Stop();
      }
      StartCoroutine(playVoiceSequence());
    }

    //public void Update() {
    //  // Add any update code here. When the command is done, call Stop().
    //  // If you've called stop above in Awake(), you can delete this method.
    //}

    public void OnDestroy() {
      // Add your finalization code here. This is critical. If the sequence is cancelled and this
      // command is marked as "required", then only Awake() and OnDestroy() will be called.
      // Use it to clean up whatever needs cleaning at the end of the sequencer command.
      // If you don't need to do anything at the end, you can delete this method.
      if (hasStopped == false) stopVoice();
    }

    private IEnumerator playVoiceSequence() {
      //currentActor = GameManager.Instance.CurrentSceneManager.CurrentDialogueController.GetDialogueActor(DialogueManager.CurrentConversationState.subtitle.speakerInfo.transform, DialogueManager.CurrentConversationState.subtitle.speakerInfo.Name);     
      //Debug.Log(GetParameter(0));
      //AUDIO
      //generate an audio requestor
      //play voice
      yield return StartCoroutine(GameManager.Instance._SoundManager.PlayVoiceSequence(GetParameter(0)));
      yield return null;
      yield return new WaitForSeconds(GameSetting.STANDARD_DURATION);
      stopVoice();
    }
    private void stopVoice() {
      //if (voiceRequestor.IsPlaying) {
      //  GameManager.Instance._SoundManager.StopVoice(GetParameter(0));
      //}
      //voiceRequestor = null;
      //hasStopped = true;
      Stop();
    }
  }
}
