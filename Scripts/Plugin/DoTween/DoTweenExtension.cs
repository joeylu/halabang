using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Halabang.Plugin {
  public static class DoTweenExtension {
    /* CAUTION: DO NOT USE tweener.IsComplete() for any kind determinations */
    public static bool IsActiveAndPlaying(this Tweener tweener) {
      if (tweener == null) return false; //tweener is not null
      if (tweener.IsActive() == false) return false; //tweener is not killed
      return (tweener.IsPlaying());
    }
  }
}
