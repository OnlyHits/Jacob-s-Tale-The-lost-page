using System;
using System.Collections.Generic;
using CustomArchitecture;
using UnityEngine;
using static Comic.Comic;

namespace Comic
{
    public class PageVisual : BaseBehaviour
    {
        [Header("Cases Visuals")]
        [SerializeField, ReadOnly] private List<CaseVisual> m_caseVisuals = new List<CaseVisual>();
        [SerializeField] private SpriteRenderer m_pageBackgroundSprite;
        [SerializeField] private SpriteRenderer m_pageBackgroundSpriteCenter;

        private void Awake()
        {
            var cases = GetComponentsInChildren<CaseVisual>(true);
            m_caseVisuals.AddRange(cases);
        }

        public void PushFront()
        {
            foreach (CaseVisual caseVisual in m_caseVisuals)
            {
                caseVisual.PushFront();
            }
            m_pageBackgroundSprite.sortingLayerName = frontLayerName;
            m_pageBackgroundSpriteCenter.sortingLayerName = frontLayerName;
        }

        public void PushBack()
        {
            foreach (CaseVisual caseVisual in m_caseVisuals)
            {
                caseVisual.PushBack();
            }
            m_pageBackgroundSprite.sortingLayerName = backLayerName;
            m_pageBackgroundSpriteCenter.sortingLayerName = backLayerName;
        }

        public void ResetDefault()
        {
            foreach (CaseVisual caseVisual in m_caseVisuals)
            {
                caseVisual.ResetDefault();
            }
            m_pageBackgroundSprite.sortingLayerName = defaultLayerName;
            m_pageBackgroundSpriteCenter.sortingLayerName = defaultLayerName;
        }

        public void AddOrderInLayer(int addValue)
        {
            foreach (CaseVisual caseVisual in m_caseVisuals)
            {
                caseVisual.AddOrderInLayer(addValue);
            }

            m_pageBackgroundSprite.sortingOrder += addValue;
        }

        public void SubOrderInLayer(int subValue)
        {
            foreach (CaseVisual caseVisual in m_caseVisuals)
            {
                caseVisual.SubOrderInLayer(subValue);
            }

            m_pageBackgroundSprite.sortingOrder -= subValue;
        }

        // public void SetMaskInteraction(bool on)
        // {
        //     m_pageBackgroundSprite.maskInteraction = on ? SpriteMaskInteraction.None : SpriteMaskInteraction.VisibleOutsideMask;
        // }
    }
}
