using UnityEngine;
using UnityEngine.InputSystem;

namespace Halabang.Game {
  public class CursorManager : MonoBehaviour {
    [SerializeField] private RectTransform cursorHolder; //获取鼠标canvas容器
    [SerializeField] private RectTransform globalCursorPrefab; //获取默认鼠标UI的预制件
    [Header("开发者")]
    [SerializeField] private bool defaultVisible;

    private CursorUI currentCursorUI; //当前鼠标UI
    private Vector3 cursorPosition; //当前鼠标位置
    private RectTransform globalCursor; //默认鼠标的UI实体
    private CursorUI globalCursorUI; //默认鼠标UI组件实体

    private void Awake() {
      Cursor.visible = defaultVisible;
      //Cursor.lockState = CursorLockMode.Locked;
    }
    private void Start() {
      if (globalCursorPrefab == null) {
        Debug.LogError("Cursor manager must set a default global cursor prefab");
        return;
      }
      initCursor();
    }
    private void Update() {
      if (currentCursorUI) {
        cursorPosition = Mouse.current.position.ReadValue();
        cursorHolder.anchoredPosition = cursorPosition;
      }
    }

    private void initCursor() {
      globalCursor = Instantiate(globalCursorPrefab, cursorHolder);
      globalCursorUI = globalCursor.GetComponent<CursorUI>();

      currentCursorUI = globalCursorUI;
    }
  }
}
