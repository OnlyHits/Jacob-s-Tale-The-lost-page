using System.Collections.Generic;
using CustomArchitecture;
using UnityEngine;

namespace Comic
{
    public class Npc : BaseBehaviour
    {
        [Header("Moves")]
        [SerializeField, ReadOnly] private bool m_faceRight = true;

        [Header("Animations")]
        [SerializeField] private Transform m_headPositionGoRight;
        [SerializeField] private Transform m_headPositionGoLeft;
        [SerializeField] private Transform m_head;
        private Vector2 m_baseHeadLocalPos;

        [Header("Others")]
        [SerializeField] private Rigidbody2D m_rb;
        [SerializeField] private Transform m_lookTarget;
        [HideInInspector] private List<SpriteRenderer> m_sprites = new List<SpriteRenderer>();

        private void Awake()
        {
            m_baseHeadLocalPos = m_head.localPosition;

            var sprites = GetComponentsInChildren<SpriteRenderer>();
            m_sprites.AddRange(sprites);
        }

        protected override void OnUpdate(float elapsed_time)
        {
            base.OnUpdate(elapsed_time);

            Vector2 directionTarget = (Vector2)m_lookTarget.position - m_rb.position;
            SetSprireFaceDirection(directionTarget);
        }

        #region SPRITES
        private void SetSprireFaceDirection(Vector2 direction)
        {
            bool wasFacingRight = m_faceRight;

            if (direction.x > 0)
            {
                m_faceRight = true;
            }
            else if (direction.x < 0)
            {
                m_faceRight = false;
            }

            if (wasFacingRight == m_faceRight)
            {
                return;
            }

            foreach (var sprite in m_sprites)
            {
                sprite.flipX = !m_faceRight;
                Transform parentHead = m_faceRight ? m_headPositionGoRight : m_headPositionGoLeft;
                m_head.parent = parentHead;
                m_head.localPosition = m_baseHeadLocalPos;
            }
        }
        #endregion SPRITES
    }
}
