using System.Collections.Generic;
using CustomArchitecture;
using Unity.Cinemachine;
using UnityEngine;
using static Comic.Comic;

namespace Comic
{
    public class Case : BaseBehaviour
    {
        [SerializeField] private List<Transform> m_allElements;
        [SerializeField] private Transform m_elements;
        [SerializeField] private SpriteRenderer m_caseSprite;

        public bool IsPlayerInCase()
        {
            bool isPlayerIn = false;
            bool isInWidth = false;
            bool isInHeight = false;

            Vector3 playerPos = ComicGameCore.Instance.GetGameMode<MainGameMode>().GetPlayer().transform.position;
            Vector3 casePos = m_caseSprite.transform.position;

            float width = m_caseSprite.bounds.size.x / 2f;
            float height = m_caseSprite.bounds.size.y / 2f;

            if (casePos.x - width < playerPos.x && playerPos.x < casePos.x + width)
            {
                isInWidth = true;
            }

            if (casePos.y - height < playerPos.y && playerPos.y < casePos.y + height)
            {
                isInHeight = true;
            }

            isPlayerIn = isInHeight && isInWidth;

            return isPlayerIn;
        }

        public List<Transform> GetCaseTransforms()
        {
            return m_allElements;
        }

        public Transform GetCaseTransform()
        {
            return m_elements.transform;
        }
    }
}
