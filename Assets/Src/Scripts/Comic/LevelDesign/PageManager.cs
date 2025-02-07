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
        [SerializeField] private PageVisualManager m_pageVisual;
        [SerializeField] private float m_durationStartGame = 5f;
        [SerializeField] private float m_durationEndGame = 10f;

        public Page GetCurrentPage() => m_currentPage;
        public Panel GetCurrentPanel() => m_currentPage.GetCurrentPanel();

        public void Init()
        {
            ComicGameCore.Instance.MainGameMode.SubscribeToUnlockChapter(OnUnlockChapter);
            ComicGameCore.Instance.MainGameMode.SubscribeToLockChapter(OnLockChapter);
            ComicGameCore.Instance.MainGameMode.SubscribeToEndGame(OnEndGame);
            
            foreach (var data in ComicGameCore.Instance.MainGameMode.GetUnlockChaptersData())
            {
                UnlockPages(ComicGameCore.Instance.MainGameMode.GetGameConfig().GetPagesByChapter(data.m_chapterType));
            }

            SwitchPageByIndex(m_currentPageIndex);

            // int i = 1;
            // foreach (var page in m_pageList)
            // {
            //     page.GetText().text = i.ToString() + "/" + m_pageList.Count.ToString(); 
            //     ++i;
            // }

            m_pageVisual.Init();

// #if UNITY_EDITOR
// #else
//             OnStartGame();
// #endif
        }

        #region START & END GAME

        // not the right place to do that, put that in maingamecore
        private void OnStartGame()
        {
            ComicGameCore.Instance.MainGameMode.GetCharacterManager().GetPlayer().Pause(true);
            ComicGameCore.Instance.MainGameMode.GetViewManager().Pause(true);

            foreach (var page in m_pageList) page.gameObject.SetActive(false);
            m_pageVisual.m_coverPage.SetActive(true);
            m_pageVisual.m_bgBookVisual.SetActive(false);
            m_pageVisual.m_endPage.SetActive(false);

            StartCoroutine(CoroutineUtils.InvokeOnDelay(m_durationStartGame, () =>
            {
                ComicGameCore.Instance.MainGameMode.GetCharacterManager().GetPlayer().Pause(false);
                ComicGameCore.Instance.MainGameMode.GetViewManager().Pause(false);

                foreach (var page in m_pageList) page.gameObject.SetActive(true);
                m_pageVisual.m_coverPage.SetActive(false);
                m_pageVisual.m_bgBookVisual.SetActive(true);
                m_pageVisual.m_endPage.SetActive(false);
            }));

        }

        private void OnEndGame()
        {
            foreach (var page in m_pageList) page.gameObject.SetActive(false);
            m_pageVisual.m_bgBookVisual.SetActive(true);
            m_pageVisual.m_endPage.SetActive(true);
            m_pageVisual.m_coverPage.SetActive(false);

            // StartCoroutine(CoroutineUtils.InvokeOnDelay(m_durationEndGame, () =>
            // {
            //     ComicGameCore.Instance.MainGameMode.GetCharacterManager().GetPlayer().Pause(false);
            //     ComicGameCore.Instance.MainGameMode.GetViewManager().Pause(false);

            //     foreach (var page in m_pageList) page.gameObject.SetActive(true);
            //     m_pageVisual.m_bgBookVisual.SetActive(true);
            //     m_pageVisual.m_endPage.SetActive(false);
            //     m_pageVisual.m_coverPage.SetActive(false);
            // }));
        }

        #endregion START & END GAME

        #region ON LOCK & UNLOCK CHAPTERS
        private void OnUnlockChapter(Chapters chapterUnlocked)
        {
            UnlockPages(ComicGameCore.Instance.MainGameMode.GetGameConfig().GetPagesByChapter(chapterUnlocked));
        }

        private void OnLockChapter(Chapters chapterUnlocked)
        {
            LockPages(ComicGameCore.Instance.MainGameMode.GetGameConfig().GetPagesByChapter(chapterUnlocked));
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
                return;
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
    }
}