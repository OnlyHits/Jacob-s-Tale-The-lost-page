using System.Collections.Generic;
using CustomArchitecture;
using UnityEngine;

namespace Comic
{
    public partial class Character : BaseBehaviour
    {
        [Header("Facing")]
        [SerializeField, ReadOnly] protected bool m_faceRight = true;

        [Header("Animations")]
        [SerializeField] protected Transform m_headPositionGoRight;
        [SerializeField] protected Transform m_headPositionGoLeft;
        [SerializeField] protected Transform m_head;
        protected Vector2 m_baseHeadLocalPos;

        [Header("Others")]
        [SerializeField] protected Rigidbody2D m_rb;
        [HideInInspector] protected List<SpriteRenderer> m_sprites = new List<SpriteRenderer>();

        protected virtual void Awake()
        {
            m_baseHeadLocalPos = m_head.localPosition;

            var sprites = GetComponentsInChildren<SpriteRenderer>();
            m_sprites.AddRange(sprites);
        }

        protected override void OnUpdate(float elapsed_time)
        {
            base.OnUpdate(elapsed_time);
        }

        #region SPRITES
        protected void SetSprireFaceDirection(Vector2 direction)
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
