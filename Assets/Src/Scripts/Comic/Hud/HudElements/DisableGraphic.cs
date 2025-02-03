using CustomArchitecture;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

namespace Comic
{
    public class DisableGraphic : BaseBehaviour
    {
        public void Start()
        {
            ComicGameCore.Instance.MainGameMode.SubscribeToAfterSwitchPage(After);
            ComicGameCore.Instance.MainGameMode.SubscribeToBeforeSwitchPage(Unactiv);
 //           ComicGameCore.Instance.MainGameMode.SubscribeToMiddleSwitchPage(Middle);
        }

        public void Unactiv(bool active, Page _1, Page _2)
        {
            if (!active)
                ActiveGraphic(true);
        }

        public void Middle(bool active, Page _1, Page _2)
        {
            if (active)
                ActiveGraphic(false);
            else
                ActiveGraphic(true);
        }

        public void After(bool active, Page _1, Page _2)
        {
            ActiveGraphic(false);
        }

        public void ActiveGraphic(bool active)
        {
            Image[] images = gameObject.GetComponentsInChildren<Image>(true);
            TMP_Text[] texts = gameObject.GetComponentsInChildren<TMP_Text>(true);

            foreach (Image image in images)
            {
                image.enabled = active;
            }

            foreach (TMP_Text text in texts)
            {
                text.enabled = active;
            }
        }
    }
}
