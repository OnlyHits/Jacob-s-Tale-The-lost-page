#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Comic
{
    public class CaseCanvasEditor : MonoBehaviour
    {
        public Transform m_caseSprite;
        public Canvas m_canvas;
        [SerializeField, ReadOnly] private RectTransform m_rectCanvas;
        [SerializeField] private Image m_leftMargin;
        [SerializeField] private Image m_rightMargin;
        [SerializeField] private Image m_topMargin;
        [SerializeField] private Image m_botMargin;

        private bool TryInit()
        {
            if (m_canvas == null)
            {
                return false;
            }
            m_rectCanvas = m_canvas.GetComponent<RectTransform>();

            if (m_rectCanvas == null)
            {
                return false;
            }
            return true;
        }

        [Button("Force Refresh")]
        public void Refresh()
        {
            if (m_rectCanvas == null)
            {
                bool res = TryInit();
                if (res == false)
                {
                    Debug.LogWarning("Canvas not set on [" + gameObject.name + "]");
                    return;
                }
            }
            UpdateCanvas();
        }

        private void UpdateCanvas()
        {
            m_canvas.transform.position = m_caseSprite.transform.position;

            float w = m_caseSprite.transform.localScale.x;
            float h = m_caseSprite.transform.localScale.y;

            m_rectCanvas.sizeDelta = new Vector2(w, h);
        }

        public void EnableVisual(bool enable)
        {
            m_leftMargin.enabled = enable;
            m_rightMargin.enabled = enable;
            m_topMargin.enabled = enable;
            m_botMargin.enabled = enable;
        }
    }
}
#endif
