using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Halabang.Game;
using System.Linq;
using Halabang.Utilities;
using UnityEngine.TextCore.Text;
using Halabang.Story;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEditor.PackageManager.Requests;

namespace Halabang.Plugin {
  public class ChatHelper {
    private static readonly HttpClient httpClient = new HttpClient();

    #region API
    public static async Task<ChatResponse> SubmitRequest(ChatRequest request) {
      return await getResponse(request);
    }
    private static async Task<ChatResponse> getResponse(ChatRequest request) {
      // 若没有配置环境变量，请用您子业务空间的百炼API Key将下行替换为：string? apiKey = "sk-xxx";
      string? apiKey = "sk-7231280ec15f4ca3b2ac09c863026ce3";

      if (string.IsNullOrEmpty(apiKey)) {
        Debug.Log("API Key 未设置。请确保环境变量 'DASHSCOPE_API_KEY' 已设置。");
        return null;
      }

      // 设置请求 URL 和内容
      string url = "https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions";

      string messageContent = JsonConvert.SerializeObject(request);
      Debug.Log(messageContent);

      //return null;

      //string jsonContent = @"{
      //      ""model"": ""qwen-plus"",
      //      ""messages"": [
      //          {
      //              ""role"": ""system"",
      //              ""content"": ""你扮演一个物理科学家.""
      //          },
      //          {
      //              ""role"": ""user"", 
      //              ""content"": ""你是谁，你能做什么？""
      //          }
      //      ]
      //  }";

      //string jsonContent = @"{
      //  ""model"": ""qwen-plus-character"",
      //  ""messages"": [
      //    {
      //      ""role"": ""system"",
      //      ""content"": [
      //        {
      //          ""type"": ""text"",
      //          ""text"": ""你将扮演一位穿梭在魔法世界和凡人世界的女巫，你的名字叫布莱尔。\n接下来我会描述一下女巫布莱尔这个角色，同时会给你一些扮演出云的规则和建议。"",
      //          ""image_url"": null
      //        }
      //      ]
      //    }
      //  ],
      //  ""stream"": false
      //}";

//      string jsonContent = @"{
//  ""model"": ""qvq-max"",
//  ""messages"": [
//    {
//      ""role"": ""user"",
//      ""content"": [
//        {
//          ""type"": ""image_url"",
//          ""text"": null,
//          ""image_url"": {
//            ""url"": ""https://img.alicdn.com/imgextra/i1/O1CN01gDEY8M1W114Hi3XcN_!!6000000002727-0-tps-1024-406.jpg""
//          }
//        },
//        {
//          ""type"": ""image_url"",
//          ""text"": null,
//          ""image_url"": {
//            ""url"": ""https://img.alicdn.com/imgextra/i1/O1CN01ukECva1cisjyK6ZDK_!!6000000003635-0-tps-1500-1734.jpg""
//          }
//        },
//        {
//          ""type"": ""text"",
//          ""text"": ""根据我提供的图的顺序，请给我编一个故事，你不需要给我任何选择，直接输出故事内容即可"",
//          ""image_url"": null
//        }
//      ]
//    }
//  ],
//  //""max_completion_tokens"": 0,
//  ""stream"": true,
//  ""stream_options"": {
//    ""include_usage"": true
//  }
//}";
      //Debug.Log(jsonContent);
      // 发送请求并获取响应
      string result = await sendPostRequestAsync(url, messageContent, apiKey);
      Debug.Log("xxxx " + result);

      ChatResponse response = new ChatResponse();
      try {
        if (request.IsStream) {

          string[] chunks = result.Split("data: ", StringSplitOptions.RemoveEmptyEntries);
          if (chunks.Length == 0) return null;

          for (int i = 0; i < chunks.Length; i++) {
            string cleanedChunk = chunks[i].Trim();
            cleanedChunk = cleanedChunk.Replace("data", "");

            try {
              ChatResponse chunk = JsonConvert.DeserializeObject<ChatResponse>(cleanedChunk);
              if (i == 0) {
                //首个碎片，建立新的 ChatResponse 对象
                response = chunk;
              } else if (i == chunks.Length - 1) {
                //最后一个碎片，初始化 ChatResponse 对象
                response.Created = chunk.Created;
                response.SystemFingerprint = chunk.SystemFingerprint;
                if (chunk.Choices != null && chunk.Choices.Count > 0) response.Choices.Add(chunk.Choices[0]);
              } else {
                //中间的碎片，将碎片response的choice提取，添加进 response 的 choice 列表中
                if (chunk.Choices != null && chunk.Choices.Count > 0) response.Choices.Add(chunk.Choices[0]);
              }
            } catch (JsonException ex) {
              Debug.Log(chunks[i] + " is not a valid chat response");
            }
          }

          string chunkedReasoning = "";
          //当stream 为 true 时，可能会有多个 Choice，
          foreach (ChatCompletionChoice choice in response.Choices) {
            chunkedReasoning += choice.Delta.ReasoningContent;
            //将碎片中的choice碎片内容合并到 response 的首个choice 中
            response.Choices[0].Message.Result += choice.Delta.Content;
          }
          Debug.Log("content: " + response.Choices[0].Message.Result);
          Debug.Log(chunkedReasoning);

          return response;
        } else {
          response = JsonConvert.DeserializeObject<ChatResponse>(result);
          response.Choices[0].Message.Result = response.Choices[0].Message.Contents[0].Text;
        }
      } catch (Exception ex) {
        Debug.Log("错误：无法转换成 ChatResponse 类，原始信息" + result);
        Debug.Log("错误：无法转换成 ChatResponse 类，出错信息 " + ex);
        return null;
      }

      return response;
    }
    private static async Task<string> sendPostRequestAsync(string url, string jsonContent, string apiKey) {
      using (var content = new StringContent(jsonContent, Encoding.UTF8, "application/json")) {
        // 设置请求头
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.Timeout = new TimeSpan(0, 3, 0);
        //Debug.Log("timeout = " + httpClient.Timeout);
        //httpClient.Timeout = 

        // 发送请求并获取响应
        HttpResponseMessage response = await httpClient.PostAsync(url, content);

        // 处理响应
        if (response.IsSuccessStatusCode) {
          return await response.Content.ReadAsStringAsync();
        } else {
          //Debug.Log(response.Content.ReadAsStringAsync() + " : " + response.ReasonPhrase);
          return $"request failed: {response.StatusCode}";
        }
      }
    }
    #endregion

