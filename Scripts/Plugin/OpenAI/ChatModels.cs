using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using Halabang.Utilities;
using Halabang.Story;
using System.Text;
using System.Linq;
using Halabang.Character;

namespace Halabang.Plugin {
  public class ChatRequest {
    [JsonProperty("model")]
    public string Model { get; set; }
    [JsonProperty("messages")]
    public List<ChatMessage> Messages { get; set; }
    //[JsonProperty("max_completion_tokens")]
    //public int MaxTokens { get; set; }
    [JsonProperty("stream")]
    public bool IsStream { get; set; }
    //[JsonProperty("stream_options")]
    //public StreamOption StreamOption { get; set; }

    [JsonIgnore]
    public string ID { get; set; }
    [JsonIgnore]
    public string ActorID { get; set; } //当前请求中，AI扮演的角色ID
    [JsonIgnore]
    public string PlayerID { get; set; } //当前请求中，玩家扮演的角色ID
    [JsonIgnore]
    public string StoryID { get; set; }
    [JsonIgnore]
    public long Created { get; set; }

    public ChatRequest() { 
      Messages = new List<ChatMessage>();
      Created = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
    public ChatRequest(ChatRequestData data) {
      ID = data.ID;
      ActorID = data.ActorID;
      PlayerID = data.PlayerID;
      StoryID = data.StoryID;
      Model = ((ChatDictionary.CHAT_MODEL)data.Model).Description();
      Created = data.Created; //DateTimeOffset.UtcNow.ToUnixTimeSeconds();

      Messages = new List<ChatMessage>();
      List<ChatContextData> contextRecords = ChatHelper.GetContextRecords(ID, null);
      if (contextRecords != null) {
        for (int i = 0; i < contextRecords.Count; i++) {
          Messages.Add(new ChatMessage(contextRecords[i]));
        }
      }

      //MaxTokens = 2048; // Default value, can be adjusted
    }
    public void UpdateSystemMessage(string content) {
      if (Messages == null || Messages.Count == 0) return;
      if (Messages.First().Contents == null || Messages.First().Contents.Count == 0) return;

      Messages.First().Contents.First().Text += (Environment.NewLine + content);
    }
    public void DeleteMessage(int contextID) {
      deleteMessage(contextID);
    }
    public void DeleteMessage(ChatMessage message) {
      if (message == null) return;
    }
    public void DeleteMessage(ChatDictionary.MESSAGE_TYPE messageType) {
      IEnumerable<ChatMessage> actorSettingMessages = Messages.Where(m => m.MessageType == messageType);
      if (actorSettingMessages != null) {
        foreach (ChatMessage message in actorSettingMessages) {
          deleteMessage(message.ID);
        }
      }
    }
    private void deleteMessage(int id) {
      if (Messages == null) return;
      ChatMessage message = Messages.Where(r => r.ID == id).FirstOrDefault();
      if (ChatHelper.DeleteContextRecord(new ChatContextData(message, ID))) {
        Messages.Remove(message);
      }
    }
  }
  public class ChatResponse {
    [JsonProperty("id")]
    public string ID { get; set; }
    [JsonProperty("object")]
    public string ObjectType { get; set; }
    [JsonProperty("model")]
    public string Model { get; set; }
    [JsonProperty("choices")]
    public List<ChatCompletionChoice> Choices { get; set; }
    public TokenUsage Usage { get; set; }
    [JsonProperty("created")]
    public long Created { get; set; }
    [JsonProperty("system_fingerprint")]
    public string SystemFingerprint { get; set; }

    [JsonIgnore]
    public string ActorID { get; set; } //当前请求中，AI扮演的角色ID
    [JsonIgnore]
    public string PlayerID { get; set; } //当前请求中，玩家扮演的角色ID
    [JsonIgnore]
    public string StoryID { get; set; }

    public ChatResponse() {
      Choices = new List<ChatCompletionChoice>();
      Created = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
  }
  public class ChatCompletionChoice {
    [JsonProperty("index")]
    public int Index { get; set; }
    [JsonProperty("message")]
    public ChatMessage Message { get; set; }
    [JsonProperty("finish_reason")]
    public string FinishReason { get; set; }
    [JsonProperty("delta")]
    public ChatCompletionChoiceDelta Delta { get; set; }

    public ChatCompletionChoice() {
      Message = new ChatMessage();
    }
  }
  public class ChatCompletionChoiceDelta {
    [JsonProperty("content")]
    public string Content { get; set; }
    [JsonProperty("reasoning_content")]
    public string ReasoningContent { get; set; }
  }
  public class ChatMessage {
    [JsonProperty("role")]
    public string Role { get; set; } // "system", "user", "assistant", etc.
    [JsonProperty("content")]
    [JsonConverter(typeof(ChatContentConverter))]
    public List<ChatContent> Contents { get; set; }

    [JsonIgnore]
    public int ID { get; set; } //对应数据库里的Context ID
    [JsonIgnore]
    public int Index { get; set; } //在回复列表中的索引位置
    [JsonIgnore]
    public string Result { get; set; }
    [JsonIgnore]
    public CharacterEmotion.EMOTION_STATE Emotion { get; set; }
    [JsonIgnore]
    public List<string> ResponseOptions { get; set; }
    [JsonIgnore]
    public ChatDictionary.MESSAGE_TYPE MessageType { get; set; }

