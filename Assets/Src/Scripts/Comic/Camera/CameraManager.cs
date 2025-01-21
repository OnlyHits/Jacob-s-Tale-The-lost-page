using UnityEngine;
using Unity.Cinemachine;
using CustomArchitecture;

namespace Comic
{
    public partial class CameraManager : BaseBehaviour
    {
        [Header("Cameras")]
        [SerializeField] protected Camera m_cam;
        [SerializeField] protected CinemachineCamera m_camController;

        [Header("Targets")]
        [SerializeField] private Transform m_target;

        [SerializeField, ReadOnly] private CinemachinePositionComposer m_composer;


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
        }
    }
}