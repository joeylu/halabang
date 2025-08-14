using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Halabang.UI {
  public class DraggerEventTrigger : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    [Header("设置")]
    [SerializeField] private Image draggableTarget;
    [Tooltip("拖拽过程中，暂时性关闭Image的Raycast")]
    [SerializeField] private bool blockRaycast = true;
    [Header("事件")]
    //public UnityEvent onDragging;
    public UnityEvent onDragStart;
    public UnityEvent onDragEnd;
    public UnityEvent onDropped;

    [SerializeField] private bool enableDebugger;

    private Canvas parentCanvase;
    private Image currentDraggableTarget;

    private void Awake() {
      if (draggableTarget == null) currentDraggableTarget = GetComponent<Image>(); ;
      parentCanvase = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
      currentDraggableTarget.raycastTarget = false;
      onDragStart.Invoke();
      if (enableDebugger) Debug.Log(name + " is began drag");
    }

    public void OnDrag(PointerEventData eventData) {
      if (parentCanvase.renderMode == RenderMode.ScreenSpaceOverlay) {
        currentDraggableTarget.transform.position = eventData.position;
      } else {
        currentDraggableTarget.rectTransform.anchoredPosition = eventData.position;
        Debug.Log(currentDraggableTarget.rectTransform.anchoredPosition + " : " + eventData.position);
      }
      if (enableDebugger) Debug.Log(name + " is dragging");
    }
    public void OnEndDrag(PointerEventData eventData) {
      currentDraggableTarget.raycastTarget = true;
      onDragEnd.Invoke();
      if (enableDebugger) Debug.Log(name + " is ended drag");
    }
  }
}