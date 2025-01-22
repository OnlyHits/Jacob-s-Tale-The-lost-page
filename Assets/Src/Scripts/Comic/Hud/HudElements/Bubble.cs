using CustomArchitecture;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using System.Collections;

namespace Comic
{
    public enum BubbleAppearIntensity
    {
        Intensity_Normal,
        Intensity_Medium,
        Intensity_Hard
    }

    public class Bubble : BaseBehaviour
    {
        [SerializeField] private TMPDialogue    m_dialogue;
        private Tween                           m_scaleTween = null;

        public bool IsCompute()
        {
            return m_scaleTween != null && m_scaleTween.IsActive() && m_scaleTween.IsPlaying();
        }

        public void Appear(BubbleAppearIntensity intensity)
        {
            if (m_scaleTween != null && m_scaleTween.IsActive() && m_scaleTween.IsPlaying())
            {
                m_scaleTween.Kill();
                m_scaleTween = null;
            }

            if (intensity == BubbleAppearIntensity.Intensity_Normal)
                NormalAppear();
            // else if (intensity == BubbleAppearIntensity.Intensity_Medium)
            // ;
            else if (intensity == BubbleAppearIntensity.Intensity_Hard)
                HardAppear();
        }

        public void NormalAppear()
        {
            transform.GetComponent<RectTransform>().localScale = Vector3.zero;

            m_scaleTween = transform.GetComponent<RectTransform>()
                .DOScale(Vector3.one, .8f)
                .SetEase(Ease.OutCirc);
                // .OnComplete(() => { gameObject.SetActive(false); });
        }

        public void HardAppear()
        {
            transform.GetComponent<RectTransform>().localScale = Vector3.zero;

            m_scaleTween = transform.GetComponent<RectTransform>()
                .DOScale(Vector3.one, .5f)
                .SetEase(Ease.OutBounce);
                // .OnComplete(() => { gameObject.SetActive(false); });
        }

        public IEnumerator TriggerAndWaitDialogue(string dialogue_name)
        {
            m_dialogue.StartDialogue("Welcome");

            while (!m_dialogue.IsFinish())
                yield return null;
        }

    }
}