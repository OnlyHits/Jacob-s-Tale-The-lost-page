using UnityEngine;
using Unity.Cinemachine;
using CustomArchitecture;
using DG.Tweening;

namespace Comic
{
    public class CanvasVisualManager : BaseBehaviour
    {
        [Header("Switch Page Anim")]
        [SerializeField, ReadOnly] private Vector3 m_baseRot;
        [SerializeField] private Transform m_destTransform;
        [SerializeField, ReadOnly] private Vector3 m_destRot;
        [SerializeField] private float m_duration = 1f;
        private Tween m_switchPageTween = null;

        private void Awake()
        {
            m_baseRot = transform.position;
            m_destRot = m_destTransform.eulerAngles;
            PageManager.onSwitchPage += OnSwitchPage;
        }

        private void OnSwitchPage(bool nextPage, Page p1, Page p2)
        {
            if (!nextPage)
            {
                return;
            }

            if (m_switchPageTween != null)
            {
                m_switchPageTween.Kill();
            }

            //m_switchPageTween = transform.DORotate(m_destRot, m_duration)
            float currentValue = m_baseRot.y;
            float startValue = m_baseRot.y;
            float destValue = m_destRot.y;

            m_switchPageTween = DOTween.To(() => startValue, x => currentValue = x, destValue, m_duration / 2)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    transform.eulerAngles = new Vector3(0, currentValue, 0);
                })
                .OnComplete(() =>
                {
                    ResetTransformToBase();
                });
        }

        private void ResetTransformToBase()
        {
            transform.eulerAngles = new Vector3(0, m_baseRot.y, 0);
        }
    }
}
