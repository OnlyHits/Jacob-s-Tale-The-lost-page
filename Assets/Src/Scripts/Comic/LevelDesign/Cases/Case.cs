using System;
using System.Collections.Generic;
using CustomArchitecture;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Comic
{
    public class Case : BaseBehaviour
    {
        [SerializeField] private List<Transform> m_allElements;
        [SerializeField] private Transform m_elements;
        [SerializeField] private SpriteRenderer m_caseSprite;

        private List<Tween> m_rotCaseTweens = new List<Tween>();
        private bool m_isRotating = false;
        private Vector3 m_currentRotation = Vector3.zero;


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

        public bool IsRotating()
        {
            return m_isRotating;
        }

        public void Rotate180(float speed, Action endRotateCallback)
        {
            if (m_rotCaseTweens.Count > 0)
                return;

            m_isRotating = true;

            Vector3 destRot = m_currentRotation + new Vector3(0, 0, 180);
            m_currentRotation += new Vector3(0, 0, 180);

            foreach (Transform t in m_allElements)
            {
                Tween tween = t.DOLocalRotate(destRot, 0.5f);
                tween.OnComplete(() =>
                    {
                        if (m_rotCaseTweens.Contains(tween))
                        {
                            m_rotCaseTweens.Remove(tween);
                            m_isRotating = false;
                            endRotateCallback?.Invoke();
                        }
                        tween = null;
                    });
                m_rotCaseTweens.Add(tween);
            }
        }

        public Transform GetCaseTransform()
        {
            return m_elements.transform;
        }
    }
}
