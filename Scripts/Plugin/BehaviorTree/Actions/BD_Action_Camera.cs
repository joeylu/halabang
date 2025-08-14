using UnityEngine;
using UnityEngine.Rendering;
using Halabang.Scene;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using Unity.Cinemachine;
using Halabang.Game;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("Call unity cinemachine actions, this action always return success after called")]
  public class BD_Action_Camera : Action {
    public enum ACTION_NAME {
      NULL,
      ACTIVATE_CINEMACHINE,
      SHAKE,
    }


    [Header("Action")]
    public ACTION_NAME triiggerAction;

    [Header("Shared references")]
    public CinemachineCamera targetCinemachnie;
    public CameraShakePreset shakeSetting;


    public override void OnAwake() {
    }

    public override void OnStart() {
      callAction();
    }

    public override TaskStatus OnUpdate() {
      return TaskStatus.Success;
    }


    private void callAction() {
      switch (triiggerAction) {
        case ACTION_NAME.ACTIVATE_CINEMACHINE:
          if (targetCinemachnie == null) {
            Debug.LogError("Target cinemachine to be activated cannot benull");
            return;
          }
          GameManager.Instance.CurrentSceneManager.CurrentCameraManager.ActivateCinemachine(targetCinemachnie);
          break;
        case ACTION_NAME.SHAKE:
          if (shakeSetting == null) {
            Debug.LogError("Shake setting cannot be null");
            return;
          }
          GameManager.Instance.CurrentSceneManager.CurrentCameraManager.CameraShake(shakeSetting);
          break;
        default:
          Debug.LogError("Cinemachine action is null");
          break;
      }
    }
  }
}