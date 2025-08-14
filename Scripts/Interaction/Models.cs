using UnityEngine;
using System;
using UnityEngine.Events;

namespace Halabang.Interactive {
  [Serializable]
  public class TriggerEvents {
    public UnityEvent onReactStart;
    public UnityEvent onCollisionStart;
    public UnityEvent onReactStop;
  }
  [Serializable]
  public class CheckingEvents {
    public UnityEvent onChecked;
  }
}