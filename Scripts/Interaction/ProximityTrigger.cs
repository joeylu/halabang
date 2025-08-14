using UnityEngine;

namespace Halabang.Interactive {
  public class ProximityTrigger : MonoBehaviour {
    [Header("基础设定")]
    [Tooltip("是否为2D触发器")]
    public bool is2DTrigger;
    [Header("事件触发")]
    public TriggerEvents triggerEvents;
    [Header("开发者")]
    public bool enableDebugger;

    protected Collider currentInteractor;
    protected Collider2D currentInteractor2D;

    protected virtual void OnTriggerEnter(Collider collider) {
      if (enableDebugger) Debug.Log(name + " 进入碰撞 > " + collider.name);
      StartReact(collider, null);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collider2D) {
      if (enableDebugger) Debug.Log(name + " 进入碰撞2D > " + collider2D.name);
      StartReact(null, collider2D);
    }
    protected virtual void OnTriggerExit(Collider collider) {
      if (enableDebugger) Debug.Log(name + " 离开碰撞 > " + collider.name);
      if (collider == currentInteractor) StopReact();
    }
    protected virtual void OnTriggerExit2D(Collider2D collider2D) {
      if (enableDebugger) Debug.Log(name + " 离开碰撞2D > " + collider2D.name);
      if (collider2D == currentInteractor2D) StopReact();
    }

    protected virtual void StartReact(Collider collider, Collider2D collider2D) {
      if (enableDebugger) Debug.Log(name + " 触发开始 > " + (is2DTrigger ? collider2D : collider.name));
      currentInteractor = collider;
      currentInteractor2D = collider2D;
      triggerEvents.onReactStart.Invoke();
    }
    protected virtual void StopReact() {
      if (enableDebugger) Debug.Log(name + " 触发结束 > " + (is2DTrigger ? currentInteractor2D.name : currentInteractor.name));
      triggerEvents.onReactStop.Invoke();
    }
  }
}