using Halabang.Plugin;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Halabang.Scene {
  public class PlacerBase : MonoBehaviour {
    public List<Transform> places { get; protected set; }

    [SerializeField] protected Collider2D holder2D;
    [Range(0, 1000)]
    [Tooltip("一共生成多少个")]
    [SerializeField] protected int count;
    [Tooltip("生成的Transform尺寸按比例缩放, x为最小值，y为最大值")]
    [SerializeField] protected Vector2 minMaxScale;
    [Tooltip("默认间隔最短距离")]
    [SerializeField] protected float minDistance;
    [Tooltip("按顺序每一个生成的对象设定")]
    [SerializeField] protected TweenSetting sequenceTweenerSetting;
    [Tooltip("当勾选时，隐藏过程不再引入顺序")]
    [SerializeField] protected bool ignoreSequenceTweenerOnHide;

    protected void ShowPlacers() {
      if (gameObject.activeInHierarchy == false) {
        Debug.LogError(name + " 游戏对象必须为 Active 状态");
        return;
      }

      destroyCurrentList();
      places = new List<Transform>();

      for (int i = 0; i < count; i++) {
        GameObject obj = new GameObject();
        Transform t = obj.transform;
        t.SetParent(holder2D.transform);
        t.position = getRandomPosionFromCollider2D();
        if (minMaxScale != Vector2.zero) {
          float randomScale = Random.Range(minMaxScale.x, minMaxScale.y);
          t.localScale = new Vector3(randomScale, randomScale, randomScale);
        }
        places.Add(t);

        obj.SetActive(false);
      }
    }
    protected void HidePlacers() {
      if (gameObject.activeInHierarchy == false) {
        Debug.LogError(name + " 游戏对象必须为 Active 状态");
        return;
      }

      destroyCurrentList();
    }

    private void destroyCurrentList() {
      if (places != null && places.Count > 0) {
        foreach (Transform place in places) {
          GameObject.Destroy(place.gameObject);
        }
      }
    }
    protected Vector2 getRandomPosionFromCollider2D() {
      Vector2 position = new Vector2(
        Random.Range(holder2D.bounds.min.x, holder2D.bounds.max.x),
        Random.Range(holder2D.bounds.min.y, holder2D.bounds.max.y)
      );
      if (Physics2D.OverlapPoint(position) == false) {
        return getRandomPosionFromCollider2D();
      } else {
        if (places.Any(r => Vector3.Distance(r.position, position) < minDistance)) {
          return getRandomPosionFromCollider2D();
        } else {
          return position;
        }
      }
    }
  }
}
