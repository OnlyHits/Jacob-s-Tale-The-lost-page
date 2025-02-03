using UnityEngine;
using Unity.Cinemachine;
using CustomArchitecture;
using DG.Tweening;
using System;

namespace Comic
{
    public class CameraManager : BaseBehaviour
    {
        [Header("Cameras")]
        [SerializeField] protected Camera m_cam;
        [SerializeField] protected Camera m_camOverlay;
        [SerializeField] protected CinemachineCamera m_camController;

        [Header("Targets")]
        [SerializeField] private Transform m_target;

        [SerializeField, ReadOnly] private CinemachinePositionComposer m_composer;

        [Header("Switch Page Animation")]
        [SerializeField] private float m_switchPageMainOrthoSize = 10.5f;
        [SerializeField, ReadOnly] private float m_baseMainOrthoSize = 9.5f;

        [Space]
        [SerializeField] private float m_switchPageOverlayOrthoSize = 60f;
        [SerializeField, ReadOnly] private float m_baseOverlayOrthoSize = 57.5f;

        [Space]
        [SerializeField, ReadOnly] private float m_durationSwitchPage = 0.25f;
        private Tween m_switchPageMainTween = null;
        private Tween m_switchPageOverlayTween = null;


        #region Shake
        public void StopCamShake() => ShakeCam.Inst?.StopShake();
        public void CamShakeMacro(float duration = 0.375f) => ShakeCam.Inst?.Shake(0.375f, duration);
        public void CamShakeMini(float duration = 0.5f) => ShakeCam.Inst?.Shake(0.5f, duration);
        public void CamShakeLight(float duration = 0.5f) => ShakeCam.Inst?.Shake(1f, duration);
        public void CamShakeHeavy(float duration = 0.5f) => ShakeCam.Inst?.Shake(2f, duration);

        public void CamShakeMacroLoop(bool state) => ShakeCam.Inst?.LoopShake(0.375f, state);
        public void CamShakeMiniLoop(bool state) => ShakeCam.Inst?.LoopShake(0.5f, state);
        public void CamShakeLightLoop(bool state) => ShakeCam.Inst?.LoopShake(1f, state);
        public void CamShakeHeavyLoop(bool state) => ShakeCam.Inst?.LoopShake(1.5f, state);
        #endregion

        private void Awake()
        {
            m_composer = m_camController.GetComponent<CinemachinePositionComposer>();
            m_camController.Follow = m_target;
            m_camController.LookAt = m_target;
            m_baseMainOrthoSize = m_camController.Lens.OrthographicSize;
            m_baseOverlayOrthoSize = m_camOverlay.orthographicSize;
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            ComicGameCore.Instance.MainGameMode.SubscribeToBeforeSwitchPage(OnBeforeSwitchPage);
            //ComicGameCore.Instance.MainGameMode.SubscribeToAfterSwitchPage(OnAfterSwitchPage);
            m_durationSwitchPage = ComicGameCore.Instance.MainGameMode.GetPageManager().GetSwitchPageDuration();
        }

        private void OnBeforeSwitchPage(bool nextPage, Page p1, Page p2)
        {
            float duration = m_durationSwitchPage / 2;

            if (m_switchPageMainTween != null)
                m_switchPageMainTween.Kill();

            if (m_switchPageOverlayTween != null)
                m_switchPageOverlayTween.Kill();

            m_switchPageMainTween = PlayZoomTweenOnCam(m_baseMainOrthoSize, m_switchPageMainOrthoSize, duration, SetControllerCamOrthoSize);
            m_switchPageOverlayTween = PlayZoomTweenOnCam(m_baseOverlayOrthoSize, m_switchPageOverlayOrthoSize, duration, SetOveralyCamOrthoSize);
        }

        private Tween PlayZoomTweenOnCam(float from, float to, float duration, Action<float> setterCallback)
        {
            float value = 0f;

            Tween tween = DOTween.To(() => from, x => value = x, to, duration)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    setterCallback?.Invoke(value);
                });

            return tween;
        }

        private void SetControllerCamOrthoSize(float newSize)
        {
            m_camController.Lens.OrthographicSize = newSize;
        }

        private void SetOveralyCamOrthoSize(float newSize)
        {
            m_camOverlay.orthographicSize = newSize;
        }
    }
}