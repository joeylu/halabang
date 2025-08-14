using System.Collections.Generic;
using UnityEngine;
using Halabang.Utilities;
using System;

namespace Halabang.Plugin {
  public class ChatTest : MonoBehaviour {
    [SerializeField] private SpriteRenderer sprite01;
    public SpriteRenderer sprite02;
    public string spriteEmotion01;
    public string spriteEmotion02;
    public string storyType;

    public bool submit;

    private void OnValidate() {
      if (submit) {
        submit = false;

        ChatRequest chatRequest = new ChatRequest();
        chatRequest.Model = ChatDictionary.CHAT_MODEL.QvqMax.Description();
        chatRequest.Messages = new List<ChatMessage>();
        chatRequest.ID = Guid.NewGuid().ToString();
        chatRequest.ActorID = Guid.NewGuid().ToString();
        chatRequest.IsStream = true;
        //chatRequest.StreamOption = new StreamOption();
        //chatRequest.StreamOption.IncludeUsage = true;

        ChatMessage message = new ChatMessage();
        message.Role = ChatDictionary.MESSAGE_ROLE.User.Description();

        ChatContent detail01 = new ChatContent();
        detail01.ContentType = ChatDictionary.CONTENT_TYPE.IMAGE_URL.Description();
        //detail01.ImageURL = new ImageUrl { URL = "https://img.alicdn.com/imgextra/i1/O1CN01gDEY8M1W114Hi3XcN_!!6000000002727-0-tps-1024-406.jpg" };
        detail01.ImageURL = new ImageUrl { URL = $"data:image/png;base64,{GenericUtilities.ConvertSpriteToBase64(sprite01.sprite)}" };
        message.Contents.Add(detail01);

        ChatContent emotion01 = new ChatContent();
        emotion01.ContentType = ChatDictionary.CONTENT_TYPE.TEXT.Description();
        emotion01.Text = "第一张图片的关键词是" + spriteEmotion01;
        message.Contents.Add(emotion01);

        ChatContent detail02 = new ChatContent();
        detail02.ContentType = ChatDictionary.CONTENT_TYPE.IMAGE_URL.Description();
        //detail02.ImageURL = new ImageUrl { URL = "https://img.alicdn.com/imgextra/i1/O1CN01ukECva1cisjyK6ZDK_!!6000000003635-0-tps-1500-1734.jpg" }; // GenericUtilities.ConvertSpriteToBase64(sprite02.sprite);
        detail02.ImageURL = new ImageUrl { URL = $"data:image/png;base64,{GenericUtilities.ConvertSpriteToBase64(sprite02.sprite)}" }; // GenericUtilities.ConvertSpriteToBase64(sprite01.sprite);
        message.Contents.Add(detail02);

        ChatContent emotion02 = new ChatContent();
        emotion02.ContentType = ChatDictionary.CONTENT_TYPE.TEXT.Description();
        emotion02.Text = "第一张图片的关键词是" + spriteEmotion02;
        message.Contents.Add(emotion02);

        ChatContent detailRequest = new ChatContent();
        detailRequest.ContentType = ChatDictionary.CONTENT_TYPE.TEXT.Description();
        detailRequest.Text = "根据我提供的图的顺序，以中文形式，请给我编一个故事，你不需要给我任何选择，直接输出故事内容即可";
        message.Contents.Add(detailRequest);
        ChatContent detailFinal = new ChatContent();
        detailFinal.ContentType = ChatDictionary.CONTENT_TYPE.TEXT.Description();
        detailFinal.Text = "请保持平均每一张图的剧情描述不超过100字，另外整个故事的类型应该是"+ storyType;
        message.Contents.Add(detailFinal);

        chatRequest.Messages.Add(message);

        submitRequest(chatRequest);




        //string testStr = "{\"choices\":[{\"delta\":{\"content\":null,\"reasoning_content\":\"cm\"},\"finish_reason\":null,\"index\":0,\"logprobs\":null}],\"object\":\"chat.completion.chunk\",\"usage\":null,\"created\":1753701648,\"system_fingerprint\":null,\"model\":\"qvq-max\",\"id\":\"chatcmpl-76561f3f-d248-95a2-b1ef-0d4a65a8e767\"}";

        //ChatResponse responseList = JsonConvert.DeserializeObject<ChatResponse>(testStr);
        //Debug.Log(responseList.ID);

      }
    }
    private async void submitRequest(ChatRequest chatRequest) {
      ChatResponse response = await ChatHelper.SubmitRequest(chatRequest);
      //Debug.Log(response.Choices[0].Message);
    }
  }
}