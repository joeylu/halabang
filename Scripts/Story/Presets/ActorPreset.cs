using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Halabang.Editor;
using Halabang.UI;

namespace Halabang.Story {
  [CreateAssetMenu(fileName = "actor_", menuName = "Geminum/Story/Actor Preset")]
  public class ActorPreset : ScriptableObject {
    public int ActorID => actorID;
    public string ActorGuid => actorGuid;
    //public UI_Icons_Setting Icons => icons;
    //public UI_Images_Setting Images => images;
    //public ItemPairPreset AvatarChip => avatarChip;
    public CopywritingPreset[] ActorSettings => actorSettings;
    public CopywritingPreset[] ActorResume => actorResume;
    public CopywritingPreset[] ActorRules => actorRules;
    public CopywritingPreset[] ResponseRules => responseRules;

    [Helpbox("DO NOT set addressable directly, as you do, it will BREAK the database record !!!", HelpboxAttribute.MessageType.Warning)]
    [ReadOnly]
    [SerializeField] private string warning;

    [Header("Basic settings")]
    //[ReadOnly]
    [SerializeField] private int actorID;
    //[ReadOnly]
    //[SerializeField] private int selectedActorID;
    //[ReadOnly]
    //[SerializeField] private string selectedActorName;
    [ReadOnly]
    [SerializeField] private string actorGuid;
    //[ReadOnly]
    //[SerializeField] private string displayName;
    //[ReadOnly]
    //[SerializeField] private string fullName;
    //[SerializeField] private string firstName;
    //[SerializeField] private string middleName;
    //[SerializeField] private string lastName;
    [ReadOnly]
    [SerializeField] private string actorNameEn;
    [ReadOnly]
    [SerializeField] private string actorNameCn;
    //[SerializeField] private UI_Icons_Setting icons;
    //[SerializeField] private UI_Images_Setting images;

    [Tooltip("描述人物的基本（起始）信息，比如性别，年龄，喜好等等基础信息")]
    [SerializeField] private CopywritingPreset[] actorSettings;
    [Tooltip("描述人物的基础经历，过往成就等")]
    [SerializeField] private CopywritingPreset[] actorResume;
    [Tooltip("AI 对话时，需要限制AI扮演该角色的基础规则")]
    [SerializeField] private CopywritingPreset[] actorRules;
    [Tooltip("AI 对话时，需要限制AI回复的基础规则")]
    [SerializeField] private CopywritingPreset[] responseRules;
    //[SerializeField] private ItemPairPreset avatarChip;

    //[Header("Story settings")]
    //[StoryDataRefresh]
    //[SerializeField] private bool refreshStoryData;
    //[StoryMomentSelector]
    //[SerializeField] private string[] timelines;
    //[StoryPlaceSelector]
    //[SerializeField] private string[] appearances;

    //[Header("Dialogue system settings")]
    //[SerializeField] private PixelCrushers.DialogueSystem.DialogueDatabase targetDialogueDatabase;

  }
}
