using UnityEngine;
using Michsky.MUIP;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Halabang.Plugin {
  public class ButtonManagerExt : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public enum BUTTON_STATE {
      Normal,  //常态
      Enter,  //悬停
      Press, 
      Release,
      Disabled
    }

    public UnityEvent onClick; //当鼠标点击发生的事件
    public UnityEvent onHover; //当鼠标悬停发生的事件
    public UnityEvent onLeave; //当鼠标悬停离开发生的事件
    public UnityEvent onPressed; //当鼠标按下时发生的事件
    public UnityEvent onReleased; //当鼠标放掉时发生的事件
    [Header("开发者选项")]
    public bool enableDebugger;
    public ButtonManager TargetButton { get; private set; }

    private Animator animator;

    private void Awake() {
      TargetButton = GetComponent<ButtonManager>();
      if (TargetButton == null) Debug.LogError(name + " 没有获得对应的 Button Manager 组件");
      animator = GetComponent<Animator>(); //尝试获取animator，若Null，则按钮使用的是 script based

      setNormal();

      TargetButton.onClick.AddListener(onPointerClick);
      TargetButton.onHover.AddListener(onPointerHover);
      TargetButton.onLeave.AddListener(onPointerLeave);
    }

    public void OnPointerDown(PointerEventData eventData) {
      if (enableDebugger) Debug.Log(name + " is down");
      onPressed.Invoke();
    }
    public void OnPointerUp(PointerEventData eventData) {
      if (enableDebugger) Debug.Log(name + " is up");
      onReleased.Invoke();
    }

    private void onPointerHover() {
      if (enableDebugger) Debug.Log(name + " is hover");
      if (animator == null) return; ;

      TargetButton.disabledCG.alpha = 0;
      TargetButton.normalCG.alpha = 0;
      TargetButton.highlightCG.alpha = 1;

      animator.SetBool(BUTTON_STATE.Enter.ToString(), true);
      animator.SetBool(BUTTON_STATE.Normal.ToString(), false);

      onHover.Invoke();
    }
    private void onPointerLeave() {
      setNormal();

      onLeave.Invoke();
    }
    private void onPointerClick() {
      if (enableDebugger) Debug.Log(name + " is clicked");
      onClick.Invoke();
    }
    private void setNormal() {
      if (animator == null) return; ;

      TargetButton.disabledCG.alpha = 0;
      TargetButton.normalCG.alpha = 1;
      TargetButton.highlightCG.alpha = 0;

      animator.SetBool(BUTTON_STATE.Enter.ToString(), false);
      animator.SetBool(BUTTON_STATE.Normal.ToString(), true);

    }
  }
}