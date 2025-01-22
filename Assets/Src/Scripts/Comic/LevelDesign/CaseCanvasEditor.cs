#if UNITY_EDITOR
using System.Collections.Generic;
using CustomArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Comic
{
    public class CaseCanvasEditor : MonoBehaviour
    {
        public Transform m_caseSprite;
        public Canvas m_canvas;
        [SerializeField, ReadOnly] private RectTransform m_rectCanvas;

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

        private void OnDrawGizmos()
        {
            Refresh();
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
    }
}
#endif
