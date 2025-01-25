using CustomArchitecture;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using Sirenix.OdinInspector;
using TMPro;

namespace Comic
{
    public class ProgressionView : AView
    {
        #if UNITY_EDITOR
        [SerializeField, OnValueChanged("DebugGraphic")] private bool m_activeGraphic = true;

        private void DebugGraphic()
        {
            ActiveGraphic(m_activeGraphic);
        }
        #endif

        public override void ActiveGraphic(bool active)
        {
            Image[] images = gameObject.GetComponentsInChildren<Image>(true);
            TMP_Text[] texts = gameObject.GetComponentsInChildren<TMP_Text>(true);

            foreach (Image image in images)
            {
                image.gameObject.SetActive(active);
            }

            foreach (TMP_Text text in texts)
            {
                text.gameObject.SetActive(active);
            }
        }

        public override void Init()
        {
        }
    }
}
