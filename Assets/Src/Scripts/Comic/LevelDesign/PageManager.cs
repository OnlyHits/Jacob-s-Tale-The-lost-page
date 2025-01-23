using System.Collections.Generic;
using CustomArchitecture;
using Sirenix.Utilities;
using UnityEngine;

namespace Comic
{
    public class PageManager : BaseBehaviour
    {
        [SerializeField] private List<Page> m_pageList = new List<Page>();
        [SerializeField, ReadOnly] private Page m_currentPage;
        [SerializeField, ReadOnly] private int m_currentPageIndex;
        [SerializeField, ReadOnly] private List<Page> m_unlockedPageList = new List<Page>();

        private void Awake()
        {
        }

        public void Init()
        {
            foreach (var data in ComicGameCore.Instance.GetGameMode<MainGameMode>().GetSavedValues())
            {
                if (data.m_hasUnlockVoice)
                {
                    UnlockPages(ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig().GetPagesByChapter(data.m_chapterType));
                }
            }

            SwitchPageByIndex(m_currentPageIndex);
        }

        private void UnlockPages(List<int> pageIndexes)
        {
            if (pageIndexes.IsNullOrEmpty())
            {
                Debug.LogError("Could not get pages indexes because the list is null");
            }
            foreach (int index in pageIndexes)
            {
                var page = m_pageList[index];
                m_unlockedPageList.Add(page);
            }
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