    #region DATABASE_CRUD
    public static void ResetActorContext(string actorGuid, string playerGuid, string storyID) {
      IEnumerable<ChatResponseData> responses = GetResponseRecords(null, actorGuid, playerGuid, storyID);
      IEnumerable<ChatRequestData> requests = GetRequestRecords(null, actorGuid, playerGuid, storyID);

      if (responses != null) {
        foreach (ChatResponseData response in responses) {
          IEnumerable<ChatContextData> contexts = GetContextRecords(null, response.ID);
          Debug.Log("responsed " + contexts + " for " + response.ID);
          if (DeleteContextRecords(contexts)) {
            DeleteResponseRecord(response);
          }
        }
      }
      //Debug.Log(requests.FirstOrDefault());
      if (requests != null) {
        foreach (ChatRequestData request in requests) {
          IEnumerable<ChatContextData> contexts = GetContextRecords(request.ID, null);
          //Debug.Log("requested " + contexts + " for " + request.ID);
          if (DeleteContextRecords(contexts)) {
            DeleteRequestRecord(request);
          }
        }
      }

      IEnumerable<ChatContextData> undefined_contexts = GetContextRecords(actorGuid, null);
      if (undefined_contexts != null) {
        foreach (ChatContextData context in undefined_contexts) {
          GameManager.Instance._DatabaseManager.ChatContextDB.Delete(context);
        }
      }
    }
    public static bool CreateResponseRecord(ChatResponseData response) {
      return GameManager.Instance._DatabaseManager.ChatContextDB.Insert(response) > 0;
    }
    public static bool DeleteResponseRecord(ChatResponseData response) { 
      return GameManager.Instance._DatabaseManager.ChatContextDB.Delete(response) > 0;
    }
    public static bool UpdateResponseRecord(ChatResponseData response) {
      return false;
    }
    public static List<ChatResponseData> GetResponseRecords(string id, string actorGuid, string playerGuid, string storyGuid) {
      List<ChatResponseData> records = new List<ChatResponseData>(
        from record in GameManager.Instance._DatabaseManager.ChatContextDB.Table<ChatResponseData>()
        where (id == null || record.ID == id) && (actorGuid == null || record.ActorID == actorGuid) && (playerGuid == null || record.PlayerID == playerGuid) && (storyGuid == null || record.StoryID == storyGuid)
        select record);

      //Debug.Log(records.Count());
      return records;
    }

