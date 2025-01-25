using CustomArchitecture;
using UnityEngine;
using UnityEngine.UI;

namespace Comic
{
    public class NpcIcon : BaseBehaviour
    {
        private VoiceType                       m_type;
        [SerializeField] private RectTransform  m_bubbleAnchor;
        [SerializeField] private Image          m_iconImage;

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
        }
    }
}