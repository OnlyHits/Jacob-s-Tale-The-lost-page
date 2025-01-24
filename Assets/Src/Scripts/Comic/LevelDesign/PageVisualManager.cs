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
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToBeforeSwitchPage(OnBeforeSwitchPage);
            //ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToAfterSwitchPage(OnAfterSwitchPage);
        }


        #region SWITCH PAGE

        private void OnBeforeSwitchPage(bool nextPage, Page currentPage, Page newPage)
        {
            if (nextPage)
            {
                float from = m_destRot.y;
                float to = currentPage.GetBaseVisualRot().y;
                TranslatePage(from, to, newPage);
            }
            else if (nextPage == false)
            {
                float from = currentPage.GetBaseVisualRot().y;
                float to = m_destRot.y;
                TranslatePage(from, to, currentPage);
            }
        }

        private void TranslatePage(float from, float to, Page page)
        {
            if (m_switchPageTween != null)
            {
                m_switchPageTween.Kill();
            }

            float currentValue = from;
            float startValue = from;
            float destValue = to;

            m_switchPageTween = DOTween.To(() => startValue, x => currentValue = x, destValue, m_duration)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    page.GetVisualTransform().eulerAngles = new Vector3(0, currentValue, 0);
                })
                .OnComplete(() =>
                {
                    ResetTransformToBase(page);
                });
        }

        private void ResetTransformToBase(Page page)
        {
            page.ResetBaseVisualRot();
        }

        #endregion SWITCH PAGE

    }

}
