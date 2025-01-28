using CustomArchitecture;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

namespace Comic
{
    public class CreditView : AView
    {
        [SerializeField] protected RectTransform m_bubbleAnchor;
        [SerializeField] protected Transform m_bubbleContainer;
        [SerializeField] protected Transform m_mainIconContainer;
        
        private GameObject          m_bubble;
        private NpcIcon             m_mainIcon;

        private Dictionary<NpcIconType, Sprite> m_iconSprites;

        #if UNITY_EDITOR
        [SerializeField, OnValueChanged("DebugGraphic")] private bool m_activeGraphic = true;

        private void DebugGraphic()
        {
            ActiveGraphic(m_activeGraphic);
        }
        #endif

        public override void ActiveGraphic(bool active)
        {
            Image[] images = gameObject.GetComponentsInChildren<Image>(true);
            TMP_Text[] texts = gameObject.GetComponentsInChildren<TMP_Text>(true);

            foreach (Image image in images)
            {
                image.enabled = active;
            }

            foreach (TMP_Text text in texts)
            {
                text.enabled = active;
            }
        }

        public override void Init()
        {
            m_iconSprites = new()
            {
                { NpcIconType.Icon_Beloved, Resources.Load<Sprite>("GUI/Icon/Sprites/Face-Beloved-1") },
                { NpcIconType.Icon_BestFriend, Resources.Load<Sprite>("GUI/Icon/Sprites/Face-BestFriend-1") },
                { NpcIconType.Icon_Boss_1, Resources.Load<Sprite>("GUI/Icon/Sprites/Face-Boss-1") },
                { NpcIconType.Icon_Boss_2, Resources.Load<Sprite>("GUI/Icon/Sprites/Face-Boss-2") },
                { NpcIconType.Icon_Bully, Resources.Load<Sprite>("GUI/Icon/Sprites/Face-Bully-1") },
                { NpcIconType.Icon_Jacob_0, Resources.Load<Sprite>("GUI/Icon/Sprites/Face-Jacob-0") },
                { NpcIconType.Icon_Jacob_1, Resources.Load<Sprite>("GUI/Icon/Sprites/Face-Jacob-1") },
                { NpcIconType.Icon_Jacob_2, Resources.Load<Sprite>("GUI/Icon/Sprites/Face-Jacob-2") },
                { NpcIconType.Icon_Jacob_3, Resources.Load<Sprite>("GUI/Icon/Sprites/Face-Jacob-3") },
                { NpcIconType.Icon_Jacob_4, Resources.Load<Sprite>("GUI/Icon/Sprites/Face-Jacob-4") },
                { NpcIconType.Icon_Mom, Resources.Load<Sprite>("GUI/Icon/Sprites/Face-Mom") },
                { NpcIconType.Icon_The_Lost_Page, Resources.Load<Sprite>("GUI/Icon/Sprites/Face-LostPage") },
            };
            
            m_bubble = Instantiate(Resources.Load<GameObject>("GUI/Bubble/Bubble_Speech_Regular"), m_bubbleContainer);
            m_bubble.SetActive(false);

            m_bubble.GetComponent<Bubble>().Init(m_bubbleContainer.GetComponent<RectTransform>());

            InitMainIcon();
        }

        private void InitMainIcon()
        {
            GameObject main_icon = Instantiate(Resources.Load<GameObject>("GUI/Icon/IconFrame_Speaker"), m_mainIconContainer);
            m_mainIcon = main_icon.GetComponent<NpcIcon>();

            RectTransform container_rect = m_bubbleContainer.GetComponent<RectTransform>();
            m_mainIcon.Init(VoiceType.Voice_None, m_iconSprites[NpcIconType.Icon_Jacob_0]);

            m_mainIcon.SetBubbleAnchor(m_bubbleAnchor);

            m_mainIcon.gameObject.SetActive(false);

            m_bubble.GetComponent<Bubble>().SubscribeToAppearCallback(AppearIcon);
            m_bubble.GetComponent<Bubble>().SubscribeToDisappearCallback(DisappearIcon);
        }

        public void AppearIcon(float intensity)
        {
            m_mainIcon.gameObject.SetActive(true);
            m_mainIcon.Appear(intensity);
        }

        public void DisappearIcon(float intensity)
        {
            m_mainIcon.Disappear(intensity);
        }

        public override void Pause(bool pause)
        {
            // m_mainIcon.Pause(pause);
            // m_bubble.GetComponent<Bubble>().Pause(pause);
        }

        private IEnumerator SetupAndStartDialogue(PartOfDialogueConfig config, bool target_main)
        {
            m_bubble.GetComponent<Bubble>().SetupDialogue(config.m_associatedDialogue);

            m_bubble.GetComponent<Bubble>().SetTarget(m_mainIcon);
 
            yield return StartCoroutine(m_bubble.GetComponent<Bubble>().DialogueCoroutine(config.m_intensity, false, target_main));
        }

        public IEnumerator TriggerMainDialogue(PartOfDialogueConfig config)
        {
            m_mainIcon.SetIconSprite(m_iconSprites[config.m_iconType]);

            m_bubble.SetActive(true);

            yield return StartCoroutine(SetupAndStartDialogue(config, true));
        }
    }
}
