using Halabang.Story;
using Halabang.UI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Halabang.Utilities;
using System.Threading.Tasks;
using Halabang.Game;
using System.Text;
using System.Net;
using UnityEngine.Events;

namespace Halabang.Plugin {
  public class OpenAIManager : MonoBehaviour {
    public ChatStoryPreset CurrentStoryPreset { get; private set; }
    public ChatRequest CurrentChatRequest { get; private set; }
    public bool IsSubmitting { get; private set; }

    [Header("事件")]
    public UnityEvent OnActorsSwitched;
    [Header("开发者选项")]
    [SerializeField] private bool enableDebugger;

    private List<ChatDirectorContent> directorMsgs = new List<ChatDirectorContent>(); //导演卡信息，key为角色ID，value为最后更新的针对该角色的导演卡信息

    /// <summary>
    /// 以文本输入者的角色进行输入
    /// </summary>
    /// <param name="user">当前的输入角色</param>
    /// <param name="targetActor">对象角色</param>
    /// <param name="text">文本</param>
    public void ChatInput(Actor user, Actor targetActor, string text) {

    }

    public void InitializeStoryRequest(ChatStoryPreset storyPreset) {
      if (storyPreset == null) return; //若故事预设文件为空，则不进行任何操作
      CurrentStoryPreset = storyPreset;

      foreach (ActorPreset actorPreset in storyPreset.Actors) {
        ChatDirectorContent directorContent = new ChatDirectorContent();
        directorContent.ActorID = actorPreset.ActorGuid;
        directorContent.PlayerID = storyPreset.DefaultPlayerActor.ActorGuid;
        directorMsgs.Add(directorContent);
      }

      //如果当前故事该角色和玩家扮演角色已经有配对的对话请求记录，则不需要重新创建对话请求，并加载所有上下文记录
      ChatRequestData requestRecord = 
        ChatHelper.GetRequestRecords(null, CurrentStoryPreset.DefaultTargetActor.ActorGuid, CurrentStoryPreset.DefaultPlayerActor.ActorGuid, CurrentStoryPreset.StoryID).FirstOrDefault();
      if (requestRecord != null) {
        CurrentChatRequest = new ChatRequest(requestRecord);
        if (enableDebugger) Debug.Log("初始化故事：获得已保存的上下文数据： " + JsonConvert.SerializeObject(CurrentChatRequest));
      } else {
        ChatRequest storyRequest = new ChatRequest();
        //新建故事设定
        ChatMessage storySystemMessage = new ChatMessage();
        storySystemMessage.MessageType = ChatDictionary.MESSAGE_TYPE.StorySetting;
        storySystemMessage.Role = ChatDictionary.MESSAGE_ROLE.System.Description();
        storySystemMessage.Index = 0;
        storySystemMessage.Contents.Add(new ChatContent(ChatDictionary.CONTENT_TYPE.TEXT, 
          CurrentStoryPreset.StoryContent.Content + Environment.NewLine + CurrentStoryPreset.StoryChatRules.Content));

        Debug.Log("xxxxxxxxxxxxx " + CurrentStoryPreset.StoryChatRules.Content);
        //添加到新的对话请求中
        storyRequest.Messages.Add(storySystemMessage);
        //新建导演卡
        ChatMessage directorSystemMessage = new ChatMessage();
        directorSystemMessage.MessageType = ChatDictionary.MESSAGE_TYPE.DirectorSetting;
        directorSystemMessage.Role = ChatDictionary.MESSAGE_ROLE.System.Description();
        directorSystemMessage.Index = 1;
        directorSystemMessage.Contents.Add(new ChatContent(ChatDictionary.CONTENT_TYPE.TEXT, string.Empty));
        //添加到新的对话请求中
        storyRequest.Messages.Add(directorSystemMessage);

        if (enableDebugger) Debug.Log("初始化故事：重构了故事设定上下文数据： " + JsonConvert.SerializeObject(storyRequest));
        //继续重新构筑角色扮演的设定信息
        InitializeStoryActorsRequest(CurrentStoryPreset.DefaultTargetActor, CurrentStoryPreset.DefaultPlayerActor, storyRequest);
      }
    }
    /// <summary>
    /// 基于当前故事预设，初始化故事角色对话上下文。注：若数据库中已经有对应的对话请求记录，则会加载该记录，并更新当前对话请求；若没有，则会构筑一个新的对话请求，并保存到数据库中。
    /// </summary>
    /// <param name="chatTargetGuid"></param>
    /// <param name="chatPlayerGuid"></param>
    /// <param name="incompleteStoryRequest">若来自initializeFromStory的call，则直接跳过获取数据库信息，继续构筑角色设定。</param>
    public void InitializeStoryActorsRequest(ActorPreset chatTarget, ActorPreset chatPlayer, ChatRequest incompleteStoryRequest = null) {

      //尝试获取当前对话请求的保存记录。 注：若当前call来自于ResetStoryRequest，则必然或获得一个刚刚保存的请求记录
      ChatRequestData requestRecord = ChatHelper.GetRequestRecords(null, chatTarget.ActorGuid, chatPlayer.ActorGuid, CurrentStoryPreset.StoryID).FirstOrDefault();
      //若来自于初始化故事，则默认为不可能会有该故事的角色对话上下文保存记录
      if (requestRecord != null && incompleteStoryRequest == null) {
        //若call获得当前故事的角色对话上下文保存记录，则基于保存的数据，更新当前对话请求
        CurrentChatRequest = new ChatRequest(requestRecord);
        //add director message
        if (enableDebugger) Debug.Log("初始化故事角色：获得已保存的上下文数据： " + JsonConvert.SerializeObject(CurrentChatRequest));
      } else {
        //若没有获得关于该故事对应的配对角色保存记录，则复制当前故事设定内容，构筑一个新的请求
        ChatRequest storyRequest = incompleteStoryRequest == null ? new ChatRequest() : incompleteStoryRequest;
        //若非继续构筑初始化故事请求，那么则重构故事设定的初始内容
        if (incompleteStoryRequest == null) {
          //新建故事设定
          ChatMessage storySystemMessage = new ChatMessage();
          storySystemMessage.MessageType = ChatDictionary.MESSAGE_TYPE.StorySetting;
          storySystemMessage.Role = ChatDictionary.MESSAGE_ROLE.System.Description();
          storySystemMessage.Index = 0;
          storySystemMessage.Contents.Add(new ChatContent(ChatDictionary.CONTENT_TYPE.TEXT,
            CurrentStoryPreset.StoryContent.Content + Environment.NewLine + CurrentStoryPreset.StoryChatRules.Content));
          //添加到新的对话请求中
          storyRequest.Messages.Add(storySystemMessage);
          //新建导演卡
          ChatMessage directorSystemMessage = new ChatMessage();
          directorSystemMessage.MessageType = ChatDictionary.MESSAGE_TYPE.DirectorSetting;
          directorSystemMessage.Role = ChatDictionary.MESSAGE_ROLE.System.Description();
          directorSystemMessage.Index = 1;

          StringBuilder directorMessage = new StringBuilder();
          foreach (ChatDirectorContent content in directorMsgs) {
            if (string.IsNullOrWhiteSpace(content.Content) == false) {
              directorMessage.AppendLine(content.Content);
            }
          }
          directorSystemMessage.Contents.Add(new ChatContent(ChatDictionary.CONTENT_TYPE.TEXT, directorMessage.ToString()));
          //添加到新的对话请求中
          storyRequest.Messages.Add(directorSystemMessage);
        }
        //初始化对话请求的上下文记录
        storyRequest.UpdateSystemMessage(ChatHelper.GetActorSystemMessage(storyRequest, new Actor(chatTarget), false));
        storyRequest.UpdateSystemMessage(ChatHelper.GetActorSystemMessage(storyRequest, new Actor(chatPlayer), true));
        //构筑新的请求对象
        CurrentChatRequest = new ChatRequest();
        CurrentChatRequest.ID = Guid.NewGuid().ToString();
        CurrentChatRequest.Model = ChatDictionary.CHAT_MODEL.QwenPlusCharacter.Description();
        CurrentChatRequest.ActorID = chatTarget.ActorGuid; //设置当前对话请求的AI扮演角色ID
        CurrentChatRequest.PlayerID = chatPlayer.ActorGuid; //设置当前对话请求的玩家扮演角色ID
        CurrentChatRequest.StoryID = CurrentStoryPreset.StoryID; //设置当前对话请求的故事ID
        CurrentChatRequest.Messages = storyRequest.Messages;
        if (enableDebugger) Debug.Log("初始化故事角色，重构角色的上下文数据： " + JsonConvert.SerializeObject(CurrentChatRequest));

        ChatRequestData chatRequestData = new ChatRequestData(CurrentChatRequest);
        //if (enableDebugger) Debug.Log(JsonConvert.SerializeObject(chatRequestData));
        if (ChatHelper.CreateRequestRecord(chatRequestData)) {
          //保存首条（目前也只有首条）系统故事角色设定内容
          createChatContextRecord(CurrentChatRequest.Messages.First());

        } else {
          Debug.LogError(name + " 尝试在数据库保存一个新的对话请求，但没有成功 " + JsonConvert.SerializeObject(CurrentChatRequest));
        }
      }
    }
    /// <summary>
    /// 添加新的对话消息到当前对话请求中。注：仅保存信息，提交Open AI需要调用SubmitRequest方法。
    /// </summary>
    /// <param name="role"></param>
    /// <param name="contentType"></param>
    /// <param name="content"></param>
    public ChatMessage CreateMessage(ChatDictionary.MESSAGE_ROLE role, ChatDictionary.CONTENT_TYPE contentType, string content, ChatRequest chatRequest = null) {
      if (string.IsNullOrWhiteSpace(content)) {
        Debug.LogWarning(name + " 尝试添加一个空的对话内容，无法进行任何操作");
        return null; //若对话内容为空，则不进行任何请求提交
      }
      if (role != ChatDictionary.MESSAGE_ROLE.User && role != ChatDictionary.MESSAGE_ROLE.Assistant) {
        Debug.Log("当前提交的role必须为user或assistant "+ role);
        return null; //必须为chat类，如果是system这些非chat类，需特殊处理，因为open ai并不支持多system message
      }

      ChatMessage chatMessage = new ChatMessage();
      chatMessage.MessageType = ChatDictionary.MESSAGE_TYPE.Chat; 
      chatMessage.Role = role.Description();
      chatMessage.Contents.Add(new ChatContent( contentType, content ));
      chatMessage.Index = chatRequest == null ? CurrentChatRequest.Messages.Count : chatRequest.Messages.Count;

      if (chatRequest != null) {
        chatRequest.Messages.Add(chatMessage);
      } else {
        CurrentChatRequest.Messages.Add(chatMessage);
      }

      createChatContextRecord(chatMessage, chatRequest == null ? null : chatRequest);

      return chatMessage;
    }
    public void SwitchTargetActor(ActorPreset actorPreset, ActorPreset playerPreset) {
      Actor targetChatActor = null;
      Actor targetPlayerActor = null;
      if (actorPreset.ActorGuid.Equals(CurrentChatRequest.ActorID, StringComparison.OrdinalIgnoreCase) == false) {
        targetChatActor = new Actor(actorPreset);
      }
      if (playerPreset.ActorGuid.Equals(CurrentChatRequest.PlayerID, StringComparison.OrdinalIgnoreCase) == false) {
        targetPlayerActor = new Actor(playerPreset);
      }
      if (targetChatActor == null && targetPlayerActor == null) {
        Debug.LogWarning(name + " 尝试切换角色，但没有任何角色变化");
        return; //若没有任何角色变化，则不进行任何操作
      }
      //基于当前角色的所有上下文记录，制作一份文档
      ChatDirectorContent targetActorDirectorMsg = 
        directorMsgs.Where(r => r.ActorID.Equals(CurrentChatRequest.ActorID, StringComparison.OrdinalIgnoreCase) && 
        r.PlayerID.Equals(CurrentChatRequest.PlayerID, StringComparison.OrdinalIgnoreCase)).First();
      
      ChatRequestData requestRecord = 
        ChatHelper.GetRequestRecords(null, CurrentStoryPreset.DefaultTargetActor.ActorGuid, CurrentStoryPreset.DefaultPlayerActor.ActorGuid, CurrentStoryPreset.StoryID).FirstOrDefault();
      if (requestRecord == null) {
        Debug.LogWarning("切换角色中，当前角色没有任何对话请求记录");
        return; 
      }

      List<ChatResponseData> responseRecords = ChatHelper.GetResponseRecords(null, CurrentStoryPreset.DefaultTargetActor.ActorGuid, CurrentStoryPreset.DefaultPlayerActor.ActorGuid, CurrentStoryPreset.StoryID);
      if (responseRecords == null) {
        Debug.LogWarning("切换角色中，当前角色没有任何对话响应记录");
        return;
      }

      StringBuilder directorMessage = new StringBuilder();
      ChatRequest chatRequest = new ChatRequest(requestRecord);
      if (chatRequest.Messages == null || chatRequest.Messages.Count == 0) {
        Debug.LogWarning("切换角色中，当前角色没有任何请求对话消息");
        return; //若当前对话请求没有任何消息，则不进行任何操作
      }

      List<ChatMessage> messages = chatRequest.Messages;

      foreach (ChatResponseData responseRecord in responseRecords) {
        List<ChatContextData> responseContextRecords = ChatHelper.GetContextRecords(null, responseRecord.ID);
        if (responseContextRecords != null) {
          for (int i = 0; i < responseContextRecords.Count; i++) {
            ChatMessage choice = new ChatMessage(responseContextRecords[i]);
            messages.Add(choice);
          }
        }
      }
      messages = messages.OrderBy(o =>o.Index).ToList();

      Actor currentChatActor = new Actor(CurrentStoryPreset.DefaultTargetActor);
      Actor currentPlayerActor = new Actor(CurrentStoryPreset.DefaultPlayerActor);
      foreach (ChatMessage message in messages) {
        if (message.Role == ChatDictionary.MESSAGE_ROLE.Assistant.Description()) {
          Debug.Log("response: " + message.Result + " > " + message.Index);
          string[] responseArray = message.Result.Split(new[] { "\r\n\r\n", "\n\n", "\n", "\\n\\n", "\\\n\\\n" }, StringSplitOptions.None);
          directorMessage.AppendLine(currentChatActor.LocalizedName + "说： " + responseArray.FirstOrDefault());
        } else if (message.Role == ChatDictionary.MESSAGE_ROLE.User.Description()) {
          Debug.Log("request: " + message.Result + " > " + message.Index);
          directorMessage.AppendLine(currentPlayerActor.LocalizedName + "说： " + message.Result);
        }
      }
      //更新当前对话请求的导演卡信息
      targetActorDirectorMsg.Content = directorMessage.ToString();

      //重构当前对话请求的角色信息
      InitializeStoryActorsRequest(actorPreset, playerPreset);

      OnActorsSwitched.Invoke();
    }
    public void ResetStoryContext(ChatStoryPreset storyPreset = null) {
      if (storyPreset == null) storyPreset = CurrentStoryPreset;
      foreach (ActorPreset actorPreset in storyPreset.Actors) {
        ChatHelper.ResetActorContext(actorPreset.ActorGuid, storyPreset.DefaultPlayerActor.ActorGuid, storyPreset.StoryID); //数据层面的清理
      }
    }
    /// <summary>
    /// 提交当前对话请求到OpenAI API，并保存返回的对话记录到数据库中。
    /// </summary>
    /// <param name="ignoreResponse">为true时，将不会记录当前返回内容，仅仅作为数据输入 （注：max_token = 0 应该能够迫使open ai 不返回任何内容，但目前失效）</param>
    /// <returns></returns>
    public async Task SubmitRequest(bool ignoreResponse = false) {
      if (CurrentChatRequest == null) {
        Debug.LogError("当前故事请求为null，无法提交请求");
        return;
      }
      if (CurrentChatRequest.Messages == null || CurrentChatRequest.Messages.Count == 0) {
        Debug.LogError("当前故事请求没有任何消息，无法提交请求");
        return;
      }
      if (enableDebugger) Debug.Log(name + " 提交新信息: " + IsSubmitting);
      if (IsSubmitting) {
        Debug.Log(name + " 正在提交对话信息，请稍后再次提交");
        //return null;
      }

      IsSubmitting = true;
      if (enableDebugger) Debug.Log(name + " 提交了 " + JsonConvert.SerializeObject(CurrentChatRequest));
      //提交请求到OpenAI API
      ChatResponse response = await ChatHelper.SubmitRequest(CurrentChatRequest);
      response.ActorID = CurrentChatRequest.ActorID;
      response.PlayerID = CurrentChatRequest.PlayerID;
      response.StoryID = CurrentChatRequest.StoryID;
      if (enableDebugger) Debug.Log(JsonConvert.SerializeObject(new ChatResponseData(response)));
      if (ignoreResponse == false) {
        ChatHelper.CreateResponseRecord(new ChatResponseData(response));
        ChatContextData contextData = new ChatContextData(response.Choices[0], response.ID); //在未开发多选择回复功能前，仅使用第一个Choice的内容
        //保存返回的对话记录到数据库
        int contextID = ChatHelper.CreateContextRecord(contextData);
        //保存返回的对话内容到数据库
        ChatContentData contentData = new ChatContentData(contextID, response.Choices[0].Message.Contents.First());
        contentData.ID = ChatHelper.CreateContentRecord(contentData);
        //构筑并更新当前请求模型
        CurrentChatRequest.Messages.Add(new ChatMessage(contextData));
      }

      IsSubmitting = false;

      //Debug.Log("finished");
      //return response;
    }
    private void createChatContextRecord(ChatMessage message, ChatRequest chatRequest = null) {
      if (CurrentChatRequest == null && chatRequest == null) {
        Debug.LogError("当前的chat 请求为空，无法记录新的 message");
        return;
      }

      ChatContextData conextData = new ChatContextData(message, chatRequest == null ? CurrentChatRequest.ID: chatRequest.ID);
      //保存到数据库,并获取新记录的ID
      if (enableDebugger) Debug.Log(JsonConvert.SerializeObject(conextData));
      message.ID = ChatHelper.CreateContextRecord(conextData);
      //保存消息内容到数据库
      List<ChatContentData> contentRecords = new List<ChatContentData>();
      foreach (ChatContent content in message.Contents) {
        //Debug.Log(message.Index + " >>>>> " + content.Text);
        ChatHelper.CreateContentRecord(new ChatContentData(message.ID, content));
      }
    }
#if UNITY_EDITOR
    [SerializeField] private bool resetContext;

    private void OnValidate() {
      if (resetContext) {
        resetContext = false;

        ResetStoryContext();
      }
    }
#endif
  }
}
