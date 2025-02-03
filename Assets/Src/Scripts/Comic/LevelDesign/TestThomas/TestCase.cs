using System;
using System.Collections.Generic;
using CustomArchitecture;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Comic
{
    public class TestCase : BaseBehaviour
    {
            public SpriteRenderer parentRenderer;  // Reference to the parent SpriteRenderer
            private SpriteRenderer childRenderer;  // Reference to the child SpriteRenderer

            void Start()
            {
                childRenderer = GetComponent<SpriteRenderer>();
            }

            void LateUpdate()
            {
                if (parentRenderer == null || childRenderer == null) return;

                // Get the bounds of the parent and child
                Bounds parentBounds = parentRenderer.bounds;
                Bounds childBounds = childRenderer.bounds;

                // Get the child's current position
                Vector3 newPosition = transform.position;

                // Clamp the child's position inside the parent
                newPosition.x = Mathf.Clamp(newPosition.x, 
                    parentBounds.min.x + (childBounds.size.x / 2), 
                    parentBounds.max.x - (childBounds.size.x / 2));

                newPosition.y = Mathf.Clamp(newPosition.y, 
                    parentBounds.min.y + (childBounds.size.y / 2), 
                    parentBounds.max.y - (childBounds.size.y / 2));

                // Apply the clamped position
                transform.position = newPosition;
            }
        // [SerializeField] private List<Transform> m_allElements;
        // [SerializeField] private Transform m_elements;
        // [SerializeField] private SpriteRenderer m_caseSprite;

        // private List<Tween> m_rotCaseTweens = new List<Tween>();
        // private bool m_isRotating = false;
        // private Vector3 m_currentRotation = Vector3.zero;


        // public bool IsPlayerInCase()
        // {
        //     bool isPlayerIn = false;
        //     bool isInWidth = false;
        //     bool isInHeight = false;

        //     Vector3 playerPos = ComicGameCore.Instance.MainGameMode.GetCharacterManager().GetPlayer().transform.position;
        //     Vector3 casePos = m_caseSprite.transform.position;

        //     float width = m_caseSprite.bounds.size.x / 2f;
        //     float height = m_caseSprite.bounds.size.y / 2f;

        //     if (casePos.x - width < playerPos.x && playerPos.x < casePos.x + width)
        //     {
        //         isInWidth = true;
        //     }

        //     if (casePos.y - height < playerPos.y && playerPos.y < casePos.y + height)
        //     {
        //         isInHeight = true;
        //     }

        //     isPlayerIn = isInHeight && isInWidth;

        //     return isPlayerIn;
        // }

        // public bool IsRotating()
        // {
        //     return m_isRotating;
        // }

        // public void Rotate180(float speed, Action endRotateCallback)
        // {
        //     if (m_rotCaseTweens.Count > 0)
        //         return;

        //     m_isRotating = true;

        //     Vector3 destRot = m_currentRotation + new Vector3(0, 0, 180);
        //     m_currentRotation += new Vector3(0, 0, 180);

        //     foreach (Transform t in m_allElements)
        //     {
        //         Tween tween = t.DOLocalRotate(destRot, 0.5f);
        //         tween.OnComplete(() =>
        //             {
        //                 if (m_rotCaseTweens.Contains(tween))
        //                 {
        //                     m_rotCaseTweens.Remove(tween);
        //                     m_isRotating = false;
        //                     endRotateCallback?.Invoke();
        //                 }
        //                 tween = null;
        //             });
        //         m_rotCaseTweens.Add(tween);
        //     }
        // }

        // public Transform GetCaseTransform()
        // {
        //     return m_elements.transform;
        // }
    }
}
