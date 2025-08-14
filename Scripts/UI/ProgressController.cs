using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.MTP;
using UnityEngine.Events;

namespace Halabang.UI {
  public class ProgressController : MonoBehaviour {
    [SerializeField] private StyleManager[] motionTitles;

    private Coroutine progressTransition;

    private void Start() {
    }

    public void SetProgress(float percentage) {
      if (motionTitles != null && motionTitles.Length == 0) {
        Debug.LogError("Motion titles are not set or empty.");
        return;
      }
      if (progressTransition != null) StopCoroutine(progressTransition);
      progressTransition = StartCoroutine(progressSequence());
    }
    private IEnumerator progressSequence() {
      yield return new WaitForSeconds(1f);
      foreach (StyleManager motionTitle in motionTitles) {
        motionTitle.PlayIn();
        yield return new WaitForSeconds(3f);
        motionTitle.PlayOut();
        yield return new WaitForSeconds(2f);
      }
    }

#if UNITY_EDITOR
    [SerializeField] private float enableProgress;
    private void OnValidate() {
      if (enableProgress > 0f) {
        SetProgress(enableProgress);
        enableProgress = 0;
      }
    }
#endif
  }
}