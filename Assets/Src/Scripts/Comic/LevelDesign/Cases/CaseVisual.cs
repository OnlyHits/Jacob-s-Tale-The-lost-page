using System;
using System.Collections.Generic;
using CustomArchitecture;
using UnityEngine;

namespace Comic
{
    public class CaseVisual : BaseBehaviour
    {
        [SerializeField, ReadOnly] private List<SpriteRenderer> m_sprites = new List<SpriteRenderer>();
        [SerializeField, ReadOnly] private SpriteMask m_caseMask;

        public readonly string frontLayerName = "SwitchPage";
        public readonly string backLayerName = "NotSwitchPage";
        public readonly string defaultLayerName = "Default";

        public int frontLayerId => SortingLayer.NameToID(frontLayerName);
        public int backLayerId => SortingLayer.NameToID(backLayerName);
        public int defaultLayerId => SortingLayer.NameToID(defaultLayerName);


        private void Awake()
        {
            var sprites = GetComponentsInChildren<SpriteRenderer>();
            m_sprites.AddRange(sprites);
            m_caseMask = GetComponentInChildren<SpriteMask>();
        }

        public void PushFront()
        {
            m_caseMask.frontSortingLayerID = frontLayerId;
            m_caseMask.backSortingLayerID = frontLayerId;
            foreach (SpriteRenderer sprite in m_sprites)
            {
                sprite.sortingLayerName = frontLayerName;
            }
        }

        public void PushBack()
        {
            m_caseMask.frontSortingLayerID = backLayerId;
            m_caseMask.backSortingLayerID = backLayerId;
            foreach (SpriteRenderer sprite in m_sprites)
            {
                sprite.sortingLayerName = backLayerName;
            }
        }

        public void ResetDefault()
        {
            m_caseMask.frontSortingLayerID = defaultLayerId;
            m_caseMask.backSortingLayerID = defaultLayerId;
            foreach (SpriteRenderer sprite in m_sprites)
            {
                sprite.sortingLayerName = defaultLayerName;
            }
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
