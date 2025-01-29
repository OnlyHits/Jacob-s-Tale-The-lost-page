using UnityEngine;
using CustomArchitecture;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace Comic
{
    public class PropsBehaviour : BaseBehaviour
    {
        [SerializeField] private float m_scaleFactor;
        [SerializeField] private float m_tweenDuration;
        [SerializeField] private Ease m_easeType;
        private Vector2 m_baseScale;
        private Tween m_scaleTween = null;

        private void Awake()
        {
            m_baseScale = transform.localScale;
        }

        public override void Pause(bool pause)
        {
            base.Pause(pause);

            if (pause && m_scaleTween != null)
                m_scaleTween.Pause();
            else if (!pause && m_scaleTween != null)
                m_scaleTween.Play();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {            
            if (other.gameObject.GetComponent<Player>() != null)
            {
                if (m_scaleTween != null)
                {
                    m_scaleTween.Kill();
                    m_scaleTween = null;
                }

                transform.localScale = m_baseScale;
                
                m_scaleTween = transform
                    .DOScale(m_baseScale * m_scaleFactor, m_tweenDuration)
                    .SetEase(m_easeType)
                    .OnComplete(() => m_scaleTween = transform
                        .DOScale(m_baseScale, .1f)
                        .SetEase(Ease.Linear)
                        .OnComplete(() => m_scaleTween = null))
                    .OnKill(() => m_scaleTween = null);
            }
        }
    }
}
