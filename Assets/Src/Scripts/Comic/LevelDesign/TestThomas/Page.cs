using UnityEngine;
using CustomArchitecture;
using System.Collections.Generic;

namespace Comic
{
    [ExecuteAlways]
    public class Page : BaseBehaviour
    {
        [SerializeField] private Transform m_panelContainer;
        [SerializeField] private SpriteRenderer m_margin;
        [SerializeField] private GameObject m_panelPrefab;
        [ReadOnly, SerializeField] private List<Panel> m_currentPanels;
        [SerializeField] private Transform m_spawnPoint;

        private void Awake()
        {
            if (m_margin != null)
            {
                foreach (var panel in m_currentPanels)
                {
                    panel.Init(m_margin);
                }
            }
        }

        #region SPAWN POINT

        public Transform TryGetSpawnPoint()
        {
            return m_spawnPoint;
        }

        #endregion SPAWN POINT

        public Panel GetCurrentPanel()
        {
            foreach (Panel panel in m_currentPanels)
            {
                if (panel.IsPlayerInCase())
                {
                    return panel;
                }
            }

            Debug.Log("Player is in no case from the page " + gameObject.name);
            return null;
        }

        public void Enable(bool enable)
        {
            gameObject.SetActive(enable);
        }

#region PageEdition

#if UNITY_EDITOR

        // TODO : generalize and made static in utils
        public void RefreshList()
        {
            m_currentPanels.Clear();

            foreach (Transform child in m_panelContainer.transform)
            {
                Panel component = child.GetComponent<Panel>();
                if (component != null)
                {
                    m_currentPanels.Add(component);
                    component.Init(m_margin);
                }
            }
        }

        public void InstantiatePanel()
        {
            GameObject panel_object = Instantiate(m_panelPrefab, m_panelContainer);
            Panel panel = panel_object.GetComponent<Panel>();

            panel.Init(m_margin);
            m_currentPanels.Add(panel);
        }
#endif

        protected override void OnUpdate(float elapsed_time)
        {
#if UNITY_EDITOR
            // will spawn a panel on every active page
            // doesnt work for some reason
            if (Input.GetKeyDown(KeyCode.P))
            {
                InstantiatePanel();
            }
#endif
        }

#endregion
    }
}