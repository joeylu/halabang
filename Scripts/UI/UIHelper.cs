using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Halabang.UI {
  public class UIHelper {
    public static void FixLayoutGroup(float delay, RectTransform targetLayoutGroup) {
      DOVirtual.DelayedCall(delay, () => LayoutRebuilder.ForceRebuildLayoutImmediate(targetLayoutGroup));
    }
  }
  public static class UIHelperExt {
    public static Vector2 GetTopLeft(this RectTransform rectTransform, bool worldPosition = false) {
      return getPoint(rectTransform, 1, worldPosition);
    }
    public static Vector2 GetTopRight(this RectTransform rectTransform, bool worldPosition = false) {
      return getPoint(rectTransform, 2, worldPosition);
    }
    public static Vector2 GetBottomRight(this RectTransform rectTransform, bool worldPosition = false) {
      return getPoint(rectTransform, 3, worldPosition);
    }
    public static Vector2 GetBottomLeft(this RectTransform rectTransform, bool worldPosition = false) {
      return getPoint(rectTransform, 0, worldPosition);
    }
    private static Vector2 getPoint(RectTransform rectTransform, int cornerIndex, bool worldPosition) {
      Vector3[] values = new Vector3[4];
      Canvas.ForceUpdateCanvases();
      LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
      rectTransform.GetWorldCorners(values);
      if (worldPosition) {
        return Camera.main.ScreenToWorldPoint(values[cornerIndex]);
      } else {
        return values[cornerIndex];
      }
    }
  }
}
