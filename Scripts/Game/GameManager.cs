using UnityEngine;
using Halabang.Plugin;
using Halabang.Audio;
using Halabang.UI;

namespace Halabang.Game {
  public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public SaveLoadManager _SaveLoadManager { get; private set; }
    public DoTweenManager _DoTweenManager { get; private set; }
    public SoundManager _SoundManager { get; private set; }
    public UIManager _UIManager { get; private set; }
    public SceneManager CurrentSceneManager { get; private set; }
    public DialogueManager _DialogueManger { get; private set; }
    public DatabaseManager _DatabaseManager { get; private set; }
    public OpenAIManager _OpenAIManager { get; private set; }

    private void Awake() {
      if (Instance == null) {
        Instance = this;
        DontDestroyOnLoad(gameObject);
      } else {
        Destroy(gameObject);
      }

      _SaveLoadManager = GetComponentInChildren<SaveLoadManager>();
      _DoTweenManager = GetComponentInChildren<DoTweenManager>();
      _SoundManager = GetComponentInChildren<SoundManager>();
      _UIManager = GetComponentInChildren<UIManager>();
      _DialogueManger = GetComponentInChildren<DialogueManager>();
      _DatabaseManager = GetComponentInChildren<DatabaseManager>();
      _OpenAIManager = GetComponentInChildren<OpenAIManager>();
    }

    public void RegisterSceneManager(SceneManager sceneManager) {
      CurrentSceneManager = sceneManager;
    }
  }
}
