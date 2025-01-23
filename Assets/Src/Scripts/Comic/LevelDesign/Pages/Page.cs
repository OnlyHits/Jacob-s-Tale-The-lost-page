using CustomArchitecture;
using Unity.Cinemachine;
using UnityEngine;

namespace Comic
{
    public class Page : BaseBehaviour
    {
        [SerializeField] private GameObject m_visual;
        [SerializeField] private Transform m_spawnPoint;
        public void Enable(bool enable)
        {
            m_visual.SetActive(enable);
        }

        public Transform TryGetSpawnPoint()
        {
            return m_spawnPoint;
        }

        public Transform GetVisualTransform()
        {
            return m_visual.transform;
        }
    }
}
