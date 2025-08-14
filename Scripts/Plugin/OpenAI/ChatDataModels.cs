using Halabang.Game;
using Halabang.Story;
using Halabang.Utilities;
using System;
using System.Collections.Generic;
using SimpleSQL;
using System.Text.RegularExpressions;
using UnityEngine;
using Halabang.Character;

namespace Halabang.Plugin {
  /// <summary>
  /// 聊天消息数据模型
  /// </summary>
  public class ChatContextData {
    [PrimaryKey]
    [AutoIncrement]
    public int ID { get; set; }
    public string ResponseID { get; set; }
    public string RequestID { get; set; }
    public int Role { get; set; }
    public int FinishReason { get; set; }
    public int ChoiceIndex { get; set; }
    public int ContextType { get; set; }
    public int Emotion { get; set; }
    [Ignore]
    public List<string> ResponseOptions { get; set; }

    public ChatContextData() { }
    /// <summary>
    /// Set data from a request, usually input from a user or system setting
    /// </summary>
    /// <param name="request"></param>
    /// <param name="index"></param>
    public ChatContextData(ChatMessage message, string requestID) {
      RequestID = requestID;
      Role = (int)Enum.Parse<ChatDictionary.MESSAGE_ROLE>(message.Role, true);
      FinishReason = (int)ChatDictionary.FINISH_REASON.Null;
      ChoiceIndex = message.Index;
      Emotion = (int)CharacterEmotion.EMOTION_STATE.Calm;
      ContextType = (int)message.MessageType;
    }
    /// <summary>
    /// Set data from a response
    /// </summary>
    /// <param name="choice"></param>
    /// <param name="responseID"></param>
    /// <param name="actorID"></param>
    public ChatContextData(ChatCompletionChoice choice, string responseID) {
      ResponseID = responseID;
      Role = (int)Enum.Parse<ChatDictionary.MESSAGE_ROLE>(choice.Message.Role, true);
      FinishReason = (int)DataHelper.GetEnumByDescription<ChatDictionary.FINISH_REASON>(choice.FinishReason);
      ContextType = (int)ChatDictionary.MESSAGE_TYPE.Chat;
      ChoiceIndex = choice.Index;

      //定于当前回复的情绪
      CharacterEmotion.EMOTION_STATE respondEmotion = CharacterEmotion.EMOTION_STATE.Calm;
      //string pattern = ChatDictionary.EMOTION_PATTERN;
      ////var emotionFound = Regex.Match(choice.Message.Result, pattern);
      //if (emotionFound.Success) {
      //}
      if (string.IsNullOrWhiteSpace(choice.Message.Result) == false) {
        string[] responseArray = choice.Message.Result.Split(new[] { "\r\n\r\n", "\n\n", "\n", "\\n\\n", "\\\n\\\n" }, StringSplitOptions.None);
        if (responseArray.Length > 1) {
          Enum.TryParse(responseArray[1], ignoreCase: true, out respondEmotion);
          Debug.Log("emotion is >>>>> " + respondEmotion + " >>>> " + responseArray[1]);
        }
        Emotion = (int)respondEmotion;
        if (responseArray.Length > 2) {
          ResponseOptions = new List<string>();
          List<string> options = new List<string>(responseArray[2].Split(new[] { "#", "##" }, StringSplitOptions.RemoveEmptyEntries));
          if (options != null) {
            for (int i = 0; i < options.Count; i++) {
              if (string.IsNullOrWhiteSpace(options[i])) continue;
              string option = Regex.Replace(options[i], ChatDictionary.COLON_INDEX_PATTERN, "");
              option = Regex.Replace(option, ChatDictionary.PERIOD_INDEX_PATTERN, "");
              ResponseOptions.Add(option.Trim());
            }
          }
        }
      }
    }
  }  
  public class ChatContentData {
    [PrimaryKey]
    [AutoIncrement]
    public int ID { get; set; }
    public int ContextID { get; set; }
    public int ContentType { get; set; }
    public string Content { get; set; }

    public ChatContentData() { }
    public ChatContentData(int contextID, ChatDictionary.CONTENT_TYPE contentTYpe, string content) {
      ContextID = contextID;
      ContentType = (int)contentTYpe;
      Content = content;
    }
    public ChatContentData(int contextID, ChatContent content) {
      ContextID = contextID;
      ContentType = (int)DataHelper.GetEnumByDescription<ChatDictionary.CONTENT_TYPE>(content.ContentType);
      if (ContentType == (int)ChatDictionary.CONTENT_TYPE.TEXT) {
        Content = content.Text;
      } else if (ContentType == (int)ChatDictionary.CONTENT_TYPE.IMAGE_URL) {
        Content = content.ImageURL.URL;
      }
    }
  }

  /// <summary>
  /// 获取响应的数据模型
  /// </summary>
  public class ChatResponseData {
    [PrimaryKey]
    public string ID { get; set; }
    public string ActorID { get; set; }
    public string PlayerID { get; set; }
    public string StoryID { get; set; }
    public int Language { get; set; }
    public int Model { get; set; }
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int Created { get; set; }

    public ChatResponseData() { }
    public ChatResponseData(ChatResponse response) {
      ID = response.ID;
      ActorID = response.ActorID;
      PlayerID = response.PlayerID;
      StoryID = response.StoryID;
      Language = (int)GameManager.Instance._SaveLoadManager.CurrentGamePreference.CurrentGameSetting.CurrentLanguage;
      Model = (int)DataHelper.GetEnumByDescription<ChatDictionary.CHAT_MODEL>(response.Model);
      PromptTokens = response.Usage.PromptTokens;
      CompletionTokens = response.Usage.CompletionTokens;
      Created = (int)response.Created;
    }
  }

  /// <summary>
  /// 提交请求的数据模型
  /// </summary>
  public class ChatRequestData {
    [PrimaryKey]
    public string ID { get; set; }
    public string ActorID { get; set; }
    public string PlayerID { get; set; }
    public string StoryID { get; set; }
    public int Language { get; set; }
    public int Model { get; set; }
    public int Created { get; set; }

    public ChatRequestData() { }
    public ChatRequestData(ChatRequest request) {
      ID = request.ID;
      ActorID = request.ActorID;
      PlayerID = request.PlayerID;
      StoryID = request.StoryID;
      Language = (int)GameManager.Instance._SaveLoadManager.CurrentGamePreference.CurrentGameSetting.CurrentLanguage;
      Model = (int)DataHelper.GetEnumByDescription<ChatDictionary.CHAT_MODEL>(request.Model);
      Created = request.Created == 0 ? (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() : (int)request.Created;
    }
  }
}