    public static int CreateContextRecord(ChatContextData context) {
      if (context == null) return -1;

      long id = -1;
      GameManager.Instance._DatabaseManager.ChatContextDB.Insert(context, out id);
      //Debug.Log("context record is created: " + id);
      return (int)id;
    }
    public static bool DeleteContextRecords(IEnumerable<ChatContextData> contexts) {
      bool deleted = true;
      foreach (ChatContextData context in contexts) {
        if (DeleteContentRecords(context.ID)) {
          deleted = DeleteContextRecord(context);
        } else {
          deleted = false;
        }
      }
      return deleted;
    }
    public static bool DeleteContextRecord(ChatContextData context) {
      return GameManager.Instance._DatabaseManager.ChatContextDB.Delete(context) > 0;
    }
    public static bool UpdateContextRecord(ChatContextData context) {
      return false;
    }
    public static List<ChatContextData> GetContextRecords(string requestID, string respondID) {
      List<ChatContextData> records = new List<ChatContextData>(
        from record in GameManager.Instance._DatabaseManager.ChatContextDB.Table<ChatContextData>()
        where (requestID == null || record.RequestID == requestID) && (respondID == null || record.ResponseID == respondID)
        select record);

      return records;
    }

    public static bool CreateRequestRecord(ChatRequestData request) {
      return GameManager.Instance._DatabaseManager.ChatContextDB.Insert(request) > 0;
    }
    public static bool DeleteRequestRecord(ChatRequestData request) {
      //Debug.Log("deleting " + JsonConvert.SerializeObject(request));
      return GameManager.Instance._DatabaseManager.ChatContextDB.Delete(request) > 0;
    }
    public static bool UpgradeRequestRecord(ChatRequestData request) { 
      return false;
    }
    public static List<ChatRequestData> GetRequestRecords(string id, string actorGuid, string playerGuid, string storyGuid) {
      List<ChatRequestData> records = new List<ChatRequestData>(
        from record in GameManager.Instance._DatabaseManager.ChatContextDB.Table<ChatRequestData>()
        where (id == null || record.ID == id) && (actorGuid == null || record.ActorID == actorGuid) && (playerGuid == null || record.PlayerID == playerGuid) && (storyGuid == null || record.StoryID == storyGuid)
        select record);

      //Debug.Log(records.Count());
      return records;
    }

    public static int CreateContentRecord(ChatContentData content) {
      if (content == null) return -1; //如果没有内容，则不需要新建，返回 -1
      long id = 0;
      GameManager.Instance._DatabaseManager.ChatContextDB.Insert(content, out id);
      return (int)id;
    }
    public static bool DeleteContentRecords(IEnumerable<ChatContentData>contents) {
      //Debug.Log("deleting " + JsonConvert.SerializeObject(request));
      if (contents == null) return false; //如果没有内容，则不需要删除，返回 false
      int count = 0;
      foreach (ChatContentData content in contents) {
        count += GameManager.Instance._DatabaseManager.ChatContextDB.Delete(content);
      }
      Debug.Log("Total deleted " + count.ToString());
      return count == contents.Count();
    }
    public static bool DeleteContentRecords(int contextID) {
      List<ChatContentData> records = GetConents(contextID);
      int count = 0;
      foreach (ChatContentData content in records) {
        count += GameManager.Instance._DatabaseManager.ChatContextDB.Delete(content);
      }
      Debug.Log("Total deleted " + count.ToString());
      return count == records.Count();
    }

