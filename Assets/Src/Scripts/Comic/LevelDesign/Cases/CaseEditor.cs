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

            m_canvasEditor?.Refresh();
            m_colliderEditor?.Refresh();
            m_decorEditor?.Refresh();

            //Vector3 childGlobalPosition = transform.position;
            //transform.parent.position = childGlobalPosition;
            //transform.SetParent(transform.parent, true);

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
            m_canvasEditor?.EnableVisual(enable);
        }

    }
}
#endif


public class AlignParentToChild : MonoBehaviour
{
    public Transform child;

    public void AlignParent()
    {
        if (child == null)
        {
            Debug.LogError("Child Transform is not assigned.");
            return;
        }

        // Store the child's current global position
        Vector3 childGlobalPosition = child.position;

        // Set the parent to the child's global position
        transform.position = childGlobalPosition;

        // Reassign the child to the parent
        child.SetParent(transform, true); // 'true' ensures it retains its global position
    }
}