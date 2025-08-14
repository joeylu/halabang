using UnityEngine;

namespace Halabang.Game {
  public class DatabaseManager : MonoBehaviour {
    public SimpleSQLManager_WithSystemData ChatContextDB => chatContextDB;

    [Header("设定")]
    [Tooltip("指定用来存储AI生成式对话的上下文数据库")]
    [SerializeField] private SimpleSQLManager_WithSystemData chatContextDB;

    private void Awake() {
      //chatContextDB.overwriteIfExists = false;
    }
  }
}