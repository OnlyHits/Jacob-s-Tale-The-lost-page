using System.Collections.Generic;
using CustomArchitecture;
using Unity.Cinemachine;
using UnityEngine;

namespace Comic
{
    public class Page : BaseBehaviour
    {
        [SerializeField] private GameObject m_visual;
        [SerializeField] private Transform m_spawnPoint;
        [SerializeField, ReadOnly] private List<Case> m_cases = new List<Case>();

        private Quaternion m_baseVisualRot;

        private void Awake()
        {
            m_baseVisualRot = m_visual.transform.rotation;

            var cases = GetComponentsInChildren<Case>(true);
            m_cases.AddRange(cases);
        }

        #region VISUAL
        public Quaternion GetBaseVisualRot()
        {
            return m_baseVisualRot;
        }
        public void ResetBaseVisualRot()
        {
            m_visual.transform.rotation = m_baseVisualRot;
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


        public Case GetCurrentCase()
        {
            foreach (Case currentCase in m_cases)
            {
                if (currentCase.IsPlayerInCase())
                {
                    return currentCase;
                }
            }
            Debug.Log("Player is in no case from the page " + gameObject.name);
            return null;
        }
    }
}
