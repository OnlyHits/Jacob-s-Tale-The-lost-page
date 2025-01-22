#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Comic
{
    public class CaseEditor : MonoBehaviour
    {
        public enum DecorType
        {
            Room,
            Outside
        }

        public CaseCanvasEditor m_canvasEditor;
        public CaseColliderEditor m_colliderEditor;
        public CaseDecorEditor m_decorEditor;

        [Space]
        public DecorType m_decorType;
        public Sprite m_backgroundSprite;
        public Sprite m_groundSprite;
        public Sprite m_ceilingSprite;

        private Vector3 m_lastScale = Vector2.zero;


        [Button("Refresh All")]
        private void RefreshComponents()
        {
            TryRefresh();
        }

        private void OnDrawGizmos()
        {
            TryRefresh();
        }

        private void TryRefresh()
        {
            if (m_lastScale == transform.localScale)
            {
                return;
            }

            m_canvasEditor?.Refresh();
            m_colliderEditor?.Refresh();
            m_decorEditor?.Refresh();

            m_lastScale = transform.localScale;
        }

        private void OnDecordChanged()
        {
            CaseDecorProvider provider = new CaseDecorProvider(m_decorType, m_backgroundSprite, m_groundSprite, m_ceilingSprite);
            m_decorEditor?.Setup(provider);
        }

    }
}
#endif