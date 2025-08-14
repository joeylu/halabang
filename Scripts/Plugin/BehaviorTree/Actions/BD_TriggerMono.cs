using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace Halabang.Plugin {
  [TaskCategory("Halabang")]
  [TaskDescription("Call Mono behavior related methods once")]
  public class BD_TriggerMono : Action {
    public enum ACTION_NAME {
      //CAUTION: DO NOT RE-SORT, used in serialized field
      NULL,
      SKIP_UPDATE, //use int32 value for how many update cycle should be skipped, default is 1
      SKIP_FIXED_UPDATE, //use int32 value for how many fixed update cycle should be skipped, default is 1
    }
    public ACTION_NAME triggerAction;
    public int targetInt;
    //public ValueCollection valueParameter;

    private int counterOnUpdate;  //when greater than zero, start counting
    private int counterOnFixedUpdate; //when greater than zero, start counting, when set to -1, allow OnUpadate() to return success

    public override void OnAwake() {
      if (triggerAction == ACTION_NAME.NULL) Debug.LogError(FriendlyName + " must assign an action");
    }
    public override void OnStart() {
      callAction();
    }
    public override TaskStatus OnUpdate() {
      if (triggerAction == ACTION_NAME.SKIP_UPDATE || triggerAction == ACTION_NAME.SKIP_FIXED_UPDATE) {
        if (counterOnFixedUpdate < 0) return TaskStatus.Success; //since OnFixedUpdate() does not return TaskStatus, use -1 as success here, by default, it should be 0 or greater than 0 when running

        if (counterOnUpdate == 0) return TaskStatus.Inactive;
        if (counterOnUpdate > 0f) {
          counterOnUpdate += 1;
        }
        if (targetInt <= 0 && counterOnUpdate > 1) return TaskStatus.Success;
        if (targetInt > 0 && counterOnUpdate > targetInt) return TaskStatus.Success;
        return TaskStatus.Running;
      } else {
        //for none timer related action, simply return success
        return TaskStatus.Success;
      }
    }
    public override void OnFixedUpdate() {
      if (triggerAction == ACTION_NAME.SKIP_FIXED_UPDATE) {
        if (counterOnFixedUpdate == 0) return; //do nothing, no fixed update timer relatec action has triggered
        if (counterOnFixedUpdate < 0) return; //already finished waiting, let onUpdate to handle

        if (counterOnFixedUpdate > 0f) {
          counterOnFixedUpdate += 1;
        }
        if (targetInt <= 0 && counterOnFixedUpdate > 1) counterOnFixedUpdate = -1;
        if (targetInt > 0 && counterOnFixedUpdate > targetInt) counterOnFixedUpdate = -1;
      }
    }

    private void callAction() {
      counterOnUpdate = 0;
      counterOnFixedUpdate = 0;

      if (triggerAction != ACTION_NAME.NULL) {
        switch (triggerAction) {
          case ACTION_NAME.SKIP_UPDATE:
            counterOnUpdate = 1;
            break;
          case ACTION_NAME.SKIP_FIXED_UPDATE:
            counterOnFixedUpdate = 1;
            break;
        }
      }
    }
  }
}
