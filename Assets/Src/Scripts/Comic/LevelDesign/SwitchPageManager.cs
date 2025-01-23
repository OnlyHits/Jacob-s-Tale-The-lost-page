using System;
using System.Collections.Generic;
using CustomArchitecture;
using Sirenix.Utilities;
using UnityEngine;

namespace Comic
{
    public partial class PageManager : BaseBehaviour
    {
        [Header("Switch Page")]
        [SerializeField] private float m_durationSwitchPage = 1f;

        private Action<bool, Page, Page> m_onBeforeSwitchPageCallback;
        private Action<bool, Page, Page> m_onAfterSwitchPageCallback;


        #region CALLBACKS

        public void SubscribeToBeforeSwitchPage(Action<bool, Page, Page> function)
        {
            m_onBeforeSwitchPageCallback -= function;
            m_onBeforeSwitchPageCallback += function;
        }

        public void SubscribeToAfterSwitchPage(Action<bool, Page, Page> function)
        {
            m_onAfterSwitchPageCallback -= function;
            m_onAfterSwitchPageCallback += function;
        }

        #endregion CALLBACKS


        private void SwitchPage(bool isNextPage, int idxNewPage)
        {
            Page currentPage = m_unlockedPageList[m_currentPageIndex];
            Page newPage = m_unlockedPageList[idxNewPage];
            // Dans les premiere 0.5secondes si (!isNextPage), dans les dernieres 0.5secondes si (isNextPage) 
            float delayEnableNewPage = isNextPage ? m_durationSwitchPage / 2 : 0;

            m_onBeforeSwitchPageCallback?.Invoke(isNextPage, currentPage, newPage);

            currentPage.Enable(true);
            StartCoroutine(CoroutineUtils.InvokeOnDelay(delayEnableNewPage, () =>
            {
                newPage.Enable(true);
            }));

            StartCoroutine(CoroutineUtils.InvokeOnDelay(m_durationSwitchPage, () =>
            {
                m_currentPageIndex = idxNewPage;
                SwitchPageByIndex(m_currentPageIndex);
                m_onAfterSwitchPageCallback?.Invoke(isNextPage, currentPage, newPage);
            }));
        }

        private void SwitchPageByIndex(int index)
        {
            foreach (var page in m_pageList)
            {
                page.Enable(false);
            }

            m_currentPage = m_unlockedPageList[m_currentPageIndex];
            m_currentPage.Enable(true);
        }
    }
}
