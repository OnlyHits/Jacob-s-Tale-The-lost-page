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
        [SerializeField] private TMP_AnimatedText       m_dialogue;
        [SerializeField] private RectTransform          m_pinRectTransform;
        private Tween                                   m_scaleTween = null;

        public bool IsCompute()
        {
            return m_scaleTween != null && m_scaleTween.IsActive() && m_scaleTween.IsPlaying();
        }

        public void SetPinPosition(RectTransform target)
        {
            RectTransform rt = gameObject.GetComponent<RectTransform>();
            Vector2 self_position = rt.TransformPoint(rt.rect.center);
            Vector2 direction = ((Vector2)rt.position - self_position).normalized;
            // float distance = Vector2.Distance(self_position, rt.position);

            // Debug.Log("Rect position " + self_position);
            // Debug.Log("Base position " + rt.anchoredPosition);

            // Debug.Log(distance);
            // distance = m_pinRectTransform.TransformPoint(new Vector2(0, distance)).y;
            // Debug.Log(distance);

            // Debug.Log("_______"); 

            m_pinRectTransform.position = rt.TransformPoint(rt.rect.center);
            m_pinRectTransform.rotation = Quaternion.LookRotation(m_pinRectTransform.forward, direction);
            // m_pinRectTransform.sizeDelta = new Vector2(m_pinRectTransform.sizeDelta.x, distance);
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

        public IEnumerator TriggerAndWaitDialogue(DialogueType type)
        {
            DialogueConfig config = TMP_AnimatedTextController.Instance.GetDialogueConfig(type);
            DynamicDialogueData datas = TMP_AnimatedTextController.Instance.GetDialogueDatas(type);

            m_dialogue.StartDialogue(config, datas);

            // while (!m_dialogue.IsFinish())
                yield return null;
        }

    }
}