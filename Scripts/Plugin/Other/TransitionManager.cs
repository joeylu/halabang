using TransitionScreenPackage;
using UnityEngine;

namespace Halabang.Plugin {
  public class TransitionManager : MonoBehaviour {
    public enum TransitionType { Normal, Outline }
    public TransitionScreenManager CurrentTransitionScreen { get; private set; }
    public bool IsTransitioning { get; private set; }  //当前正在转入，或者转出中
    public bool IsRevealed { get; private set; } //当前转场是否已经转入

    [SerializeField] private TransitionScreenManager[] normalTransitions;
    [SerializeField] private TransitionScreenManager[] outlineTransitions;

    public void ToggleTransition(int index, TransitionType transitionType) {
      if (IsRevealed) {
        StopTransition();
      } else {
        StartTransition(index, transitionType);
      }
    }

    public void StartTransition(int index, TransitionType transitionType) {
      TransitionScreenManager prefab = getPrefab(index, transitionType);
      if (prefab == null) return;

      if (CurrentTransitionScreen?.name == prefab.name) {
        CurrentTransitionScreen.Reveal();
      } else {
        if (CurrentTransitionScreen != null) {
          CurrentTransitionScreen.FinishedHideEvent -= offTransitioning;
          CurrentTransitionScreen.FinishedRevealEvent -= offTransitioning;
          Destroy(CurrentTransitionScreen.gameObject);
        }

        // Instantiate new transition screen prefab
        CurrentTransitionScreen = Instantiate(prefab, transform);
        CurrentTransitionScreen.name = prefab.name;
        CurrentTransitionScreen.Reveal();
        IsTransitioning = true;

        CurrentTransitionScreen.FinishedHideEvent += offTransitioning;
        CurrentTransitionScreen.FinishedRevealEvent += offTransitioning;
      }

      IsRevealed = true;
    }

    public void StopTransition() {
      if (CurrentTransitionScreen == null) return;
      CurrentTransitionScreen.Hide();
      IsTransitioning = true;
      IsRevealed = false;
    }

    private void offTransitioning() {
      IsTransitioning = false;
    }
    private TransitionScreenManager getPrefab(int index, TransitionType type) {
      if (type == TransitionType.Normal) {
        if (index < normalTransitions.Length) {
          return normalTransitions[index];
        }
      } else {
        if (index < outlineTransitions.Length) {
          return outlineTransitions[index];
        }
      }

      return null;
    }
  }
}