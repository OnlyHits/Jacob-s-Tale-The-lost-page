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
    public class BubbleChoice : Bubble
    {
        [SerializeField] private TMP_AnimatedText m_choiceOneDialogue;
        [SerializeField] private TMP_AnimatedText m_choiceTwoDialogue;
        [SerializeField] private GameObject m_cursorOne;
        [SerializeField] private GameObject m_cursorTwo;
        protected override bool IsBubbleChoice() => true;

        private bool                        m_accept = true;
        private bool                        m_validate = false;

        public override void Init(RectTransform container_rect)
        {
            base.Init(container_rect);

            ComicGameCore.Instance.MainGameMode.GetNavigationInput().SubscribeToCancel(OnCancel);
            ComicGameCore.Instance.MainGameMode.GetNavigationInput().SubscribeToValidate(OnValidation);
            ComicGameCore.Instance.MainGameMode.GetNavigationInput().SubscribeToNavigate(OnNavigate);

            m_cursorOne.SetActive(true);
            m_cursorTwo.SetActive(false);
        }

        public void SetupChoiceOne(DialogueType type)
        {
            DialogueConfig config = TMP_AnimatedTextController.Instance.GetDialogueConfig<ComicGameCore>(type);
            DynamicDialogueData datas = TMP_AnimatedTextController.Instance.GetDialogueDatas<ComicGameCore>(type);

            m_choiceOneDialogue.StartDialogue(config, datas);
            
            m_cursorOne.SetActive(true);
            m_cursorTwo.SetActive(false);

            m_validate = false;
            m_accept = true;
        }

        public void SetupChoiceTwo(DialogueType type)
        {
            DialogueConfig config = TMP_AnimatedTextController.Instance.GetDialogueConfig<ComicGameCore>(type);
            DynamicDialogueData datas = TMP_AnimatedTextController.Instance.GetDialogueDatas<ComicGameCore>(type);

            m_choiceTwoDialogue.StartDialogue(config, datas);
            m_accept = true;
        }

        protected override IEnumerator WaitForInput()
        {
            yield return new WaitUntil(() => m_validate);
        }

        private void OnNavigate(InputType input, Vector2 v)
        {
            if (input == InputType.PRESSED)
            {
                m_accept = (v.x < 0f);
                m_cursorOne.SetActive(m_accept);
                m_cursorTwo.SetActive(!m_accept);
            }
            else if (input == InputType.COMPUTED)
            {
                m_accept = (v.x < 0f);
                m_cursorOne.SetActive(m_accept);
                m_cursorTwo.SetActive(!m_accept);
            }
            else if (input == InputType.RELEASED)
            {
            }
        }

        private void OnValidation(InputType input, bool b)
        {
            if (input == InputType.PRESSED)
            {
                m_validate = true;
            }
            else if (input == InputType.COMPUTED)
            {
                m_validate = true;
            }
            else if (input == InputType.RELEASED)
            {
                m_validate = false;
            }
        }

        private void OnCancel(InputType input, bool b)
        {
            if (input == InputType.PRESSED)
            {
            }
            else if (input == InputType.COMPUTED)
            {
            }
            else if (input == InputType.RELEASED)
            {
            }
        }
    }
}