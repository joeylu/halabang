using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.MUIP;

namespace Halabang.Plugin {
  public class HorizontalSelectorExt : MonoBehaviour {
    public HorizontalSelector TargetSelector { get { getSelector(); return targetSelector; } }

    private HorizontalSelector targetSelector;

    private void getSelector() {
      if (TargetSelector != null) return;

      targetSelector = GetComponent<HorizontalSelector>();
    }
  }
}
