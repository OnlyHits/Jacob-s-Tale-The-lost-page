using CustomArchitecture;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace Comic
{
    public class TurningPage : BaseBehaviour
    {
        [SerializeField] private Image m_turningPage;
        [SerializeField] private Image m_leftShadow;
        [SerializeField] private Image m_rightShadow;

        [SerializeField] private float m_turnDuration;

        private Sprite m_frontSprite;
        private Sprite m_backSprite;

        private Sequence m_turnSequence;
        // private Sequence m_shadowSequence;

        private Tween m_rotationTween;

        public void SetFrontSprite(Sprite sprite) => m_frontSprite = sprite;
        public void SetBackSprite(Sprite sprite) => m_backSprite = sprite;
        private Action m_onEndTurning;

        public void RegisterToEndTurning(Action function)
        {
            m_onEndTurning -= function;
            m_onEndTurning += function;
        }

        public void PreviousPage()
        {
            // gameObject.SetActive(true);
            
            // if (m_turnSequence != null)
            // {
            //     m_turnSequence.Kill();
            //     m_turnSequence = null;
            // }

            // m_turningPage.sprite = m_backSprite;
            
            // RectTransform rect = m_turningPage.GetComponent<RectTransform>();

            // rect.eulerAngles = new Vector3(0, 180, 0);

            // var right_color = m_rightShadow.color;
            // right_color.a = 0f;
            // m_rightShadow.color = right_color;
            // right_color.a = 1f;

            // var left_color = m_leftShadow.color;
            // left_color.a = 1f;
            // m_leftShadow.color = left_color;
            // left_color.a = 0f;

            // m_turnSequence = DOTween.Sequence();

            // m_turnSequence.Append(rect.DORotate(new Vector3(0, 90, 0), m_turnDuration * 0.5f, RotateMode.FastBeyond360)
            //     .SetEase(Ease.InQuad));
            // m_turnSequence.Append(rect.DORotate(Vector3.zero, m_turnDuration * 0.5f, RotateMode.FastBeyond360)
            //     .SetEase(Ease.OutQuad));
            // m_turnSequence.OnComplete(() => m_turnSequence = null);

            // m_shadowSequence = DOTween.Sequence();

            // m_shadowSequence.Append(m_leftShadow.DOColor(left_color, m_turnDuration * .5f).SetEase(Ease.InQuad));
            // m_shadowSequence.Append(m_rightShadow.DOColor(right_color, m_turnDuration * .5f).SetEase(Ease.OutQuad));
            // m_shadowSequence.OnComplete(() => m_shadowSequence = null);
        }

        public void NextPage()
        {
            gameObject.SetActive(true);

            if (m_turnSequence != null)
            {
                m_turnSequence.Kill();
                m_turnSequence = null;
            }

            m_turningPage.sprite = m_frontSprite;

            RectTransform rect = m_turningPage.GetComponent<RectTransform>();

            rect.eulerAngles = Vector3.zero;

            var right_color = m_rightShadow.color;
            right_color.a = 1f;
            m_rightShadow.color = right_color;
            right_color.a = 0f;

            var left_color = m_leftShadow.color;
            left_color.a = 0f;
            m_leftShadow.color = left_color;
            left_color.a = 1f;

            SetupRect(true);

            m_turnSequence = DOTween.Sequence();

            Sequence rotate_sequence = DOTween.Sequence();
            Sequence shadow_sequence = DOTween.Sequence();

            rotate_sequence.Append(rect.DORotateQuaternion(Quaternion.Euler(0, 90, 0), m_turnDuration * 0.5f)
                .SetEase(Ease.InQuad)
                .OnComplete(() => {
                    Vector3 rotation = rect.eulerAngles;
                    rotation.y = 280f;
                    rect.eulerAngles = rotation;

                    SetupRect(false);

                    m_turningPage.sprite = m_backSprite;
                }));
            rotate_sequence.Append(rect.DORotateQuaternion(Quaternion.Euler(0, 360, 0), m_turnDuration * 0.5f)
                .SetEase(Ease.OutQuad));

            shadow_sequence.Append(m_rightShadow.DOColor(right_color, m_turnDuration * .5f).SetEase(Ease.InQuad));
            shadow_sequence.Append(m_leftShadow.DOColor(left_color, m_turnDuration * .5f).SetEase(Ease.OutQuad));

            m_turnSequence.Join(rotate_sequence);
            m_turnSequence.Join(shadow_sequence);
            m_turnSequence.OnComplete(() => {
                m_onEndTurning?.Invoke();
                m_turnSequence = null;
                gameObject.SetActive(false);
            });
        }

        private void SetupRect(bool right)
        {
            RectTransform rect = m_turningPage.GetComponent<RectTransform>();

            if (right)
            {
                rect.pivot = new Vector2(0f, .5f);
                // rect.anchorMin = new Vector2(.5f, 0f);
                // rect.anchorMax = new Vector2(1f, 1f);
            }
            else
            {
                rect.pivot = new Vector2(1f, .5f);
                // rect.anchorMin = new Vector2(0f, 0f);
                // rect.anchorMax = new Vector2(.5f, 1f);
            }
        }

        #region Utils

        public Canvas m_canvas;

        private void MatchImage(Image image, Vector2 min, Vector2 max)
        {
            RectTransform rect = image.GetComponent<RectTransform>();

            rect.sizeDelta = new Vector2(Mathf.Abs(max.x - min.x), Mathf.Abs(max.y - min.y));
        }

        public void MatchBounds(Camera rendering_camera, Vector3 min_screen, Vector3 max_screen)
        {
            if (rendering_camera == null)
                return;
            RectTransform canvasRect = m_canvas.GetComponent<RectTransform>();

            Vector2 min = min_screen;
            Vector2 max = max_screen;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, min_screen, rendering_camera, out min);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, max_screen, rendering_camera, out max);

            MatchImage(m_turningPage, min, max);
            MatchImage(m_rightShadow, min, max);
            MatchImage(m_leftShadow, min, max);
        }

        #endregion
    }
}