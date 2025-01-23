using CustomArchitecture;
using DG.Tweening;
using UnityEngine;

namespace Comic
{
    public class PageVisualManager : BaseBehaviour
    {
        [Header("Switch Page Anim")]
        [SerializeField] private Transform m_destTransform;
        [SerializeField, ReadOnly] private Vector3 m_destRot;
        [SerializeField] private float m_duration = 1f;
        private Tween m_switchPageTween = null;

        private void Awake()
        {
            m_destRot = m_destTransform.eulerAngles;
            PageManager.onSwitchPage += OnSwitchPage;
        }

        private void OnSwitchPage(bool nextPage, Page currentPage, Page newPage)
        {
            if (nextPage)
            {
                return;
            }

            if (nextPage == false)
            {
                if (m_switchPageTween != null)
                {
                    m_switchPageTween.Kill();
                }

                float currentValue = currentPage.GetBaseVisualRot().y;
                float startValue = currentPage.GetBaseVisualRot().y;
                float destValue = m_destRot.y;

                m_switchPageTween = DOTween.To(() => startValue, x => currentValue = x, destValue, m_duration / 2)
                    .SetEase(Ease.Linear)
                    .OnUpdate(() =>
                    {
                        currentPage.GetVisualTransform().eulerAngles = new Vector3(0, currentValue, 0);
                    })
                    .OnComplete(() =>
                    {
                        ResetTransformToBase(currentPage);
                    });
            }
        }

        private void ResetTransformToBase(Page page)
        {
            page.ResetBaseVisualRot();
        }
    }

}
