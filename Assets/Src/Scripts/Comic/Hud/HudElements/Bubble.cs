using CustomArchitecture;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Comic
{
    public class Bubble : BaseBehaviour
    {
        private Dictionary<DialogueAppearIntensity, float> m_durationByIntensity = null;
        private float m_disappearDuration = .3f;

        private Action<float> m_onAppearCallback;
        private Action<float> m_onDisappearCallback;

        [SerializeField] private TMP_AnimatedText m_dialogue;
        [SerializeField] private RectTransform m_pinRect;
        private NpcIcon m_target;
        private RectTransform m_containerRect;
        private Tween m_scaleTween = null;
        private Coroutine m_dialogueCoroutine = null;

        public void SetTarget(NpcIcon icon) => m_target = icon;
        public TMP_AnimatedText GetAnimatedText() => m_dialogue;
        protected virtual bool IsBubbleChoice() => false;

        public void SubscribeToAppearCallback(Action<float> function)
        {
            m_onAppearCallback -= function;
            m_onAppearCallback += function;
        }

        public void SubscribeToDisappearCallback(Action<float> function)
        {
            m_onDisappearCallback -= function;
            m_onDisappearCallback += function;
        }

        public virtual void Init(RectTransform container_rect)
        {
            if (!IsBubbleChoice())
                ComicGameCore.Instance.MainGameMode.GetNavigationInput().SubscribeToValidate(OnValid);
            
            m_containerRect = container_rect;
            gameObject.SetActive(false);
            gameObject.GetComponent<RectTransform>().position = m_containerRect.position;

            m_durationByIntensity = new()
            {
                { DialogueAppearIntensity.Intensity_Normal, .7f },
                { DialogueAppearIntensity.Intensity_Medium, .5f },
                { DialogueAppearIntensity.Intensity_Hard, .3f},
            };
        }

        private void OnValid(InputType input, bool v)
        {
            if (input == InputType.PRESSED)
            {
                m_dialogue.Validate(true);
            }
            else if (input == InputType.COMPUTED)
            {
                m_dialogue.Validate(true);
            }
            else if (input == InputType.RELEASED)
            {
                m_dialogue.Validate(false);
            }
        }

        public override void Pause(bool pause)
        {
            base.Pause(pause);

            m_dialogue.Pause(pause);

            if (pause && m_scaleTween != null)
                m_scaleTween.Pause();
            else if (!pause && m_scaleTween != null)
                m_scaleTween.Play();
        }

        protected override void OnLateUpdate(float elapsed_time)
        {
            if (m_target != null)
                SetPinTransform();
        }

        public void SetupDialogue(DialogueType type)
        {
            if (m_dialogueCoroutine != null)
            {
                m_dialogue.StopDialogue();
                StopCoroutine(m_dialogueCoroutine);
                m_dialogueCoroutine = null;
            }

            DialogueConfig config = TMP_AnimatedTextController.Instance.GetDialogueConfig<ComicGameCore>(type);
            DynamicDialogueData datas = TMP_AnimatedTextController.Instance.GetDialogueDatas<ComicGameCore>(type);

            m_dialogue.StartDialogue(config, datas);
        }

        public bool IsDialogueComplete()
        {
            if (m_pause)
                return false;

            if (m_dialogue.GetState() == TMP_AnimatedText_State.State_Displaying)
                return false;

            if (m_scaleTween != null && m_scaleTween.IsActive())
                return false;

            return true;
        }

        protected virtual IEnumerator WaitForInput()
        {
            yield return null;
        }

        public IEnumerator DialogueCoroutine(DialogueAppearIntensity intensity, bool wait_input, bool target_main)
        {
            Appear(intensity);

            if (target_main)
                m_onAppearCallback?.Invoke(m_durationByIntensity[intensity]);

            yield return new WaitUntil(() => IsDialogueComplete()
                && (m_target != null && !m_target.IsCompute()));

            if (wait_input)
                yield return StartCoroutine(WaitForInput());

            Disappear();

            if (target_main)
                m_onDisappearCallback?.Invoke(m_disappearDuration);

            yield return new WaitWhile(() =>
                m_scaleTween != null || m_pause);

            gameObject.SetActive(false);
        }

        #region Constraints

        private void SetPinTransform()
        {
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            Vector2 self_position = rect.TransformPoint(rect.rect.center);
            Vector2 direction = (self_position - (Vector2)m_target.GetBubbleAnchor().position).normalized;
            float distance = Vector2.Distance(
                m_pinRect.InverseTransformPoint(m_target.GetBubbleAnchor().position),
                m_pinRect.InverseTransformPoint(self_position));

            m_pinRect.position = (self_position + (Vector2)m_target.GetBubbleAnchor().position) * .5f;
            m_pinRect.localPosition = new Vector3(m_pinRect.localPosition.x, m_pinRect.localPosition.y, 0f);
            m_pinRect.rotation = Quaternion.LookRotation(m_pinRect.forward, direction);
            m_pinRect.sizeDelta = new Vector2(m_pinRect.rect.width, distance);
        }

        #endregion

        #region EaseCoroutine

        public void Appear(DialogueAppearIntensity intensity)
        {
            if (m_target != null)
                gameObject.GetComponent<RectTransform>().SetPivotInWorldSpace(m_target.GetBubbleAnchor().position);

            if (IsCompute())
            {
                m_scaleTween.Kill();
                m_scaleTween = null;
            }

            if (intensity == DialogueAppearIntensity.Intensity_Normal)
                NormalAppear();
            else if (intensity == DialogueAppearIntensity.Intensity_Medium)
                MediumAppear();
            else if (intensity == DialogueAppearIntensity.Intensity_Hard)
                HardAppear();

            m_scaleTween
                .OnComplete(() => m_scaleTween = null)
                .OnKill(() => m_scaleTween = null);

            if (m_pause)
                m_scaleTween.Pause();
        }

        public void Disappear()
        {
            gameObject.GetComponent<RectTransform>().SetPivotInWorldSpace(m_target.GetBubbleAnchor().position);

            if (IsCompute())
            {
                m_scaleTween.Kill();
                m_scaleTween = null;
            }

            m_scaleTween = transform.GetComponent<RectTransform>()
                .DOScale(Vector3.zero, m_disappearDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() => m_scaleTween = null)
                .OnKill(() => m_scaleTween = null);

            if (m_pause)
                m_scaleTween.Pause();
        }

        public bool IsCompute()
        {
            return m_scaleTween != null && m_scaleTween.IsActive() && m_scaleTween.IsPlaying();
        }

        public void NormalAppear()
        {
            transform.GetComponent<RectTransform>().localScale = Vector3.zero;

            m_scaleTween = transform.GetComponent<RectTransform>()
                .DOScale(Vector3.one, m_durationByIntensity[DialogueAppearIntensity.Intensity_Normal])
                .SetEase(Ease.OutBack);
        }

        public void MediumAppear()
        {
            transform.GetComponent<RectTransform>().localScale = Vector3.zero;

            m_scaleTween = transform.GetComponent<RectTransform>()
                .DOScale(Vector3.one, m_durationByIntensity[DialogueAppearIntensity.Intensity_Medium])
                .SetEase(Ease.OutBack);
        }

        public void HardAppear()
        {
            transform.GetComponent<RectTransform>().localScale = Vector3.zero;

            m_scaleTween = transform.GetComponent<RectTransform>()
                .DOScale(Vector3.one, m_durationByIntensity[DialogueAppearIntensity.Intensity_Hard])
                .SetEase(Ease.OutBack);
        }

        #endregion
    }
}