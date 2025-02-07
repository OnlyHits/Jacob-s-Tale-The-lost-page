//#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Comic
{
    // public class PageEditor : MonoBehaviour
    // {
    //     [SerializeField, ReadOnly] private List<CaseEditor> m_editorCases = new List<CaseEditor>();
    //     [SerializeField, ReadOnly] private PageMarginEditor m_editorPageMargin;


    //     [OnValueChanged("OnDisplayMarginCasesChanged")]
    //     public bool m_displayMarginCases = false;

    //     [OnValueChanged("OnDisplayMarginPageChanged")]
    //     public bool m_displayMarginPage = false;

    //     [Button("Refresh")]
    //     private void Refresh()
    //     {
    //         TryInitPageMargin();
    //         UpdatePageVisual();
    //         TryInitCases();
    //         UpdateCaseVisual();
    //     }

    //     private void TryInitCases()
    //     {
    //         var editorCases = GetComponentsInChildren<CaseEditor>();
    //         m_editorCases.Clear();
    //         m_editorCases.AddRange(editorCases);
    //     }

    //     private void TryInitPageMargin()
    //     {
    //         var editorPageMargin = GetComponentInChildren<PageMarginEditor>();
    //         m_editorPageMargin = editorPageMargin;
    //     }

    //     private void OnDisplayMarginCasesChanged()
    //     {
    //         if (m_editorCases.Count <= 0)
    //         {
    //             TryInitCases();
    //         }
    //         UpdateCaseVisual();
    //     }

    //     private void UpdateCaseVisual()
    //     {
    //         foreach (CaseEditor caseEditor in m_editorCases)
    //         {
    //             caseEditor.EnableCanvasVisual(m_displayMarginCases);
    //         }
    //     }

    //     private void OnDisplayMarginPageChanged()
    //     {
    //         if (m_editorPageMargin == null)
    //         {
    //             TryInitPageMargin();
    //         }
    //         UpdatePageVisual();
    //     }

    //     private void UpdatePageVisual()
    //     {
    //         m_editorPageMargin.EnableVisual(m_displayMarginPage);
    //     }

    // }
}
//#endif
