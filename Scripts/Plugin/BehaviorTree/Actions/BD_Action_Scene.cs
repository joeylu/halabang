using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using Halabang.Game;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("")]
  public class BD_Action_Scene : Action {
    public enum ACTION_NAME {
      NULL,
      AUTOSAVE,
      SWITCH_SCENE_SINGLE,
      LOAD_SCENE_ADD,
      LOAD_SCENE_MINIGAME,
      UNLOAD_SCENE_ADD,
      UNLOAD_SCENE_MINIGAME,
    }

    public ACTION_NAME triggerAction;
    public SaveLoadManager.SCENE_NAME_SINGLE targetSwitchScene;
    //public SaveLoadManager.SCENE_NAME_ADDITIVE_STORY targetAdditiveStoryScene;
    public SaveLoadManager.SCENE_NAME_ADDITIVE_MINIGAME targetAdditiveMiniGameScene;


    public override void OnStart() {
      callAction();
    }
    public override TaskStatus OnUpdate() {
      return TaskStatus.Success;
    }

    private void callAction() { 
      switch (triggerAction) {
        case ACTION_NAME.SWITCH_SCENE_SINGLE:
          GameManager.Instance._SaveLoadManager.LoadSceneSingle(targetSwitchScene);
          break;
        case ACTION_NAME.LOAD_SCENE_ADD:
          //GameManager.Instance._SaveLoadManager.LoadSceneAdditive(targetAdditiveStoryScene);
          break;
        case ACTION_NAME.LOAD_SCENE_MINIGAME:
          GameManager.Instance._SaveLoadManager.LoadSceneAdditive(targetAdditiveMiniGameScene);
          break;
        case ACTION_NAME.UNLOAD_SCENE_ADD:
          //GameManager.Instance._SaveLoadManager.UnloadSceneAdditive(targetAdditiveStoryScene);
          break;
        case ACTION_NAME.UNLOAD_SCENE_MINIGAME:
          GameManager.Instance._SaveLoadManager.UnloadMiniGameScene();
          break;
      }    
    }
  }
}