    public ChatMessage() { 
      Contents = new List<ChatContent>();
    }
    public ChatMessage(ChatContextData data) {
      Role = ((ChatDictionary.MESSAGE_ROLE)data.Role).Description();
      Contents = new List<ChatContent>();
      Index = data.ID;
      Emotion = (CharacterEmotion.EMOTION_STATE)data.Emotion;
      ResponseOptions = data.ResponseOptions;
      MessageType = (ChatDictionary.MESSAGE_TYPE)data.ContextType;

      List<ChatContentData> chatContents = ChatHelper.GetConents(data.ID);
      if (chatContents != null) {
        StringBuilder contentResult = new StringBuilder();
        foreach (ChatContentData contentData in chatContents) {
          Contents.Add(new ChatContent(contentData));
          if (contentData.ContentType == (int)ChatDictionary.CONTENT_TYPE.TEXT) {
            contentResult.Append(contentData.Content);
          }
        }
        Result = contentResult.ToString();
      }
    }
  }
  public class ChatContent {
    [JsonProperty("type")]
    public string ContentType { get; set; }
    [JsonProperty("text")]
    public string Text { get; set; }
    [JsonProperty("image_url")]
    public ImageUrl ImageURL { get; set; }

    public ChatContent() { }
    public ChatContent(ChatDictionary.CONTENT_TYPE contentType, string content) {
      ContentType = contentType.Description();
      switch (contentType) {
        case ChatDictionary.CONTENT_TYPE.TEXT:
          Text = content;
          break;
        case ChatDictionary.CONTENT_TYPE.IMAGE_URL:
          ImageURL = new ImageUrl { URL = content };
          break;
      }
    }
    public ChatContent(ChatContentData data) {
      ContentType = ((ChatDictionary.CONTENT_TYPE)data.ContentType).Description();
      if (data.ContentType == (int)ChatDictionary.CONTENT_TYPE.TEXT) {
        Text = data.Content;
      } else if (data.ContentType == (int)ChatDictionary.CONTENT_TYPE.IMAGE_URL) {
        ImageURL = new ImageUrl { URL = data.Content };
      }
    }
  }
  public class ImageUrl {
    [JsonProperty("url")]
    public string URL { get; set; }
  }
  public class TokenUsage {
    [JsonProperty("prompt_tokens")]
    public int PromptTokens { get; set; }
    [JsonProperty("completion_tokens")]
    public int CompletionTokens { get; set; }
    [JsonProperty("total_tokens")]
    public int TotalTokens { get; set; }
  }
  public class StreamOption {
    [JsonProperty("include_usage")]
    public bool IncludeUsage { get; set; }
  }
  public class ChatDictionary {
    public enum MESSAGE_TYPE {
      Chat, //普通对话类
      StorySetting, //故事背景，人物角色设定
      DirectorSetting, //导演卡，基于实时的对话推进的关键信息
      SingleChatSetting //单次对话设定类
    }
    public enum MESSAGE_ROLE {
      [Description("system")] System,
      [Description("user")] User,
      [Description("assistant")] Assistant,
      [Description("tool")] Tool
    }
    public enum CHAT_MODEL {
      [Description("qwen-plus")] QwenPlus,
      [Description("qwen-plus-character")] QwenPlusCharacter, //角色扮演
      [Description("qvq-max")] QvqMax, //视觉推理
      [Description("qwen-vl-max")] QvqVlMax, //视觉理解-强-高成本
      [Description("qwen-vl-plus")] QvqVlPlus, //视觉理解-均衡
    }
    public enum FINISH_REASON {
      [Description("null")] Null,
      [Description("stop")] Stop,
      [Description("length")] Length,
      [Description("content_filter")] ContentFilter,
      [Description("tool_calls")] ToolCalls,
      [Description("function_call")] FunctionCall
    }
    public enum CONTENT_TYPE {
      [Description("text")] TEXT,
      [Description("image_url")] IMAGE_URL
    }
    //基于回复内容的 Regex 定义
    public const string EMOTION_PATTERN = @"<<<(.*?)>>>";
    public const string IGNORE_BRACKETS_PATTERN = @"[\(（][^）\)]*[\)）]";
    public const string COLON_INDEX_PATTERN = @"^.{1}[:：]\s*";
    public const string PERIOD_INDEX_PATTERN = @"^.*?\.\s*";
  }

  public class ChatDirectorContent {
    public string ActorID { get; set; }
    public string PlayerID { get; set; }
    public string Content { get; set; }

    public ChatDirectorContent() { }
  }
  public class ChatContentConverter : JsonConverter<List<ChatContent>> {
    public override List<ChatContent> ReadJson(JsonReader reader, Type objectType, List<ChatContent> existingValue, bool hasExistingValue, JsonSerializer serializer) {
      if (reader.TokenType == JsonToken.String) {
        // Case: "content": "Hello!"
        string text = (string)reader.Value;
        return new List<ChatContent> { new ChatContent { ContentType = ChatDictionary.CONTENT_TYPE.TEXT.Description(), Text = text } };
      } else if (reader.TokenType == JsonToken.StartArray) {
        // Case: "content": [ { "type": "text", "text": "..." } ]
        return serializer.Deserialize<List<ChatContent>>(reader);
      }

      throw new JsonSerializationException("Unexpected token type for 'content'");
    }

    public override void WriteJson(JsonWriter writer, List<ChatContent> value, JsonSerializer serializer) {
      // Serialize as an array always (optional: you can make this adaptive too)
      serializer.Serialize(writer, value);
    }
  }
}