using Halabang.Utilities;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using static Halabang.Constants;

namespace Halabang.Animation {
  public class AnimationHelper {
    public static string[] GetActiveBoolParam(Animator ac) {
      if (ac && ac.parameters.Length > 0) {
        return ac.parameters.Where(r => ac.GetBool(r.name) == true).Select(s => s.name).ToArray();
      }
      return null;
    }
    public static bool CheckBoolParamsExist(Animator ac, string[] parameters) {
      if (!ac || ac.parameters.Length == 0) {
        Debug.LogError("CheckBoolParamsExist has no animator or parameters given: " + ac.gameObject.activeInHierarchy + " >> " + ac?.runtimeAnimatorController + " : " + ac?.transform?.parent + " > " + ac.parameterCount + " / " + ac.parameters.Length);
        return false;
      }
      if (ac && ac.parameters.Length > 0) {
        foreach (string param in parameters) {
          if (string.IsNullOrWhiteSpace(param)) continue;
          if (string.Equals(param, NULL, StringComparison.OrdinalIgnoreCase)) continue;
          if (!ac.parameters.Any(a => a.name == param)) {
            //Debug.LogError(param + " param is not existed in current animator " + ac.transform.parent?.name ?? "" + " / " + ac.name);
            return false;
          }
        }
      }
      return true;
    }
    public static bool CheckBoolParamExist(Animator ac, string param) {
      if (!ac || ac.parameters.Length == 0 || string.IsNullOrWhiteSpace(param)) return false;
      if (ac && ac.parameters.Length > 0) {
        return ac.parameters.Any(a => a.name == param);
      }
      return false;
    }
    public static bool CheckLayersExist(Animator ac, IEnumerable<AnimLayer> layers) {
      if (!ac) {
        Debug.LogError("CheckLayersExist has no animator : " + ac + " : " + ac.transform.parent);
        return false;
      }
      if (ac && ac.layerCount > 1) {
        foreach (AnimLayer layer in layers) {
          if (CheckLayerExist(ac, layer.LayerName) == false) return false;
        }
      }
      return true;
    }
    public static bool CheckLayerExist(Animator ac, string layername) {
      if (layername.Equals(AnimLayer.CHARACTER_LAYER.BASE.Description(), StringComparison.OrdinalIgnoreCase)) return true; //always true if target layer is the base layer
      return ac.GetLayerIndex(layername) > 0;
    }
    public static AnimSequence<object> GetRandomSequence(List<AnimSequence<object>> seqs) {
      if (seqs == null) throw new ArgumentException("AnimationHelper.GetRandomSequence() Parameter cannot be null");
      //return first found if any of the collection has a chance equals to 1
      if (seqs.Any(a => a.Chance >= 1)) return seqs.Where(r => r.Chance >= 1).First();

      var rnd = new System.Random();
      //get a default seq in case nothing returns from random pick
      var defaultSeq = seqs[rnd.Next(seqs.Count)];
      //for each collection, filter disqualified seqs then get a final list
      foreach (var seq in seqs)
        if (!GenericUtilities.RandomChance(seq.Chance)) seqs.Remove(seq);

      if (seqs.Count == 0) return defaultSeq;
      //return random one if more than one remains
      if (seqs.Count > 1) return seqs[rnd.Next(seqs.Count)];
      //return the qualify one if only one remains
      return seqs.First();
    }
  }
}
