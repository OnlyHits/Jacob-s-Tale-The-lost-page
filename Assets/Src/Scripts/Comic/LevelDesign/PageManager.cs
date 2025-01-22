using System.Collections.Generic;
using CustomArchitecture;
using Unity.Cinemachine;
using UnityEngine;

namespace Comic
{
    public class PageManager : BaseBehaviour
    {
        [SerializeField] private List<Page> m_pageList = new List<Page>();
        [SerializeField, ReadOnly] private Page m_currentPage;
        [SerializeField, ReadOnly] private int m_currentPageIndex;

        private void Awake()
        {
            SwitchPageByIndex(m_currentPageIndex);
        }

        public bool TryNextPage()
        {
            int nextIdx = m_currentPageIndex + 1;
            if (nextIdx >= m_pageList.Count)
            {
                return false;
            }
            m_currentPageIndex = nextIdx;
            SwitchPageByIndex(m_currentPageIndex);
            return true;
        }
        public bool TryPrevPage()
        {
            int prevIdx = m_currentPageIndex - 1;
            if (prevIdx < 0)
            {
                return false;
            }
            m_currentPageIndex = prevIdx;
            SwitchPageByIndex(m_currentPageIndex);
            return true;
        }

        private void SwitchPageByIndex(int index)
        {
            foreach (var page in m_pageList)
            {
                page.Enable(false);
            }

            m_currentPage = m_pageList[m_currentPageIndex];
            m_currentPage.Enable(true);
        }
    }
}