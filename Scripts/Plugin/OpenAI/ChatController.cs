using System.Collections;
using System.Linq;
using UnityEngine;
using Michsky.MUIP;
using Halabang.Story;
using System.Collections.Generic;
using Halabang.Utilities;
using Newtonsoft.Json;
using System;
using UnityEngine.UI;
using Halabang.UI;
using Halabang.Character;
using Halabang.Game;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Halabang.Plugin {
  public class ChatController : MonoBehaviour {
    [Header("基础设定")]
    [Tooltip("基于故事框架和对话规范的预设文件")]
    [SerializeField] private ChatStoryPreset chatStoryPreset;
    [Header("对话UI设定")]
    [Tooltip("对话输入框")]
    [SerializeField] private CustomInputField inputField;
    [Tooltip("对话提交按钮")]
    [SerializeField] private ButtonManagerExt submitButton;
    [Tooltip("系统设置输入框")]
    [SerializeField] private CustomInputField systemInputField;
    [Tooltip("系统设置提交按钮")]
    [SerializeField] private ButtonManagerExt systemSubmitButton;
    [Header("对话历史记录面板设定")]
    [Tooltip("是否激活对话历史记录")]
    [SerializeField] private bool enableChatHistory;
    [SerializeField] private CanvasGroup chatRecordCanvasGroup;
    [Tooltip("对话记录列表父系")]
    [SerializeField] private RectTransform chatRecordListHolder;
    [Tooltip("对话输出预制件")]
    [SerializeField] private ChatRecord chatRecordOutputPrefab;
    [Tooltip("对话输入预制件")]
    [SerializeField] private ChatRecord chatRecordInputPrefab;

    [Header("开发者选项")]
    [SerializeField] private bool resetStoryContext;
    [SerializeField] private bool disableActorInitialization;
    public bool enableDebugger;

    private CharacterBase storyChatPlayer; //当前对话时让玩家扮演的角色
    private CharacterBase storyChatTarget; //当前对话时让AI扮演的角色
    private List<KeyValuePair<int, string>> currentStoryResponseOptions; //缓存当前对话的响应选项列表
    private int currentStoryResponseIndex = 0; //当前故事响应选项索引

    private void Awake() {
      clearContext();
      //chatResponsesLog = new List<ChatResponse>();
      //chatContextLog = new List<ChatContextData>();
      if (submitButton) submitButton.onClick.AddListener(onSubmitClicked);
      if (systemSubmitButton) systemSubmitButton.onClick.AddListener(systemSettingFill);
    }
    private IEnumerator Start() {
      if (disableActorInitialization) yield break;

      yield return null; //确保游戏管理器所有管理器都已初始化完成
      if (chatStoryPreset == null) Debug.LogError(name + " 必须指定一个故事预设本");
      if (ValidationHelper.IsGuid(chatStoryPreset.StoryID) == false) Debug.LogError(name + " 必须指定一个有效的故事ID");
      if (chatStoryPreset.DefaultPlayerActor == null) Debug.LogError(name + " 必须给玩家指定一个角色卡");
      storyChatPlayer = GameManager.Instance.CurrentSceneManager.CurrentCharaterManager.GetCharacter(chatStoryPreset.DefaultPlayerActor);
      if (chatStoryPreset.DefaultTargetActor == null) Debug.LogError(name + " 必须给AI扮演的角色指定一个角色卡");
      storyChatTarget = GameManager.Instance.CurrentSceneManager.CurrentCharaterManager.GetCharacter(chatStoryPreset.DefaultTargetActor);
      //初始化故事对话请求
      if (resetStoryContext) GameManager.Instance._OpenAIManager.ResetStoryContext(chatStoryPreset);
      GameManager.Instance._OpenAIManager.InitializeStoryRequest(chatStoryPreset);
      //提交最终初始化请求，但不记录响应内容
      Task submitTask = Task.Run(() => GameManager.Instance._OpenAIManager.SubmitRequest(true));
      while (submitTask.IsCompleted == false) {
        yield return null;
      }
      //等待初始化完成后，显示首个问答选项（若有）
      currentStoryResponseIndex = 0; //重置当前回应选项索引
      storyMenuResponse();

      GameManager.Instance._OpenAIManager.OnActorsSwitched.AddListener(switchCharacter);
    }

    private void switchCharacter() {
      if (GameManager.Instance._OpenAIManager.CurrentChatRequest.ActorID.Equals(chatStoryPreset.DefaultTargetActor.ActorGuid, StringComparison.OrdinalIgnoreCase) == false) {
        storyChatTarget = GameManager.Instance.CurrentSceneManager.CurrentCharaterManager.GetCharacter(GameManager.Instance._OpenAIManager.CurrentChatRequest.ActorID);
      }
      if (GameManager.Instance._OpenAIManager.CurrentChatRequest.PlayerID.Equals(chatStoryPreset.DefaultPlayerActor.ActorGuid, StringComparison.OrdinalIgnoreCase) == false) {
        storyChatPlayer = GameManager.Instance.CurrentSceneManager.CurrentCharaterManager.GetCharacter(GameManager.Instance._OpenAIManager.CurrentChatRequest.PlayerID);
      }
    }

    private void systemSettingFill() {
      if (string.IsNullOrWhiteSpace(systemInputField.inputText.text)) return;

      GameManager.Instance._OpenAIManager.CreateMessage(ChatDictionary.MESSAGE_ROLE.System, ChatDictionary.CONTENT_TYPE.TEXT, systemInputField.inputText.text);
      systemInputField.processSubmit = true; //手动激活提交状态
      setInputSubmitted(systemInputField); //交内容组件的状态
      submitRequest(true);
    }
    private void onSubmitClicked() {
      if (string.IsNullOrWhiteSpace(inputField.inputText.text)) return;

      GameManager.Instance._OpenAIManager.CreateMessage(ChatDictionary.MESSAGE_ROLE.User, ChatDictionary.CONTENT_TYPE.TEXT, inputField.inputText.text);
      inputField.processSubmit = true; //手动激活提交状态
      setInputSubmitted(inputField); //手动更新提交内容组件的状态
      submitRequest(false);
    }
    private async void submitRequest(bool ignoreResponse) {
      //有另外一个对话在提交中，当前对话不允许提交
      if (GameManager.Instance._OpenAIManager.IsSubmitting) return;
            
      if (submitButton) submitButton.TargetButton.Interactable(false); //手动禁用提交按钮
      if (systemSubmitButton) systemSubmitButton.TargetButton.Interactable(false); //手动禁用系统设置提交按钮

      await GameManager.Instance._OpenAIManager.SubmitRequest(ignoreResponse);
      //Debug.Log(JsonConvert.SerializeObject(GameManager.Instance._OpenAIManager.CurrentChatRequest.Messages.Last()));
      if (ignoreResponse == false) {
        setChatRecordUI(GameManager.Instance._OpenAIManager.CurrentChatRequest.Messages.Last());
      }

      if (submitButton) submitButton.TargetButton.Interactable(true); //手动启用提交按钮
      if (systemSubmitButton) systemSubmitButton.TargetButton.Interactable(true); //手动启用系统设置提交按钮
      if (inputField) inputField.processSubmit = false; //手动重置提交状态
      if (systemInputField) systemInputField.processSubmit = false; //手动重置系统设置提交状态
    }
    private void clearContext() {
      if (chatRecordListHolder) chatRecordListHolder.transform.DestroyChildren();
    }

    private void setChatRecordUI(ChatMessage message) {
      //构筑对话记录UI
      if (enableChatHistory && chatRecordListHolder && chatRecordInputPrefab && chatRecordOutputPrefab) {
        if (enableDebugger) Debug.Log("设置UI中：" + JsonConvert.SerializeObject(message));
        if (message.Role == ChatDictionary.MESSAGE_ROLE.User.Description()) {
          ChatRecord record = Instantiate(chatRecordInputPrefab, chatRecordListHolder);
          record.SetRecord(message, storyChatPlayer.CurrentActor);
        } else if (message.Role == ChatDictionary.MESSAGE_ROLE.Assistant.Description()) {
          ChatRecord record = Instantiate(chatRecordOutputPrefab, chatRecordListHolder);
          record.SetRecord(message, storyChatTarget.CurrentActor);
        }
        UIHelper.FixLayoutGroup(0.5f, chatRecordListHolder);
      }
    }
    private void setInputSubmitted(CustomInputField input) {
      if (input == null) return;

      input.onSubmit.Invoke();

      if (input.clearOnSubmit) {
        input.inputText.text = "";
        input.UpdateState();
      }
    }

    /// <summary>
    /// 显示故事预设的响应选项菜单，如果没有预设的故事回应选项或当前对话响应内容没有提供回应选项，则不进行任何操作
    /// </summary>
    private void storyMenuResponse() {
      currentStoryResponseOptions = null; //重置

      if (chatStoryPreset.ResponseOptions.Count > currentStoryResponseIndex) {
        //如果有预设的响应选项，则显示选项菜单
        if (chatStoryPreset.ResponseOptions == null || chatStoryPreset.ResponseOptions.FirstOrDefault() == null) return;
        if (chatStoryPreset.ResponseOptions.Count <= currentStoryResponseIndex) return; //如果当前响应索引超出范围，则不进行任何操作

        List<CopywritingPreset> responseContents = chatStoryPreset.ResponseOptions[currentStoryResponseIndex].Contents;
        if (responseContents != null && responseContents.Count > 0) {
          currentStoryResponseOptions = new List<KeyValuePair<int, string>>();
          for (int i = 0; i < responseContents.Count; i++) {
            currentStoryResponseOptions.Add(new KeyValuePair<int, string>(i, responseContents[i].Title));
          }
          storyChatPlayer._CharacterUI.ShowMenu(currentStoryResponseOptions, onStoryMenuResponsed);
        }
      } else {
        //如果没有预设的响应选项，则分析当前对话内容，若有回馈响应选项，则生成新的响应选项，若无，则中断对话
        List<string> options = GameManager.Instance._OpenAIManager.CurrentChatRequest.Messages.Last().ResponseOptions;
        if (options != null) {
          currentStoryResponseOptions = new List<KeyValuePair<int, string>>();
          for (int i = 0; i < options.Count; i++) {
            currentStoryResponseOptions.Add(new KeyValuePair<int, string>(i, options[i]));
          }
          storyChatPlayer._CharacterUI.ShowMenu(currentStoryResponseOptions, onStoryMenuResponsed);
        }
      }
    }
    /// <summary>
    /// 响应选项被选择后执行的回调方法
    /// </summary>
    /// <param name="index"></param>
    private void onStoryMenuResponsed(int index) {
      if (currentStoryResponseOptions != null) {
        storyChatPlayer._CharacterUI.HideMenu();
        string responseContent = string.Empty;

        if (chatStoryPreset.ResponseOptions.Count > currentStoryResponseIndex) {
          CopywritingPreset responsedCopywriting = chatStoryPreset.ResponseOptions[currentStoryResponseIndex].Contents[index];
          responseContent = responsedCopywriting.Brief;

          //重建规则。注意！！！！！ 该规则会完全覆盖过往设立的所有规则
          if (string.IsNullOrWhiteSpace(responsedCopywriting.Content) == false) {
            Debug.LogWarning(name + " 当前的提交 " + responsedCopywriting.Title + " 将覆盖当前故事的初始设定包括人物设定，及所有过往规则");
            GameManager.Instance._OpenAIManager.CreateMessage(ChatDictionary.MESSAGE_ROLE.User, ChatDictionary.CONTENT_TYPE.TEXT, responsedCopywriting.Content);
          }
        } else {
          responseContent = currentStoryResponseOptions[index].Value;
        }

        //当前故事响应选项，创建新消息
        storyChatPlayer._CharacterUI.Bark(responseContent);
        GameManager.Instance._OpenAIManager.CreateMessage(ChatDictionary.MESSAGE_ROLE.User, ChatDictionary.CONTENT_TYPE.TEXT, responseContent);
      }
      //Debug.Log("xxxxxxxxxx "+ JsonConvert.SerializeObject(GameManager.Instance._OpenAIManager.CurrentChatRequest));
      //提交请求
      StartCoroutine(startRespondChatSequence());
    }
    private IEnumerator startRespondChatSequence() {
      if (enableDebugger) Debug.Log("开始提交当前回应请求 轮次: " + currentStoryResponseIndex);
      float timeout = 0;
      //等待对话请求提交并获得响应内容
      Task task = Task.Run(() => GameManager.Instance._OpenAIManager.SubmitRequest());
      while (task.IsCompleted == false && timeout < GameSettings.DEFAULT_TIMEOUT) {
        if (task.IsCanceled || task.IsFaulted) break;
        timeout += Time.deltaTime;
        yield return null;
      }
      if (enableDebugger) Debug.Log("完成提交: " + task.IsCompleted + " 用时：" + timeout);
      if (task.IsCompleted == false) {
        //如果任务被取消或失败，则不进行任何操作
        storyChatTarget._CharacterUI.Bark("Sorry, Mr. Anderson");
      } else {
        //获取响应内容并更新Chat对象Bark
        if (enableDebugger) Debug.Log("获得响应内容：" + GameManager.Instance._OpenAIManager.CurrentChatRequest.Messages.Last().Contents.First().Text);
        string[] responseArray = GameManager.Instance._OpenAIManager.CurrentChatRequest.Messages.Last().Result.Split(new[] { "\r\n\r\n", "\n\n", "\n", "\\n\\n", "\\\n\\\n" }, StringSplitOptions.None);
        if (responseArray.Length == 3 && string.IsNullOrWhiteSpace(responseArray[0]) == false && string.IsNullOrWhiteSpace(responseArray[1]) == false && string.IsNullOrWhiteSpace(responseArray[2]) == false) {
          string firstParagraph = Regex.Replace(responseArray[0], ChatDictionary.IGNORE_BRACKETS_PATTERN, "").Trim();
          storyChatTarget._CharacterUI.Bark(firstParagraph);
          storyChatTarget._CharacterEmotion.ChangeEmotion(GameManager.Instance._OpenAIManager.CurrentChatRequest.Messages.Last().Emotion);
        } else {
          storyChatTarget._CharacterUI.Bark("Houston, we have a problem.");
        }

      }
      //等待对话泡显示完成
      yield return new WaitUntil(() => storyChatTarget._CharacterUI.IsBarking == false);

      //进行下一轮对话
      currentStoryResponseIndex += 1; //更新当前回应选项索引
      storyMenuResponse();
    }

#if UNITY_EDITOR
    [Header("编辑器排查选项")]
    [SerializeField] private bool fixLayout;

    private void OnValidate() {
      if (fixLayout && chatRecordListHolder) {
        fixLayout = false;
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatRecordListHolder);
      }
    }
#endif
  }
}
