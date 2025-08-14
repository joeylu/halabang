using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Halabang.Scenes;
using Halabang.Audio;
using Halabang.Game;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;
//using static Halabang.Scenes.DebugHelper;
using BehaviorDesigner.Runtime;
using System.Linq;

namespace Halabang.Plugin {
  public class SequencerCommandScene : SequencerCommand {
    private float delay;
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

      //USE field name for different scene commands
      //Parameter 0 (float) a delay in second to trigger scene related logics
      delay = 0;
      if (GetParameter(0) != null && GetParameterAsFloat(0) > 0) delay = GetParameterAsFloat(0);

      triggerSceneCommand();
    }

    private void triggerSceneCommand() {
      DialogueEntry currentEntry = PixelCrushers.DialogueSystem.DialogueManager.currentConversationState.subtitle.dialogueEntry;
      string targetMusicAddress = Field.LookupValue(currentEntry.fields, DialogueSystemDictionary.FIELD_NAME_VALUE_MUSIC);
      string targetBehaviorName = Field.LookupValue(currentEntry.fields, DialogueSystemDictionary.FIELD_NAME_VALUE_BEHAVIOR);
      if (string.IsNullOrWhiteSpace(targetMusicAddress) == false) {
        GameManager.Instance._SoundManager.PlayMusic(targetMusicAddress, delay);
      }
      Debug.Log("001 " + targetBehaviorName);
      if (string.IsNullOrWhiteSpace(targetBehaviorName) == false) {
        var allresults = GameObject.FindObjectsByType<BehaviorTree>(FindObjectsSortMode.None);
        Debug.Log("002 " + allresults);
        if (allresults != null && allresults.Length > 0) {
          BehaviorTree behaviorTree = allresults.ToList().Where(r => r.name.Equals(targetBehaviorName)).FirstOrDefault();
          Debug.Log("003 " + behaviorTree);
          if (behaviorTree) behaviorTree.EnableBehavior();
        } 
      }

      Stop();
    }
  }
}