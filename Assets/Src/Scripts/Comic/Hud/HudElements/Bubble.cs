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
        [SerializeField] private RectTransform          m_pinRect;
        private NpcIcon                                 m_iconRect;
        private RectTransform                           m_containerRect;
        private Canvas                                  m_canvas;
        private Tween                                   m_scaleTween = null;

        public void Init(NpcIcon icon_rect, RectTransform container_rect, Canvas canvas)
        {
            m_iconRect = icon_rect;
            m_containerRect = container_rect;
            m_canvas = canvas;
        }

        protected override void OnLateUpdate(float elapsed_time)
        {
//            SetBubblePosition();
            ConstraintPosition();
            SetPinTransform();
        }

        private void ConstraintPosition()
        {
            Vector3[] parentCorners = new Vector3[4];
            Vector3[] childCorners = new Vector3[4];
            RectTransform rect = gameObject.GetComponent<RectTransform>();

            m_containerRect.GetWorldCorners(parentCorners);
            rect.GetWorldCorners(childCorners);

            Vector3 offset = Vector3.zero;

            if (childCorners[0].x < parentCorners[0].x)
                offset.x = parentCorners[0].x - childCorners[0].x;
            if (childCorners[2].x > parentCorners[2].x)
                offset.x = parentCorners[2].x - childCorners[2].x;
            if (childCorners[0].y < parentCorners[0].y)
                offset.y = parentCorners[0].y - childCorners[0].y;
            if (childCorners[1].y > parentCorners[1].y)
                offset.y = parentCorners[1].y - childCorners[1].y;

            rect.position += offset;
        }

        private void SetPinTransform()
        {
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            Vector2 self_position = rect.TransformPoint(rect.rect.center);
            Vector2 direction = (self_position - (Vector2)m_iconRect.GetBubbleAnchor().position).normalized;
            float distance = Vector2.Distance(
                m_pinRect.InverseTransformPoint(m_iconRect.GetBubbleAnchor().position),
                m_pinRect.InverseTransformPoint(self_position));
            
            m_pinRect.position = (self_position + (Vector2)m_iconRect.GetBubbleAnchor().position) * .5f;
            m_pinRect.localPosition = new Vector3(m_pinRect.localPosition.x, m_pinRect.localPosition.y, 0f);
            m_pinRect.rotation = Quaternion.LookRotation(m_pinRect.forward, direction);
            m_pinRect.sizeDelta = new Vector2(m_pinRect.rect.width, distance);
        }

        private void SetBubblePosition()
        {

            // bubble_rect.pivot = new Vector2(.5f, .5f);

            // Vector3 offset_x = new Vector3(icon_rect.rect.width + bubble_rect.rect.width, 0, 0);
            // Vector3 offset_y = new Vector3(0, container_rect.rect.height - bubble_rect.rect.height, 0);
            // Vector3 top_position = container_rect.position + container_rect.TransformPoint(offset_y * .5f);
            // Vector3 bot_position = container_rect.position - container_rect.TransformPoint(offset_y * .5f);

            // float y_position = Mathf.Clamp(icon_rect.position.y, bot_position.y, top_position.y);

            // bubble_rect.position = new Vector3(0, y_position, bubble_rect.position.z);

            // gameObject.GetComponent<RectTransform>().SetPivotInWorldSpace(m_iconRect.position);
        }
    
        #region EaseCoroutine

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

        public bool IsCompute()
        {
            return m_scaleTween != null && m_scaleTween.IsActive() && m_scaleTween.IsPlaying();
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

        #endregion
    }
}