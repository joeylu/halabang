using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.VFX;
using Halabang.Utilities;
using Halabang.Plugin;

namespace Halabang.Game {
  public class SceneDataSaver : MonoBehaviour {
    public DialogueTriggerData SaverDialogueTriggerData => dialogueTriggerData;
    //public LightData SaverLightData => lightData;
    //public ReflectionProbeData SaverReflectionData => reflectionData;
    //public LightBeamData SaverLightBeamData => lightBeamData;
    //public ParticleData SaverParticleData => particleData;
    //public VisualEffectData SaverVisualEffectData => visualEffectData;
    public string UniqueID => uniqueID;

    [Header("Unique ID for identifications from serialized methods")]
    [Tooltip("if a previous ID has already been assigned, generate guid menu will not generate a new one")]
    //[ReadOnly]
    [SerializeField] string uniqueID;

    [Header("Development only")]
    [SerializeField] private bool updateDataReference;

    private DialogueTriggerData dialogueTriggerData;
    //private LightData lightData;
    //private ReflectionProbeData reflectionData;
    //private LightBeamData lightBeamData;
    //private ParticleData particleData;
    //private VisualEffectData visualEffectData;

    private Light lightModule;
    private DialogueTriggerController dialogueTriggerModule;
    private ReflectionProbe reflectionModule;
    //private VolumetricLightBeamAbstractBase lightBeamModule;
    //private VolumetricLightBeamHD lightBeamHD;
    //private VolumetricLightBeamSD lightBeamSD;
    private ParticleSystem particleModule;
    private VisualEffect visualEffectModule;

    public SaverData GetSaverData() {
      setAllData();

      //if (dialogueTriggerData == null && lightData == null && reflectionData == null && lightBeamData == null && particleData == null && visualEffectData == null) {
      //  //Debug.Log(name + " has a scene data saver but has no related module to be saved");
      //  return null;
      //}

      //getLightBeamData();
      //getLightData();
      getDialogueTriggerData();
      //getReflectionData();

      SaverData data = new SaverData();
      data.Name = name;
      data.ID = UniqueID;
      data.DefaultDialogueTriggerData = dialogueTriggerData;
      //data.DefaultLightData = lightData;
      //data.DefaultReflectionData = reflectionData;
      //data.DefaultLightBeamData = lightBeamData;
      //data.DefaultParticleData = particleData;
      //data.DefaultVisualEffectData = visualEffectData;

      return data;
    }
    public void ApplySaverData(SaverData data) {
      if (data == null) return;
      setAllData();

      //applyLightData(data.DefaultLightData);
      applyDialogueTriggerData(data.DefaultDialogueTriggerData);
      //applyLightBeamData(data.DefaultLightBeamData);
      //applyReflectionData(data.DefaultReflectionData);
      //applyVisualEffectData(data.DefaultVisualEffectData);
      //applyParticleData(data.DefaultParticleData);
    }

    private void setAllData() {
      //setLightBeamData();
      //setLightData();
      setDialogueTriggerData();
      //setReflectionData();
    }
    //private void setLightBeamData() {
    //  if (lightBeamData != null) return;

    //  lightBeamHD = GetComponent<VolumetricLightBeamHD>();
    //  lightBeamSD = GetComponent<VolumetricLightBeamSD>();
    //  lightBeamModule = lightBeamHD ? lightBeamHD : lightBeamSD;
    //  if (lightBeamHD || lightBeamSD) {
    //    lightBeamData = new LightBeamData();
    //  }
    //}
    //private void setLightData() {
    //  if (lightData != null) return;

    //  lightModule = GetComponent<Light>();
    //  if (lightModule) {
    //    lightData = new LightData();
    //  }
    //}
    private void setDialogueTriggerData() {
      if (dialogueTriggerData != null) return;

      dialogueTriggerModule = GetComponent<DialogueTriggerController>();
      if (dialogueTriggerModule) {
        dialogueTriggerData = new DialogueTriggerData();
      }
    }
    //private void setReflectionData() {
    //  if (reflectionData != null) return;

    //  reflectionModule = GetComponent<ReflectionProbe>();
    //  if (reflectionModule) {
    //    reflectionData = new ReflectionProbeData();
    //  }
    //}
    //private void setParticleData() {

    //}
    //private void setVisualEffectData() {

    //}

    //private void getLightBeamData() {
    //  if (lightBeamData != null) {
    //    lightBeamData.IsHD = lightBeamHD ?? false;
    //    lightBeamData.Name = name;
    //    lightBeamData.Activated = gameObject.activeSelf;
    //  }
    //}
    //private void getLightData() {
    //  if (lightData != null) {
    //    lightData.Name = name;
    //    lightData.Activated = gameObject.activeSelf;
    //    lightData.ColorValue = lightModule.color.ConvertToFloat();
    //    lightData.Intensity = lightModule.intensity;
    //    lightData.Range = lightModule.range;
    //  }
    //}
    private void getDialogueTriggerData() {
      if (dialogueTriggerData != null) {
        dialogueTriggerData.Name = name;
        dialogueTriggerData.IsTriggered = dialogueTriggerModule.HasTriggered;
      }
    }
    //private void getReflectionData() {
    //  if (reflectionData != null) {
    //    reflectionData.Name = name;
    //    reflectionData.Activated = gameObject.activeSelf;
    //  }
    //}
    //private void getParticleData() {

    //}
    //private void getVisualEffectData() {

    //}

    //private void applyLightBeamData(LightBeamData data) {

    //}
    //private void applyLightData(LightData data) {
    //  if (data == null) return;
    //  if (lightModule == null) return;

    //  if (data.Intensity >= 0) lightModule.intensity = data.Intensity;
    //  if (data.Range >= 0) lightModule.range = data.Range;
    //  if (data.ColorValue.Length == 4) lightModule.color = GenericUtilities.FloatToColor(data.ColorValue);

    //  //Debug.Log(name + " light data has applied");
    //}
    private void applyDialogueTriggerData(DialogueTriggerData data) {
      if (data == null) return;
      if (dialogueTriggerModule == null) return;
      dialogueTriggerModule.HasTriggered = data.IsTriggered;
    }
    //private void applyReflectionData(ReflectionProbeData data) {

    //}
    //private void applyParticleData(ParticleData data) {

    //}
    //private void applyVisualEffectData(VisualEffectData data) {

    //}

#if UNITY_EDITOR
    private void OnValidate() {
      updateEditorReference();
    }

    private void updateEditorReference() {
      if (updateDataReference) {
        updateDataReference = false;
      }
    }

    [ContextMenu("Generate Guid")]
    public void GenerateUniqueID() {
      //if (ValidationHelper.IsGuid(uniqueID) == false) {
      //  uniqueID = Guid.NewGuid().ToString();
      //}
      uniqueID = Guid.NewGuid().ToString();
    }
    [ContextMenu("Re-Generate Guid")]
    public void ReGenerateUniqueID() {
      uniqueID = Guid.NewGuid().ToString();
    }
#endif
  }
}