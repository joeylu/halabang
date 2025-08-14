using UnityEngine;

namespace Halabang.Interactive {
  public class Checkpoint : ProximityTrigger {
    public bool IsChecked { get; protected set; }

    [Header("设定")]
    public bool checkedOnReact;
    public bool disabledOnChecked;
    [Header("检查点事件触发")]
    public CheckingEvents checkingEvents;

    public void SetChecked() {
      if (IsChecked) return;

      IsChecked = true;
      checkingEvents.onChecked.Invoke();
      if (disabledOnChecked) gameObject.SetActive(false);
    }

    protected override void StartReact(Collider collider, Collider2D collider2D) {
      if (IsChecked) return;

      base.StartReact(collider, collider2D);

      if (checkedOnReact) SetChecked();
    }
  }
}
