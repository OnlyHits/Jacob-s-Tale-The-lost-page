using UnityEngine;
using Unity.Cinemachine;
using CustomArchitecture;
using DG.Tweening;

namespace Comic
{
    public class CameraManager : BaseBehaviour
    {
        [Header("Cameras")]
        [SerializeField] protected Camera m_cam;
        [SerializeField] protected CinemachineCamera m_camController;

        [Header("Targets")]
        [SerializeField] private Transform m_target;

        [SerializeField, ReadOnly] private CinemachinePositionComposer m_composer;

        [Header("Switch Page Animation")]
        [SerializeField] private float m_durationSwitchPage = 0.25f;
        [SerializeField] private float m_switchPageOrthoSize = 10.5f;
        [SerializeField, ReadOnly] private float m_baseOrthagraphicSize = 9.5f;
        private Tween m_switchPageTween = null;


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

            m_baseOrthagraphicSize = m_camController.Lens.OrthographicSize;
            PageManager.onSwitchPage += OnSwitchPage;
        }

        private void OnSwitchPage(bool nextPage, Page p1, Page p2)
        {
            if (m_switchPageTween != null)
            {
                m_switchPageTween.Kill();
            }

            float currentValue = m_camController.Lens.OrthographicSize;
            float startValue = m_baseOrthagraphicSize;
            float duration = m_durationSwitchPage / 2;
            float destValue = m_switchPageOrthoSize;

            m_switchPageTween = DOTween.To(() => startValue, x => currentValue = x, destValue, duration)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InQuad)
                .OnUpdate(() =>
                {
                    m_camController.Lens.OrthographicSize = currentValue;
                })
                .OnComplete(() =>
                {
                    //Debug.Log("> Complete");
                })
                .OnKill(() =>
                {
                    //Debug.Log("> Killed");
                });
        }
    }
}