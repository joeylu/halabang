using UnityEngine;
using Halabang.Story;
using System;
using Halabang.Utilities;
using UnityEngine.UI;
using Halabang.Character;

namespace Halabang.Plugin {
  public class ChatRecord : MonoBehaviour {
    [SerializeField] private TextMeshExtend titleText;
    [SerializeField] private TextMeshExtend recordText;
    [SerializeField] private Image emojiImage;

    [Header("Emoji Images")]
    [SerializeField] private Sprite emojiNeutral;
    [SerializeField] private Sprite emojiSad;
    [SerializeField] private Sprite emojiVerySad;
    [SerializeField] private Sprite emojiHappy;
    [SerializeField] private Sprite emojiVeryHappy;


    public void SetRecord(ChatMessage message, Actor actor = null) {
      //Debug.Log(response.Choices[0].Message.Role + " >>> " + response.Choices[0].Message.Content);
      //if (actor != null) Debug.Log(actor.LocalizedName + " : " + context.Role);
      string title = "Unknown";
      if (message.Role == ChatDictionary.MESSAGE_ROLE.Assistant.Description()) {
        //是AI助手回复
        if (actor != null) {
          title = actor.LocalizedName;
        } else {
          title = ChatDictionary.MESSAGE_ROLE.Assistant.Description();
        }
      } else if (message.Role == ChatDictionary.MESSAGE_ROLE.User.Description()) {
        title = "玩家";
      } else {
        title = message.Role;
      }

      titleText.SetText(title);
      recordText.SetText(message.Result);

      CharacterEmotion.EMOTION_STATE emotion = message.Emotion;
      switch (emotion) {
        //case ActorDictionary.ACTOR_EMOTION.NEUTRAL:
        //  emojiImage.sprite = emojiNeutral;
        //  break;
        //case ActorDictionary.ACTOR_EMOTION.HAPPY:
        //  emojiImage.sprite = emojiHappy;
        //  break;
        //case ActorDictionary.ACTOR_EMOTION.VERY_HAPPY:
        //  emojiImage.sprite = emojiVeryHappy;
        //  break;
        //case ActorDictionary.ACTOR_EMOTION.SAD:
        //  emojiImage.sprite = emojiSad;
        //  break;
        //case ActorDictionary.ACTOR_EMOTION.VERY_SAD:
        //  emojiImage.sprite = emojiVerySad;
        //  break;
        default:
          emojiImage.sprite = emojiNeutral;
          break;
      }
    }
  }
}