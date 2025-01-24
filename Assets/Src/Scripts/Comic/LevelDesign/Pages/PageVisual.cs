using System;
using System.Collections.Generic;
using CustomArchitecture;
using UnityEngine;

namespace Comic
{
    public class PageVisual : BaseBehaviour
    {
        [Header("Cases Visuals")]
        [SerializeField, ReadOnly] private List<CaseVisual> m_caseVisuals = new List<CaseVisual>();
        [SerializeField] private SpriteRenderer m_pageBackgroundSprite;

        private void Awake()
        {
            var cases = GetComponentsInChildren<CaseVisual>();
            m_caseVisuals.AddRange(cases);
        }

        public void PushFront()
        {
            foreach (CaseVisual caseVisual in m_caseVisuals)
            {
                caseVisual.PushFront();
            }
            m_pageBackgroundSprite.sortingLayerName = "SwitchPage";
        }

        public void PushBack()
        {
            foreach (CaseVisual caseVisual in m_caseVisuals)
            {
                caseVisual.PushBack();
            }
            m_pageBackgroundSprite.sortingLayerName = "NotSwitchPage";
        }

        public void ResetDefault()
        {
            foreach (CaseVisual caseVisual in m_caseVisuals)
            {
                caseVisual.ResetDefault();
            }
            m_pageBackgroundSprite.sortingLayerName = "Default";
        }

        /*
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
        */

        /*public void SetMaskInteraction(bool on)
        {
            m_pageBackgroundSprite.maskInteraction = on ? SpriteMaskInteraction.None : SpriteMaskInteraction.VisibleOutsideMask;
        }*/
    }
}
