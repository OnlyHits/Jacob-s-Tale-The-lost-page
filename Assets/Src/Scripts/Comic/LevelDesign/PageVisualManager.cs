using CustomArchitecture;

namespace Comic
{
    public class PageVisualManager : BaseBehaviour
    {
        private void Awake()
        {
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
                currentPage.GetVisualTransform().
                //currentPage.transform.position
            }
        }

    }
}
