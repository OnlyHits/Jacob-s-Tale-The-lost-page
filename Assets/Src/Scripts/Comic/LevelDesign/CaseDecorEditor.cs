#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Comic
{
    public struct CaseDecorProvider
    {
        public CaseDecorProvider(CaseEditor.DecorType dt, Sprite sB = null, Sprite sF = null, Sprite sC = null)
        {
            decorType = dt;
            sBackground = sB;
            sFloor = sF;
            sCeiling = sC;
        }

        public CaseEditor.DecorType decorType;
        public Sprite sBackground;
        public Sprite sFloor;
        public Sprite sCeiling;
    }
    public class CaseDecorEditor : MonoBehaviour
    {
        public Transform m_caseSprite;
        public Transform m_decorTransform;
        [SerializeField, ReadOnly] private CaseEditor.DecorType m_decorType;

        [Space]
        public SpriteRenderer m_wall;
        public SpriteRenderer m_floor;
        public SpriteRenderer m_ceiling;

        public void Setup(CaseDecorProvider provider)
        {
            m_decorType = provider.decorType;

            m_wall.sprite = provider.sBackground;
            m_floor.sprite = provider.sFloor;
            m_ceiling.sprite = provider.sCeiling;

            switch (m_decorType)
            {
                case CaseEditor.DecorType.Room:
                    m_wall.gameObject.SetActive(true);
                    m_floor.gameObject.SetActive(true);
                    m_ceiling.gameObject.SetActive(true);
                    break;
                case CaseEditor.DecorType.Outside:
                    m_wall.gameObject.SetActive(true);
                    m_floor.gameObject.SetActive(true);
                    m_ceiling.gameObject.SetActive(false);
                    break;
            }
        }

        [Button("Force Refresh")]
        public void Refresh()
        {
            if (m_decorTransform == null)
            {
                Debug.LogWarning("Decor parent transform not set on [" + gameObject.name + "]");
                return;
            }

            if (m_wall == null || m_floor == null || m_ceiling == null)
            {
                Debug.LogWarning("Decor childrens (wall, floor, etc) not set on [" + gameObject.name + "]");
                return;
            }

            //UpdateElements();
        }

        private void UpdateElements()
        {
            m_decorTransform.position = m_caseSprite.position;

            float w = m_caseSprite.localScale.x;
            float h = m_caseSprite.localScale.y;
            m_wall.size = new Vector2(w, h);
        }
    }
}
#endif