using System;
using DG.Tweening;
using UnityEngine;
using Halabang.Utilities;

namespace Halabang.Plugin {
  [Serializable]
  public class TweenSetting {
    public float DurationValue { get { return (Duration > 0) ? Duration : DurationRange.RandomBetween(); } }  //(No idea why -1 but 0, but too damn afraid to change)"

    public float Delay;
    public float Duration;
    [Tooltip("如果Duration为0，则使用此随机范围值")]
    public Vector2 DurationRange;
    public LoopType LoopType;
    public int LoopCycle;
    public Ease EaseType;
  }
}
