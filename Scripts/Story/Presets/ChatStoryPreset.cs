using System;
using System.Collections.Generic;
using UnityEngine;
using Halabang.Editor;
using Halabang.Utilities;
using System.Xml;
using System.Linq;

namespace Halabang.Story {
  [CreateAssetMenu(fileName = "story-chat_", menuName = "Geminum/Story/Story Chat Preset")]
  public class ChatStoryPreset : ScriptableObject {
    public string StoryID => uniqueID;
    public CopywritingPreset StoryContent => story;
    public CopywritingPreset StoryChatRules => chatRules;
    public List<CopywritingCollection> ResponseOptions => responseOptions;
    public ActorPreset DefaultPlayerActor => defaultPlayerActor;
    public ActorPreset DefaultTargetActor => defaultChatActors;
    public IEnumerable<ActorPreset> Actors => responseOptions == null ? null : responseOptions.GroupBy(g => g.ChatActor).Select(r => r.First().ChatActor);


    [Helpbox("问答文本规范：Title 为问答标题，Brief为问答内容，Description为规范当前问答的额外规则。", HelpboxAttribute.MessageType.Info)]
    [ReadOnly]
    [SerializeField] private string uniqueID;
    [Tooltip("故事内容")]
    [SerializeField] private CopywritingPreset story;
    [Tooltip("基于当前故事内容，全局性的规范对话规则")]
    [SerializeField] private CopywritingPreset chatRules;
    [Tooltip("当前故事里玩家默认扮演的角色")]
    [SerializeField] private ActorPreset defaultPlayerActor;
    [Tooltip("当前故事里其AI莫仍扮演的角色")]
    [SerializeField] private ActorPreset defaultChatActors;

    [Header("固定的问答文本")]
    [SerializeField] private List<CopywritingCollection> responseOptions;

#if UNITY_EDITOR
    [ContextMenu("Generate Guid")]
    public void GenerateUniqueID() {
      if (ValidationHelper.IsGuid(uniqueID) == false) {
        uniqueID = Guid.NewGuid().ToString();
      }
    }
#endif
  }

  [Serializable]
  public class CopywritingCollection {
    public string Name;
    public ActorPreset ChatActor;
    public ActorPreset PlayerActor;
    public List<CopywritingPreset> Contents;
  }
}