using DG.Tweening;
using Halabang.UI;
using Halabang.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Halabang.Game {
  public class SaveLoadManager : MonoBehaviour {
    public enum SCENE_NAME_SINGLE {
      MENU = 0, 
      MAIN = 1, 
      STORY = 2
    }
    public enum SCENE_NAME_ADDITIVE_MINIGAME {
      NULL,
      TETRIS = 1,
    }

    public PersistentGameSettings CurrentGamePreference { get; private set; }

    public bool IsLoading => isLoadingScene;
    public bool IsAddingScene { get; private set; } //true if loading scene additive is in progressing

    [Header("基础设定")]
    [SerializeField] private CanvasGroup holder;
    [SerializeField] private ProgressController loadingProgressController; 
    [Tooltip("默认最低加载时间")]
    [SerializeField] private float loadingProgressDuration;
    [Tooltip("加载时触发的后期")]
    [SerializeField] private Volume loadingTransitionVolume;
    [Tooltip("加载页面的后期")]
    [SerializeField] private Volume loadingScreenVolume;
    [Tooltip("加载过长的时长")]
    [SerializeField] private float loadingTransitionDuration;
    [Header("Events")]
    public UnityEvent onLoadingStart;
    public UnityEvent onLoadingCompleted;

    [Header("开发者选项")]
    [SerializeField] private bool enableDebugger;

    private bool isLoadingScene;
    private bool isUnloadingScene;
    private int currentMinigameIndex = -1;
    private float currentProgress;
    private string filePath;

    private void Awake() {
      if (holder == null) Debug.LogError("加载管理器必须指定一个 Loading Screen");
      //if (loadingBar == null) Debug.LogError("加载管理器必须指定一个 Loading Bar 进度条 image");
      if (loadingProgressDuration <= 0) Debug.LogError("加载管理器的默认最低加载时长必须大于0");
      if (loadingTransitionVolume == null) Debug.LogError("加载管理器必须指定一个 Loading Transition 后期");
      if (loadingTransitionDuration <= 0) Debug.LogError("加载管理器的 Loading Transition 过长时长必须大于0");

      CurrentGamePreference = GetComponent<PersistentGameSettings>();

      filePath = getFilePath();
    }
    private void Start() {
      //Debug.Log(loadingCharacterStartPosition + " : " + loadingCharacterEndPosition);
      holder.alpha = 0;
    }

    public void LoadSceneAdditive(SCENE_NAME_ADDITIVE_MINIGAME targetSceneAdditive) {
      if (IsAddingScene) return; //避免重复加载`
      currentMinigameIndex = (int)targetSceneAdditive;
      StartCoroutine(loadingSceneAdditiveSequence((int)targetSceneAdditive));
    }
    private IEnumerator loadingSceneAdditiveSequence(int sceneIndex) {
      IsAddingScene = true;
      UnityEngine.AsyncOperation loadingScreenTask = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex,LoadSceneMode.Additive);
      while (loadingScreenTask.isDone == false) {
        //在加载场景过程中的行为逻辑
        Debug.Log("000");
        yield return null;
      }
      yield return null; //等scene manager Awake

      IsAddingScene = false;
      Debug.Log("1111");
    }
    public void UnloadMiniGameScene() {
      if (currentMinigameIndex < 0) return;
      StartCoroutine(unloadingSceneAdditiveSequence(currentMinigameIndex));
      currentMinigameIndex = -1;
    }
    private IEnumerator unloadingSceneAdditiveSequence(int sceneIndex) {
      isUnloadingScene = true;

      UnityEngine.AsyncOperation loadingScreenTask = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneIndex);

      while (loadingScreenTask.isDone == false) {
        //在加载场景过程中的行为逻辑
        yield return null;
      }
      yield return null; //等scene manager Awake
      isUnloadingScene = false;
    }
    public void LoadSceneSingle(SCENE_NAME_SINGLE targetSceneSingle) {
      StartCoroutine(loadingSceneSingleSequence(targetSceneSingle));
    }
    private IEnumerator loadingSceneSingleSequence(SCENE_NAME_SINGLE targetSceneSingle) {
      isLoadingScene = true;
      currentProgress = 0;
      onLoadingStart.Invoke();
      //temporally load the progress bar unconditionally
      if (loadingProgressController != null) loadingProgressController.SetProgress(currentProgress);
      //加载过场
      if (enableDebugger) Debug.Log("开始加载场景: " + currentProgress + "%");
      Volume currentSceneVolume = GameManager.Instance.CurrentSceneManager.CurrentVolume;
      DOTween.To(() => currentSceneVolume.weight, loadingTransitionValue => currentSceneVolume.weight = loadingTransitionValue, 0f, loadingTransitionDuration).SetEase(Ease.OutSine);
      DOTween.To(() => loadingTransitionVolume.weight, loadingTransitionValue => loadingTransitionVolume.weight = loadingTransitionValue, 1f, loadingTransitionDuration).SetEase(Ease.InSine);
      holder.DOFade(1, loadingTransitionDuration).SetEase(Ease.InSine);
      yield return new WaitForSeconds(loadingTransitionDuration);
      DOTween.To(() => loadingTransitionVolume.weight, loadingTransitionValue => loadingTransitionVolume.weight = loadingTransitionValue, 0f, loadingTransitionDuration).SetEase(Ease.InSine);
      DOTween.To(() => loadingScreenVolume.weight, loadingTransitionValue => loadingScreenVolume.weight = loadingTransitionValue, 1f, loadingTransitionDuration).SetEase(Ease.OutSine);
      yield return new WaitForSeconds(loadingTransitionDuration);
      //开始加载
      yield return new WaitForSeconds(1);
      updateLoadingProgress(0.25f, loadingProgressDuration / 4f);
      yield return new WaitForSeconds(loadingProgressDuration / 4f);

      UnityEngine.AsyncOperation loadingScreenTask = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)targetSceneSingle, UnityEngine.SceneManagement.LoadSceneMode.Single);
      while (loadingScreenTask.isDone == false) {
        //在加载场景过程中的行为逻辑
        yield return null;
      }
      yield return null; //等scene manager Awake
      yield return new WaitUntil(() => GameManager.Instance.CurrentSceneManager.IsPrerequisiteLoaded); //等scene manager初始化
      //重置新加载的场景默认后期权重为0
      currentSceneVolume = GameManager.Instance.CurrentSceneManager.CurrentVolume;
      currentSceneVolume.weight = 0f;
      yield return new WaitForSeconds(1f);
      if (enableDebugger) Debug.Log("加载场景结束: " + currentProgress + "%");

      updateLoadingProgress(0.25f, loadingProgressDuration / 4f);
      yield return new WaitForSeconds(loadingProgressDuration / 4f);

      updateLoadingProgress(0.5f, loadingProgressDuration / 2f);
      yield return new WaitForSeconds(loadingProgressDuration / 2f);
      //加载过场
      yield return new WaitForSeconds(1);
      if (enableDebugger) Debug.Log("过场动画结束: " + currentProgress + "%");
      DOTween.To(() => loadingScreenVolume.weight, loadingTransitionValue => loadingScreenVolume.weight = loadingTransitionValue, 0f, loadingTransitionDuration);
      DOTween.To(() => loadingTransitionVolume.weight, loadingTransitionValue => loadingTransitionVolume.weight = loadingTransitionValue, 1f, loadingTransitionDuration);
      yield return new WaitForSeconds(loadingTransitionDuration);
      holder.DOFade(0f, loadingTransitionDuration).SetEase(Ease.InSine);
      DOTween.To(() => loadingTransitionVolume.weight, loadingTransitionValue => loadingTransitionVolume.weight = loadingTransitionValue, 0f, loadingTransitionDuration);
      DOTween.To(() => currentSceneVolume.weight, loadingTransitionValue => currentSceneVolume.weight = loadingTransitionValue, 1f, loadingTransitionDuration);
      yield return new WaitForSeconds(loadingTransitionDuration);
      //加载结束
      if (enableDebugger) Debug.Log("加载结束: " + currentProgress + "%");
      onLoadingCompleted.Invoke();
      isLoadingScene = false;
    }
    private void updateLoadingProgress(float loadingPercentage, float duration) {
      currentProgress += loadingPercentage;
      if (loadingProgressController != null) {
        loadingProgressController.SetProgress(currentProgress);
      } else {
        Debug.LogWarning("Loading Progress Controller is not set, skipping progress update.");
      }
    }


    public void SaveToFile() {
      try {
        GameData gameData = getData();
        //create the directory if it is not existed
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        //stringfy game filePath
        string saveData = JsonConvert.SerializeObject(gameData, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        Debug.Log(saveData);
        //write it to the file
        using (FileStream stream = new FileStream(filePath, FileMode.Create)) {
          using (StreamWriter writer = new StreamWriter(stream)) {
            writer.Write(saveData);
          }
        }
      } catch (Exception ex) {
        Debug.LogError("Saving game data failed for " + filePath + ": " + ex.Message);
      }
    }
    public void LoadFromFile() {
      string loadData = "";
      try {
        if (File.Exists(filePath)) {
          using (FileStream stream = new FileStream(filePath, FileMode.Open)) {
            using (StreamReader reader = new StreamReader(stream)) {
              loadData = reader.ReadToEnd();
            }
          }
          Debug.Log(loadData);
          GameData gameData = JsonConvert.DeserializeObject<GameData>(loadData);
          //Player.position = new Vector3(gameData.PlayerPosition[0], gameData.PlayerPosition[1], gameData.PlayerPosition[2]);
        } else {
          Debug.Log("File is not found to load for " + filePath);
        }
      } catch (Exception ex) {
        Debug.LogError("Loading game data failed for " + filePath + ": " + ex.Message);
      }
    }

    private GameData getData() {
      GameData gameData = new GameData();
      //float[] position = new float[] { Player.position.x, Player.position.y, Player.position.z };
      //gameData.PlayerPosition = position;

      return gameData;
    }
    private string getFilePath() {
      string filePath = Path.Combine(Application.persistentDataPath, "CatWoman", "user_data");
      //Debug.Log(filePath);
      return filePath;
    }
  }
}