    public static List<ChatContentData> GetConents(int contextID) {
      List<ChatContentData> records = new List<ChatContentData>(
        from record in GameManager.Instance._DatabaseManager.ChatContextDB.Table<ChatContentData>()
        where (record.ContextID == contextID)
        select record);

      return records;
    }
    #endregion

    #region COMMON
    public static string GetActorSystemMessage(ChatRequest request, Actor actor, bool isPlayer = false) {
      if (request == null || actor == null) return string.Empty;

      if (IsValidActor(actor) == false) {
        Debug.LogError("指定的角色 " + actor.LocalizedName + " 不符合对话角色扮演的要求，请检查角色卡设置");
        return string.Empty;
      }

      StringBuilder actorSystemMsg = new StringBuilder();

      //定义对话角色扮演的基础信息      
      if (string.IsNullOrWhiteSpace(actor.CurrentBasic?.Brief) == false) {
        actorSystemMsg.Append("接下来，" + (isPlayer ? "，我开始扮演故事里的" : "你开始扮演故事里的") + actor.CurrentBasic.Subtitle + "。" + actor.CurrentBasic.Brief);
        actorSystemMsg.Append(Environment.NewLine);
      }
      //定义对话角色扮演的个性描述
      if (string.IsNullOrWhiteSpace(actor.CurrentBasic?.Content) == false) {
        actorSystemMsg.Append(actor.CurrentBasic.Content);
        actorSystemMsg.Append(Environment.NewLine);
      }
      //定义对话角色扮演的经历或履历
      if (string.IsNullOrWhiteSpace(actor.CurrentResume?.Content) == false) {
        actorSystemMsg.Append(actor.CurrentResume.Content);
        actorSystemMsg.Append(Environment.NewLine);
      }
      //定义对话角色扮演的响应规则
      if (isPlayer == false && string.IsNullOrWhiteSpace(actor.CurrentRules?.Content) == false) {
        actorSystemMsg.Append(actor.CurrentRules.Content);
        actorSystemMsg.Append(Environment.NewLine);
      }
      //定义对话全局的响应规则
      if (isPlayer == false &&  string.IsNullOrWhiteSpace(actor.CurrentResponseRules?.Content) == false) {
        actorSystemMsg.Append(actor.CurrentResponseRules.Content);
        actorSystemMsg.Append(Environment.NewLine);
      }
      //Debug.Log("cccccccccccccccccccc : " + actorSystemMsg.ToString());
      return actorSystemMsg.ToString();
    }
    public static bool IsValidActor(Actor chatTarget) {
      StringBuilder msg = new StringBuilder();
      if (chatTarget == null || ValidationHelper.IsGuid(chatTarget.Guid) == false) msg.Append("指定的角色不存在");

      if (string.IsNullOrWhiteSpace(chatTarget.CurrentBasic.Brief)) msg.Append(" 指定的角色 " + chatTarget.LocalizedName + " 需要有限制AI扮演该角色的基础规则Brief");
      if (string.IsNullOrWhiteSpace(chatTarget.CurrentBasic.Content)) msg.Append(" 指定的角色 " + chatTarget.LocalizedName + " 需要有描述人物的基本（起始）信息");
      if (string.IsNullOrWhiteSpace(chatTarget.CurrentResume.Content)) msg.Append(" 指定的角色 " + chatTarget.LocalizedName + " 需要有描述人物的基础经历");
      if (string.IsNullOrWhiteSpace(chatTarget.CurrentRules.Content)) msg.Append(" 指定的角色 " + chatTarget.LocalizedName + " 需要有限制AI扮演该角色的基础规则内容");

      if (msg.Length > 0) Debug.LogError(msg);

      return msg.Length == 0;
    }
    #endregion
  }
}