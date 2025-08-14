using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Halabang.UI {
  public class PointerEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {
    public UnityEvent onPointerEnter;
    public UnityEvent onPointerExit;
    public UnityEvent onPointerDown;
    public UnityEvent onPointerUp;
    public UnityEvent onPointerClick;

    [SerializeField] private bool enableDebugger;

    public void OnPointerClick(PointerEventData eventData) {
      onPointerClick.Invoke();
      if (enableDebugger) Debug.Log(name + " is clicked");
    }

    public void OnPointerEnter(PointerEventData eventData) {
      onPointerEnter.Invoke();
      if (enableDebugger) Debug.Log(name + " is enter");
    }

    public void OnPointerExit(PointerEventData eventData) {
      onPointerExit.Invoke();
      if (enableDebugger) Debug.Log(name + " is exit");
    }
    public void OnPointerDown(PointerEventData eventData) {
      onPointerDown.Invoke();
      if (enableDebugger) Debug.Log(name + " is down");
    }

    public void OnPointerUp(PointerEventData eventData) {
      onPointerUp.Invoke();
      if (enableDebugger) Debug.Log(name + " is up");
    }
  }
}
