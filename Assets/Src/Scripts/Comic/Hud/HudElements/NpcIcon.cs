using CustomArchitecture;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace Comic
{
    public class NpcIcon : BaseBehaviour
    {
        private VoiceType                       m_type;
        [SerializeField] private RectTransform  m_bubbleAnchor;
        [SerializeField] private Image          m_iconImage;
        private Tween                           m_scaleTween;

        public RectTransform GetBubbleAnchor() => m_bubbleAnchor;

        public void Init(VoiceType type, Sprite sprite)
        {
            m_iconImage.sprite = sprite;
            m_type = type;
        }

        public void SetIconSprite(Sprite sprite)
        {
            m_iconImage.sprite = sprite;
        }

        public override void Pause(bool pause)
        {
            base.Pause(pause);
            
            if (pause && m_scaleTween != null)
                m_scaleTween.Pause();
            else if (!pause && m_scaleTween != null)
                m_scaleTween.Play();

        }

        public void Appear(float duration)
        {
            if (IsCompute())
            {
                m_scaleTween.Kill();
                m_scaleTween = null;
            }

            transform.GetComponent<RectTransform>().localScale = Vector3.zero;

            m_scaleTween = transform.GetComponent<RectTransform>()
                .DOScale(Vector3.one, duration)
                .SetEase(Ease.OutCirc)
                .OnComplete(() => m_scaleTween = null)
                .OnKill(() => m_scaleTween = null);

            if (m_pause)
                m_scaleTween.Pause();
        }

        public void Disappear()
        {
            if (IsCompute())
            {
                m_scaleTween.Kill();
                m_scaleTween = null;
            }

            m_scaleTween = transform.GetComponent<RectTransform>()
                .DOScale(Vector3.zero, .3f)
                .SetEase(Ease.OutCirc)
                .OnComplete(() => { m_scaleTween = null; gameObject.SetActive(false); })
                .OnKill(() => { m_scaleTween = null; gameObject.SetActive(false); });

            if (m_pause)
                m_scaleTween.Pause();
        }

        public bool IsCompute()
        {
            return m_scaleTween != null && m_scaleTween.IsActive() && m_scaleTween.IsPlaying();
        }
    }
}