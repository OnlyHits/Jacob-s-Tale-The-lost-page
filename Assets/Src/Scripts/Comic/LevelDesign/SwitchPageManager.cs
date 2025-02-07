using System;
using CustomArchitecture;
using UnityEngine;

namespace Comic
{
    public partial class PageManager : BaseBehaviour
    {
        [Header("Switch Page")]
        [SerializeField] private float m_durationSwitchPage = 1f;

        private Action<bool, Page, Page> m_onBeforeSwitchPageCallback;
        // remove
        private Action<bool, Page, Page> m_onMiddleSwitchPageCallback;
        private Action<bool, Page, Page> m_onAfterSwitchPageCallback;

        // remove
        private Action<bool> m_onAfterCloneCanvasCallback;

        [Header("Canvas Duplication")]
        [SerializeField, ReadOnly] private GameObject m_canvasDuplicated;

        #region CALLBACKS

        public void SubscribeToAfterCloneCanvasCallback(Action<bool> function)
        {
            m_onAfterCloneCanvasCallback -= function;
            m_onAfterCloneCanvasCallback += function;
        }

        public void SubscribeToBeforeSwitchPage(Action<bool, Page, Page> function)
        {
            m_onBeforeSwitchPageCallback -= function;
            m_onBeforeSwitchPageCallback += function;
        }

        public void SubscribeToMiddleSwitchPage(Action<bool, Page, Page> function)
        {
            m_onMiddleSwitchPageCallback -= function;
            m_onMiddleSwitchPageCallback += function;
        }

        public void SubscribeToAfterSwitchPage(Action<bool, Page, Page> function)
        {
            m_onAfterSwitchPageCallback -= function;
            m_onAfterSwitchPageCallback += function;
        }

        #endregion CALLBACKS


        public float GetSwitchPageDuration()
        {
            return m_durationSwitchPage;
        }

        private void SwitchPage(bool isNextPage, int idxNewPage)
        {
            if (ComicGameCore.Instance.MainGameMode.GetCameraManager().IsCameraRegister(URP_OverlayCameraType.Camera_Hud))
            {
                StartCoroutine(ComicGameCore.Instance.MainGameMode.GetCameraManager().ScreenAndApplyTexture());
            }


            // Page currentPage = m_unlockedPageList[m_currentPageIndex];
            // Page newPage = m_unlockedPageList[idxNewPage];

            // m_onBeforeSwitchPageCallback?.Invoke(isNextPage, currentPage, newPage);

            // SwitchCanvas(isNextPage, idxNewPage);

            // m_onAfterCloneCanvasCallback?.Invoke(isNextPage);

            // float delayEnableCurrentPage = isNextPage ? 0 : 0;
            // float delayDisableCurrentPage = isNextPage ? m_durationSwitchPage : m_durationSwitchPage / 2;
            // float delayEnableNewPage = isNextPage ? m_durationSwitchPage / 2 : 0;
            // float delayDisableNewPage = isNextPage ? m_durationSwitchPage : m_durationSwitchPage;

            // if (!isNextPage) StartCoroutine(CoroutineUtils.InvokeOnDelay(delayEnableCurrentPage, () => currentPage.Enable(true)));
            // StartCoroutine(CoroutineUtils.InvokeOnDelay(delayDisableCurrentPage, () => currentPage.Enable(false)));
            // StartCoroutine(CoroutineUtils.InvokeOnDelay(delayEnableNewPage, () => newPage.Enable(true)));
            // if (isNextPage) StartCoroutine(CoroutineUtils.InvokeOnDelay(delayDisableNewPage, () => newPage.Enable(false)));

            // StartCoroutine(CoroutineUtils.InvokeOnDelay(m_durationSwitchPage / 2, () =>
            // {
            //     m_onMiddleSwitchPageCallback?.Invoke(isNextPage, currentPage, newPage);
            // }));

            // StartCoroutine(CoroutineUtils.InvokeOnDelay(m_durationSwitchPage, () =>
            // {
            //     m_currentPageIndex = idxNewPage;
            //     SwitchPageByIndex(m_currentPageIndex);
            //     DestroyCanvasCopy();
            //     m_onAfterSwitchPageCallback?.Invoke(isNextPage, currentPage, newPage);
            // }));
        }

        private void SwitchCanvas(bool isNextPage, int idxNewPage)
        {
            // m_canvasDuplicated = Instantiate(m_canvas);
            // foreach (var d in m_canvasDuplicated.GetComponentsInChildren<AView>())
            // {
            //     if (d.gameObject.activeSelf)
            //         d.Pause(true);
            // }
            DisableAllMonoBehaviours(m_canvasDuplicated);
        }

        public static void DisableAllMonoBehaviours(GameObject parent)
        {
            if (parent == null)
            {
                Debug.LogWarning("Parent GameObject is null. Cannot disable MonoBehaviours.");
                return;
            }

            BaseBehaviour[] monoBehaviours = parent.GetComponents<BaseBehaviour>();
            foreach (BaseBehaviour mb in monoBehaviours)
            {
                mb.enabled = false;
            }

            foreach (Transform child in parent.transform)
            {
                DisableAllMonoBehaviours(child.gameObject);
            }
        }

        // private void DestroyCanvasCopy()
        // {
        //     if (m_canvasDuplicated != null)
        //     {
        //         Destroy(m_canvasDuplicated);
        //     }
        // }

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
