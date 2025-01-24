using System.Collections.Generic;
using CustomArchitecture;
using DG.Tweening;
using UnityEngine;

namespace Comic
{
    public class PageVisualManager : BaseBehaviour
    {
        [Header("Switch Page Anim")]
        [SerializeField] private Transform m_destTransform;
        [SerializeField, ReadOnly] private Quaternion m_destRotQuat;
        [SerializeField, ReadOnly] private float m_duration = 1f;
        private Tween m_switchPageTween = null;

        [Header("Page Visuals")]
        [SerializeField, ReadOnly] private List<PageVisual> m_pageVisuals = new List<PageVisual>();

        private void Awake()
        {
            m_destRotQuat = m_destTransform.rotation;

            var pages = GetComponentsInChildren<PageVisual>();
            m_pageVisuals.AddRange(pages);
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToBeforeSwitchPage(OnBeforeSwitchPage);
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToAfterSwitchPage(OnAfterSwitchPage);
            m_duration = ComicGameCore.Instance.GetGameMode<MainGameMode>().GetPageManager().GetSwitchPageDuration();
        }


        #region SWITCH PAGE

        private void OnBeforeSwitchPage(bool nextPage, Page currentPage, Page newPage)
        {
            if (nextPage)
            {
                Quaternion from = m_destRotQuat;
                Quaternion to = currentPage.GetBaseVisualRot();
                TranslatePage(from, to, newPage);
                newPage.gameObject.GetComponent<PageVisual>().PushFront();
            }
            else if (nextPage == false)
            {
                Quaternion from = currentPage.GetBaseVisualRot();
                Quaternion to = m_destRotQuat;
                TranslatePage(from, to, currentPage);
                currentPage.gameObject.GetComponent<PageVisual>().PushFront();
            }
        }

        private void OnAfterSwitchPage(bool nextPage, Page currentPage, Page newPage)
        {
            if (nextPage)
            {
                newPage.gameObject.GetComponent<PageVisual>().ResetDefault();
            }
            else if (nextPage == false)
            {
                currentPage.gameObject.GetComponent<PageVisual>().ResetDefault();
            }
        }

        private void TranslatePage(Quaternion from, Quaternion to, Page page)
        {
            if (m_switchPageTween != null)
            {
                m_switchPageTween.Kill();
            }

            if (Quaternion.Dot(from, to) < 0)
            {
                to = new Quaternion(-to.x, -to.y, -to.z, -to.w);
            }

            m_switchPageTween = page.GetVisualTransform().DORotateQuaternion(to, m_duration)
                .From(from)
                .SetEase(Ease.Linear)
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
