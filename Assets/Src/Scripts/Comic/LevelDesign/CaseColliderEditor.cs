#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using CustomArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Comic
{
    public class CaseColliderEditor : MonoBehaviour
    {
        public Transform m_caseSprite;
        public GameObject m_edgeColliderObj;

        [SerializeField, ReadOnly] private Transform m_edgeColliderTransform;
        [SerializeField, ReadOnly] private EdgeCollider2D m_edgeCollider;

        private bool TryInit()
        {
            if (m_edgeColliderObj == null)
            {
                return false;
            }
            m_edgeColliderTransform = m_edgeColliderObj.transform;
            m_edgeCollider = GetComponent<EdgeCollider2D>();

            if (m_edgeColliderTransform == null || m_edgeCollider == null)
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
            if (m_edgeColliderTransform == null || m_edgeCollider == null)
            {
                bool res = TryInit();
                if (res == false)
                {
                    Debug.LogWarning("EdgeCollider not set on [" + gameObject.name + "]");
                    return;
                }
            }

            UpdateEdges();
        }

        private void UpdateEdges()
        {

        }
    }
}
#endif