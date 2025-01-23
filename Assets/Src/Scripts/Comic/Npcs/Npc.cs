using System.Collections.Generic;
using CustomArchitecture;
using UnityEngine;

namespace Comic
{
    public class Npc : Character
    {
        [Header("Others")]
        [SerializeField] private Transform m_lookTarget;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnUpdate(float elapsed_time)
        {
            base.OnUpdate(elapsed_time);

            if (m_lookTarget == null)
            {
                return;
            }

            Vector2 directionTarget = (Vector2)m_lookTarget.position - m_rb.position;
            SetSprireFaceDirection(directionTarget);
        }
    }
}
