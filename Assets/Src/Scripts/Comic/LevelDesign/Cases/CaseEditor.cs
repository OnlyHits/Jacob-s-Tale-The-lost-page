//#if UNITY_EDITOR
using System.Collections.Generic;
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

        // replaced by guizmos
        // public CaseCanvasEditor m_canvasEditor;
        public CaseColliderEditor m_colliderEditor;
        public CaseDecorEditor m_decorEditor;

        [Space]
        public Transform m_propsParent;
        private List<Prop> m_props;


        #region DECOR FIELDS
        [Space]
        [OnValueChanged("OnDecorFieldChanged")]
        public DecorType m_decorType;

        [OnValueChanged("OnDecorFieldChanged")]
        [ShowIf("IsRoomOrOutside")]
        public Sprite m_backgroundSprite;

        [OnValueChanged("OnDecorFieldChanged")]
        [ShowIf("IsRoomOrOutside")]
        public Sprite m_groundSprite;

        [OnValueChanged("OnDecorFieldChanged")]
        [ShowIf("IsRoom")]
        public Sprite m_ceilingSprite;
        #endregion DECOR FIELDS

        private Vector3 m_lastPosition = Vector2.zero;
        private Vector3 m_lastScale = Vector2.zero;
        private Vector3 m_lastRotation = Vector2.zero;

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
            if (m_lastScale == transform.localScale && m_lastPosition == transform.position && m_lastRotation == transform.eulerAngles)
            {
                return;
            }

            // m_canvasEditor?.Refresh();
            m_colliderEditor?.Refresh();
            m_decorEditor?.Refresh();

            m_propsParent.position = transform.position;

            m_lastScale = transform.localScale;
            m_lastPosition = transform.position;
            m_lastRotation = transform.eulerAngles;
        }

        private void OnDecorFieldChanged()
        {
            if (m_decorType == DecorType.Outside)
            {
                m_ceilingSprite = null;
            }

            CaseDecorProvider provider = new CaseDecorProvider(m_decorType, m_backgroundSprite, m_groundSprite, m_ceilingSprite);
            m_decorEditor?.Setup(provider);
        }

        private bool IsRoom() => m_decorType == DecorType.Room;
        private bool IsRoomOrOutside() => m_decorType == DecorType.Room || m_decorType == DecorType.Outside;

        public void EnableCanvasVisual(bool enable)
        {
            // m_canvasEditor?.EnableVisual(enable);
        }

    }
}
//#endif
