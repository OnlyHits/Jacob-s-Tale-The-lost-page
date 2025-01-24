using CustomArchitecture;
using Unity.Cinemachine;
using UnityEngine;
using Comic;

public class ShakeCam : BaseBehaviour
{
    public static ShakeCam Inst { get; private set; }
    [SerializeField, ReadOnly] private CinemachineCamera m_cam;
    [SerializeField, ReadOnly] private CinemachineBasicMultiChannelPerlin m_multChanPerlin;
    private float m_shakeTimer;

    private void Awake()
    {
        Inst = this;

        m_cam = GetComponentInChildren<CinemachineCamera>();
        m_multChanPerlin = m_cam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
    }

    public void StopShake()
    {
        m_multChanPerlin.AmplitudeGain = 0f;
    }

    public void LoopShake(float intensity, bool state)
    {
        m_multChanPerlin.AmplitudeGain = state ? intensity : 0f;
    }

    public void Shake(float intensity, float time)
    {
        m_multChanPerlin.AmplitudeGain = intensity;
        m_shakeTimer = time;
    }

    protected override void OnUpdate(float delta)
    {
        if (m_shakeTimer > 0)
        {
            m_shakeTimer -= delta;

            if (m_shakeTimer <= 0f)
            {
                m_multChanPerlin.AmplitudeGain = 0f;
            }
        }
    }
}