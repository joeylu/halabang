using UnityEngine;
using Halabang.Utilities;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Halabang.Animation {
  public class AnimLayer {
    public enum CHARACTER_LAYER {
      [Description("Base Layer")] BASE = 0,
      [Description("Mood")] MOOD = 1, //情绪Layer是权重始终为1的Additive层
      [Description("Health")] HEALTH = 2,
      [Description("Stamina")] STAMINA = 3
    }

    public int LayerIndex { get; set; }
    public string LayerName { get; set; }
    public float Weight { get; set; }

    public static bool HasCharacterLayer(IEnumerable<AnimLayer> layers) {
      if (layers == null) return false;
      return layers.Any(r =>
        r.LayerName.Equals(CHARACTER_LAYER.MOOD.Description()) ||
        r.LayerName.Equals(CHARACTER_LAYER.HEALTH.Description()) ||
        r.LayerName.Equals(CHARACTER_LAYER.STAMINA.Description())
      );
    }
  }
  /// <summary>
  ///additional animation to be played  while in a looping animation
  ///special anim cannot be looped
  /// </summary>
  /// <typeparam name="T"></typeparam>
  [Serializable]
  public class AnimSequence<T> {
    public T State; //name is a string, comes from any type of Action/State enum, using T here to allow unity inspector to display it as a dropdown dynamically instead input a text string
    [Tooltip("additional param to trigger the animator, only accept 1 param")]
    public string SequenceParam;
    [Tooltip("if it's in loop, play this after a period of time")]
    public float Delay;
    [Tooltip("percentage in 0-100, chance to be played")]
    public int Chance;

    /// <summary>
    /// Generate a known type to generic type
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static AnimSequence<object> FormGeneric(AnimSequence<T> s) {
      return new AnimSequence<object> {
        State = s.State,
        Delay = s.Delay,
        SequenceParam = s.SequenceParam,
        Chance = s.Chance
      };
    }
  }
  public class Dictionary {
    [Flags]
    public enum PARAM {
      NULL,
      ALTER_A,
      ALTER_B,
      ALTER_C,
    }
  }
}
