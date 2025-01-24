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

        private void Awake()
        {
            var sprites = GetComponentsInChildren<SpriteRenderer>();
            m_sprites.AddRange(sprites);
            m_caseMask = GetComponentInChildren<SpriteMask>();
        }

        public void PushFront()
        {
            int sortingLayerID = SortingLayer.NameToID("SwitchPage");
            m_caseMask.frontSortingLayerID = sortingLayerID;
            m_caseMask.backSortingLayerID = sortingLayerID;
            //m_caseMask.sortingLayerName = "SwitchPage";

            foreach (SpriteRenderer sprite in m_sprites)
            {
                sprite.sortingLayerName = "SwitchPage";
            }
        }

        public void PushBack()
        {
            int sortingLayerID = SortingLayer.NameToID("NotSwitchPage");
            m_caseMask.frontSortingLayerID = sortingLayerID;
            m_caseMask.backSortingLayerID = sortingLayerID;
            //m_caseMask.sortingLayerName = "NotSwitchPage";

            foreach (SpriteRenderer sprite in m_sprites)
            {
                sprite.sortingLayerName = "NotSwitchPage";
            }
        }

        public void ResetDefault()
        {
            int sortingLayerID = SortingLayer.NameToID("Default");
            m_caseMask.frontSortingLayerID = sortingLayerID;
            m_caseMask.backSortingLayerID = sortingLayerID;
            //m_caseMask.sortingLayerName = "Default";

            foreach (SpriteRenderer sprite in m_sprites)
            {
                sprite.sortingLayerName = "Default";
            }
        }

        /*public void AddOrderInLayer(int addValue)
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
        }*/
    }
}
