using CustomArchitecture;
using Unity.Cinemachine;
using UnityEngine;

namespace Comic
{
    public class Page : BaseBehaviour
    {
        [SerializeField] private GameObject m_visual;
        [SerializeField] private Transform m_spawnPoint;

        [SerializeField, ReadOnly] private Vector3 m_baseVisualRot;

        private void Awake()
        {
            m_baseVisualRot = m_visual.transform.eulerAngles;
        }

        #region VISUAL
        public Vector3 GetBaseVisualRot()
        {
            return m_baseVisualRot;
        }
        public void ResetBaseVisualRot()
        {
            m_visual.transform.eulerAngles = new Vector3(0, m_baseVisualRot.y, 0);
        }
        public Transform GetVisualTransform()
        {
            return m_visual.transform;
        }

        public void Enable(bool enable)
        {
            m_visual.SetActive(enable);
        }
        #endregion VISUAL

        #region SPAWN POINT
        public Transform TryGetSpawnPoint()
        {
            return m_spawnPoint;
        }
        #endregion SPAWN POINT
    }
}
