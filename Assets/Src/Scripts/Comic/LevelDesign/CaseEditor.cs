#if UNITY_EDITOR
using System.Collections.Generic;
using CustomArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Comic
{
    public class CaseEditor : MonoBehaviour
    {
        public CaseCanvasEditor m_canvasEditor;
        public CaseColliderEditor m_colliderEditor;

        [Button("Refresh All")]
        private void RefreshComponents()
        {
            m_canvasEditor?.Refresh();
            m_colliderEditor?.Refresh();
        }
    }
}
#endif