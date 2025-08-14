using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using PixelCrushers.DialogueSystem;
using Halabang.Game;

namespace Halabang.Plugin {
  [RequireComponent(typeof(DialogueSystemTrigger))]
  public class DialogueTriggerController : MonoBehaviour {
    #region PROPERTIES
    public const string OBJECT_NAME = "dialogue_";
    //public Examinable CurrentExaminable { get; set; }
    //public CraftItemTransferSettings CurrentTransfer { get; set; }
    //public BasicProperty CurrentProperty { get; set; }
    //public AvatarController CurrentAvatarController { get; set; }
    //public CraftItemSlotUI CurrentSlot { get; set; }

    //public CraftLocalInventory CurrentInventory => inventory;
    //public CraftProgressController CurrentCraftControllers => craftableController;
    //public IEnumerable<string> CurrentPublicKeys => publicKeys;
    public DialogueSystemTrigger Trigger => trigger;
    public bool HasTriggered { get; set; } //set from dialogue controller startDialogueTriggerSequence when conversation is ended
    public bool IsTriggering { get; set; } //set from dialogue controller startDialogueTriggerSequence when is valid to trigger (may not be started), end in this.Clear()
    public Coroutine CurrentTransition { get; set; }
    #endregion
    #region SERIALIZED_FIELDS
    [Header("Trigger behaviors")]
    [SerializeField] private float triggerWithDelay;
    [Tooltip("Trigger only once in its lifetime")]
    [SerializeField] private bool triggerOnlyOnce;
    //[Tooltip("While in conversation, movement input has been disabled")]
    //public bool noMovementInputInConversation;
    //[Tooltip("Trigger current player out of examining action")]
    //public bool endExaminAfterConversation;
    //[Tooltip("Auto save the game after conversation is ended")]
    [SerializeField] private bool autoSaveAfterConversation;
    //[Tooltip("override playing music (BGM) volume while talking")]
    //[Range(-80, 20)]
    //public FloatParameter musicVolumeWhileTalking;
    //[Tooltip("override playing SFX volume while talking")]
    //[Range(-80, 20)]
    //public FloatParameter SFXVolumeWhileTalking;
    [Tooltip("Indicate this dialogue does not actually has conversations, voice logics such as transit dialogue mixer will be ignored")]
    [SerializeField] private bool menuOnly;
    [Tooltip("When ticked, No animation state will be assiciated with actor")]
    [SerializeField] private bool stateless;
    [Tooltip("When ticked, this dialogue will not be interrupted by new dialogue trigger")]
    [SerializeField] private bool uninterruptible;
    [SerializeField] private bool autoStart;
    [SerializeField] private List<TextMeshExtend> textComponents;
    [Header("Events")]
    public UnityEvent OnConversationStart;
    public UnityEvent OnConversationEnd;
    //[Tooltip("Trigger an event when slot chipset is getting filtered while in conversation")]
    //public UnityEvent onChipsetFiltering;
    //[Tooltip("Trigger an event when slot chipset or any public/private keys comparing methods started while in conversation")]
    //public UnityEvent onKeyComparing;
    [Header("Development only")]
    public bool enableDebugger;
    #endregion
    #region LOCAL_FIELDS
    private DialogueSystemTrigger trigger;
    //private CraftLocalInventory inventory;
    //private CraftProgressController craftableController;
    private List<string> publicKeys;
    private List<string> privateKeys;
    #endregion
    #region UNITY_METHODS
    private void Awake() {
      trigger = GetComponent<DialogueSystemTrigger>();
    }
    private void Start() {
      if (autoStart) OnUse();
    }
    #endregion
    #region PUBLIC_METHODS
    public void OnUse() {
      if (enableDebugger) Debug.Log(name + " is triggered on Use");
      GameManager.Instance._DialogueManger.TriggerDialogue(this);
    }
    public void OnUseWithDelay(float delay = 0) {
      GameManager.Instance._DialogueManger.TriggerDialogue(this, delay);
    }
    public TextMeshExtend GetInSceneTextComponent(string textMeshName) {
      if (textComponents == null) return null;

      if (string.IsNullOrWhiteSpace(textMeshName)) return textComponents.First();
      return textComponents.Where(r => r.name.Equals(textMeshName, System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    }

    //public void OnUseWithSlot(CraftItemSlotUI targetSlot) {
    //  if (targetSlot == null) {
    //    Debug.LogError(name + " cannot be triggered with slot because its target slot is null");
    //    return;
    //  }
    //  slotItem = targetSlot.CurrentSlotItem;
    //  setKeys();

    //  GameManager.instatnce.CurrentSceneManager.CurrentDialogueController.TriggerDialogue(this);
    //}
    //public void OnUseWithInventory(CraftLocalInventory targetInventory) {
    //  if (targetInventory == null) {
    //    Debug.LogError(name + " cannot be triggered with inventory because its target inventory is null");
    //    return;
    //  }
    //  inventory = targetInventory;
    //  GameManager.instatnce.CurrentSceneManager.CurrentDialogueController.TriggerDialogue(this);
    //}
    //public void OnUseWithCraftableController(CraftProgressController ctrl) {
    //  craftableController = ctrl;
    //  if (craftableController == null) {
    //    Debug.LogError(name + " cannot be triggerred with craftable controller with a null value ");
    //    return;
    //  }
    //  GameManager.instatnce.CurrentSceneManager.CurrentDialogueController.TriggerDialogue(this);
    //}
    //public bool CompareKeys(IEnumerable<string> matchingKeys) {
    //  onKeyComparing.Invoke();
    //  return ChipsetHelper.CompareKeys(publicKeys, matchingKeys);
    //}
    //public bool FilterChipset(IEnumerable<string> matchingChipsetCategories, IEnumerable<string> matchingChipsetFunctions) {
    //  onChipsetFiltering.Invoke();
    //  return ChipsetHelper.FilterChipset(CurrentSlot?.CurrentSlotItem?.ChipsetItem, matchingChipsetCategories, matchingChipsetFunctions);
    //}
    /// <summary>
    /// Called from basic dialogue component when conversation is ended
    /// </summary>
    public void Clear() {
      if (enableDebugger) Debug.Log(name + " is cleared");
      IsTriggering = false;
      //CurrentExaminable = null;
      //CurrentTransfer = null;
      //CurrentProperty = null;
      //CurrentAvatarController = null;
      //CurrentSlot = null;

      publicKeys = null;
      privateKeys = null;
      //inventory = null;
      //craftableController = null;
      if (HasTriggered && triggerOnlyOnce == false) HasTriggered = false;
      //GameManager.instatnce.CurrentSceneManager.CurrentDialogueController.TriggeringQueue.Remove(this);
    }
    #endregion
    #region DIALOGUE_METHODS
    //private void OnConversationStart() {
    //  if (GameManager.instatnce.DialogueBasic.CurrentDialogueController == this) {

    //  }
    //}
    #endregion
    #region LOCAL_METHODS
    //private void setKeys() {
    //  publicKeys = new List<string>();
    //  privateKeys = new List<string>();
    //  //set keys from slot item
    //  if (CurrentSlot?.CurrentSlotItem != null && CurrentSlot?.CurrentSlotItem?.ChipsetItem != null) {
    //    if (ValidationHelper.IsGuid(CurrentSlot?.CurrentSlotItem.ChipsetItem.PublicKey)) publicKeys.Add(CurrentSlot?.CurrentSlotItem.ChipsetItem.PublicKey);
    //    if (ValidationHelper.IsGuid(CurrentSlot?.CurrentSlotItem.ChipsetItem.PrivateKey)) privateKeys.Add(CurrentSlot?.CurrentSlotItem.ChipsetItem.PrivateKey);
    //  }
    //  //set keys from other... lately
    //}
    #endregion
#if UNITY_EDITOR
    private void OnValidate() {
      if (triggerOnlyOnce) {
        if (GetComponent<SceneDataSaver>() == null) Debug.LogError(name + " is a one time dialogue trigger but has no scene data saver bound");
      }
    }
#endif
  }
}