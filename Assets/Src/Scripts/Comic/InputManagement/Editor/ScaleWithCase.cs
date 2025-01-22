#if UNITY_EDITOR
using System.Collections.Generic;
using System.ComponentModel;
using CustomArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Comic
{
    public partial class ScaleWithCaseEditor : MonoBehaviour
    {
        public Canvas canvas;
        public SpriteRenderer caseSprite;

        private void OnDrawGuizmos()
        {
            UpdateCanvas();
        }

        [Button("Force Resfresh")]
        private void UpdateCanvas()
        {

        }
    }
}
#endif
