using System;
using System.Collections.Generic;
using CustomArchitecture;
using UnityEngine;

namespace Comic
{
    public class CaseVisual : BaseBehaviour
    {
        [SerializeField, ReadOnly] private List<SpriteRenderer> m_sprites = new List<SpriteRenderer>();

        private void Awake()
        {
            var sprites = GetComponentsInChildren<SpriteRenderer>();
            m_sprites.AddRange(sprites);
        }

        public void AddOrderInLayer(int addValue)
        {
            foreach (SpriteRenderer sprite in m_sprites)
            {
                sprite.sortingOrder += addValue;
            }
        }

        public void SubOrderInLayer(int subValue)
        {
            foreach (SpriteRenderer sprite in m_sprites)
            {
                sprite.sortingOrder -= subValue;
            }
        }
    }
}
