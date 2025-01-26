//#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;

namespace Comic
{
    public class PageMarginEditor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_leftMargin;
        [SerializeField] private SpriteRenderer m_rightMargin;
        [SerializeField] private SpriteRenderer m_topMargin;
        [SerializeField] private SpriteRenderer m_botMargin;

        public void EnableVisual(bool enable)
        {
            m_leftMargin.enabled = enable;
            m_rightMargin.enabled = enable;
            m_topMargin.enabled = enable;
            m_botMargin.enabled = enable;

        }
    }
}
//#endif
