using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using Halabang.Plugin;

namespace Halabang.Scene {
  public class CameraManager : MonoBehaviour {
    [SerializeField] private CinemachineCamera defaultCinemachine;

    private CinemachineCamera currentCinemachine;
    private Tweener shakeOnceTweener;
    private Tweener shakeLoopTweener;

    private void Start() {
      resetCurrentCamera();
    }


    public void ActivateCinemachine(CinemachineCamera targetRig) {
      if (currentCinemachine == targetRig) return;
      ShakeLoopStop();
      ShakeOnceStop();
      currentCinemachine.Priority = 0;
      currentCinemachine = targetRig;
      targetRig.Priority = 10;
    }
    public void CameraFadeBlack() {

    }
    public void CameraFadeWhite() {

    }
    public void CameraShake(CameraShakePreset settings) {
      if (currentCinemachine == null) return;

      ShakeLoopPause(true);

      Tweener targetTweener = null;
      CinemachinePositionComposer positionComposer = currentCinemachine.GetComponent<CinemachinePositionComposer>();
      if (positionComposer) {
        if (settings.StrengthVector == Vector3.zero) {
          targetTweener = DOTween.Shake(
            () => positionComposer.TargetOffset, value => positionComposer.TargetOffset = value,
            settings.Duration, settings.StrengthFloat, settings.Vibrato, settings.Randomness, !settings.IncludeZAxis, !settings.ManuallyFadeout, settings.randomnessMode);
        } else {
          targetTweener = DOTween.Shake(
            () => positionComposer.TargetOffset, value => positionComposer.TargetOffset = value,
            settings.Duration, settings.StrengthVector, settings.Vibrato, settings.Randomness, !settings.ManuallyFadeout, settings.randomnessMode);
        }
      }

      targetTweener
        //.OnStart(() => { Debug.Log(Camera.main.name + " start shaking"); })
        //.OnUpdate(() => { Debug.Log(shakeValue); })
        .SetDelay(settings.Delay)
        .SetLoops(settings.TweenLoopCycle, settings.TweenLoopType)
        .SetEase(settings.EaseType)
        .OnComplete(() => { if (settings.TweenLoopCycle >= 0) ShakeLoopPause(false); }); //resume loop shake if any

      if (settings.TweenLoopCycle >= 0) {
        shakeOnceTweener = targetTweener;
      } else {
        shakeLoopTweener = targetTweener;
      }
    }
    public void ShakeLoopStop() {
      if (shakeLoopTweener.IsActiveAndPlaying()) {
        shakeLoopTweener.Rewind(); //use rewind to make sure the previous shake does not alter the camera offset value
        shakeLoopTweener.Kill(); //complete does not effect on infinity loops, use kill
      }
    }
    public void ShakeOnceStop() {
      if (shakeOnceTweener.IsActiveAndPlaying()) {
        shakeOnceTweener.Rewind(); //use rewind to make sure the previous shake does not alter the camera offset value
        shakeOnceTweener.Kill(); //complete does not effect on infinity loops, use kill
      }
      ShakeLoopPause(false);
    }
    public void ShakeLoopPause(bool pause) {
      if (shakeLoopTweener.IsActiveAndPlaying()) {
        if (pause) {
          shakeLoopTweener.Pause();
        } else {
          shakeLoopTweener.Play();
        }
      }
    }

    private void resetCurrentCamera() {
      if (defaultCinemachine == null) return;
      currentCinemachine = defaultCinemachine;
      currentCinemachine.Priority = 10;
    }
  }
}
