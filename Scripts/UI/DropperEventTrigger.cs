using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Halabang.UI {
  public class DropperEventTrigger : MonoBehaviour, IDropHandler {
    public UnityEvent onDropped;

    [SerializeField] private bool enableDebugger;

    public void OnDrop(PointerEventData eventData) {
      if (enableDebugger) Debug.Log(eventData.pointerDrag + " is dropped on " + name);
    }
  }
}