using System.Collections.Generic;
using CustomArchitecture;
using Sirenix.Utilities;
using UnityEngine;

namespace Comic
{
    public partial class PageManager : BaseBehaviour
    {
        [SerializeField] private List<Page> m_pageList = new List<Page>();
        [SerializeField, ReadOnly] private Page m_currentPage;
        [SerializeField, ReadOnly] private int m_currentPageIndex;
        [SerializeField, ReadOnly] private List<Page> m_unlockedPageList = new List<Page>();


        private void Awake()
        {

        }

        private void Start()
        {
            foreach (var data in ComicGameCore.Instance.GetGameMode<MainGameMode>().GetUnlockChaptersData())
            {
                UnlockPages(ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig().GetPagesByChapter(data.m_chapterType));
            }

            SwitchPageByIndex(m_currentPageIndex);
        }

        public void Init()
        {
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToUnlockChapter(OnUnlockChapter);
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToLockChapter(OnLockChapter);
        }

        #region ON LOCK & UNLOCK CHAPTERS
        private void OnUnlockChapter(Chapters chapterUnlocked)
        {
            UnlockPages(ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig().GetPagesByChapter(chapterUnlocked));
        }
        private void OnLockChapter(Chapters chapterUnlocked)
        {
            LockPages(ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig().GetPagesByChapter(chapterUnlocked));
        }

        #endregion ON LOCK & UNLOCK CHAPTERS

        #region LOCK & UNLOCK PAGES
        private void LockPages(List<int> pageIndexes)
        {
            if (pageIndexes.IsNullOrEmpty())
            {
                Debug.LogError("Could not get pages indexes because the list is null");
            }
            foreach (int index in pageIndexes)
            {
                if (index >= m_pageList.Count)
                    continue;

                var page = m_pageList[index];
                m_unlockedPageList.Remove(page);

                if (m_currentPageIndex == index)
                {
                    m_currentPageIndex = m_unlockedPageList.Count - 1;
                    SwitchPageByIndex(m_currentPageIndex);
                }
            }
        }

        private void UnlockPages(List<int> pageIndexes)
        {
            if (pageIndexes.IsNullOrEmpty())
            {
                Debug.LogError("Could not get pages indexes because the list is null");
            }
            foreach (int index in pageIndexes)
            {
                if (index >= m_pageList.Count)
                    continue;

                var page = m_pageList[index];
                m_unlockedPageList.Add(page);
            }
        }

        #endregion LOCK & UNLOCK PAGES

        #region TRY NEXT & PREV PAGE
        public bool TryNextPage()
        {
            int nextIdx = m_currentPageIndex + 1;
            if (nextIdx >= m_unlockedPageList.Count)
            {
                return false;
            }
            SwitchPage(true, nextIdx);
            return true;
        }
        public bool TryPrevPage()
        {
            int prevIdx = m_currentPageIndex - 1;
            if (prevIdx < 0)
            {
                return false;
            }

            SwitchPage(false, prevIdx);
            return true;
        }

        #endregion TRY NEXT & PREV PAGE

        public Transform GetSpawnPointByPageIndex(int indexPage)
        {
            if (indexPage >= m_pageList.Count)
            {
                Debug.LogWarning("Try to get page index " + indexPage.ToString() + " which does not exist in PageManager");
                return null;
            }
            Page page = m_pageList[indexPage];

            return page.TryGetSpawnPoint();
        }

        public Page GetCurrentPage()
        {
            return m_currentPage;
        }
        public Case GetCurrentCase()
        {
            return m_currentPage.GetCurrentCase();
        }
    }
}