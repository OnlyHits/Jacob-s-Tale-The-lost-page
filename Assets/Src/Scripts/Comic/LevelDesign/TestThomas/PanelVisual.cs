using UnityEngine;
using CustomArchitecture;

namespace Comic
{
    [ExecuteAlways]
    public class PanelVisual : BaseBehaviour
    {
        [SerializeField] private SpriteRenderer m_referenceSprite;
        [SerializeField] private SpriteRenderer m_outlineSprite;
        [SerializeField] private float m_outlineThickness = 0.1f;

        public SpriteRenderer PanelReference() => m_referenceSprite;
        public void LockPosition() => transform.localPosition = Vector3.zero;

        public void Init()
        {
            // Not sure about that here until it's made in Awake method
            // Not sure that we need it att all anyway until it is always resolved in editor
            ResizeSprite();
        }

        private void Awake()
        {
            if (m_referenceSprite == null)
                m_referenceSprite = GetComponent<SpriteRenderer>();

            if (m_outlineSprite == null && m_referenceSprite != null)
            {
                m_outlineSprite = GetComponentInChildren<SpriteRenderer>();

                if (m_outlineSprite == null)
                {
                    Debug.LogWarning("Outline sprite is missing! Please assign it.");
                    return;
                }
            }

            ResizeSprite();
        }

        protected override void OnUpdate(float elapsed_time)
        {
#if UNITY_EDITOR
            ResizeSprite();
#endif
        }

        private void ResizeSprite()
        {
            if (m_referenceSprite == null || m_outlineSprite == null)
            {
                Debug.LogWarning("Reference or Outline Sprite not assigned!");
                return;
            }

            Vector2 refSize = m_referenceSprite.bounds.size;
            Vector2 newSize = refSize + new Vector2(m_outlineThickness, m_outlineThickness);
            Vector2 targetOriginalSize = m_outlineSprite.sprite.bounds.size;
            Vector3 parentScale = m_referenceSprite.transform.lossyScale;

            m_outlineSprite.transform.localScale = new Vector3(
                (newSize.x / targetOriginalSize.x) / parentScale.x,
                (newSize.y / targetOriginalSize.y) / parentScale.y,
                1
            );

            m_outlineSprite.transform.localPosition = Vector3.zero;
            m_outlineSprite.transform.rotation = transform.rotation;
        }
    }
}
