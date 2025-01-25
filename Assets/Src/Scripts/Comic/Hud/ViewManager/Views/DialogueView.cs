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
    public class VoiceViewDatas
    {
        public NpcIcon m_icon;
        public Bubble m_bubble;
    }

    public class DialogueView : AView
    {
        [SerializeField] protected Transform m_bubbleContainer;
        [SerializeField] protected Transform m_iconContainer;
        [SerializeField] protected Transform m_mainIconContainer;

        private Dictionary<VoiceType, VoiceViewDatas> m_datas;

        private NpcIcon m_mainIcon;
        private Bubble m_mainBubble;

        [SerializeField] protected Canvas m_canvas;
        private Dictionary<NpcIconType, Sprite> m_iconSprites;

        #if UNITY_EDITOR
        [SerializeField, OnValueChanged("DebugGraphic")] private bool m_activeGraphic = true;

        [SerializeField] private TMP_AnimatedText test;

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
            m_datas = new();
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
            };

            InitMainIcon();
        }

        private void InitMainIcon()
        {
            GameObject main_icon = Instantiate(Resources.Load<GameObject>("GUI/Icon/IconFrame_Speaker"), m_mainIconContainer);
            m_mainIcon = main_icon.GetComponent<NpcIcon>();
            GameObject main_bubble = Instantiate(Resources.Load<GameObject>("GUI/IconBubble_Speaker"), m_bubbleContainer);
            m_mainBubble = main_bubble.GetComponent<Bubble>();

            RectTransform container_rect = m_bubbleContainer.GetComponent<RectTransform>();
            m_mainIcon.Init(VoiceType.Voice_None, m_iconSprites[ NpcIconType.Icon_Jacob_0]);
            m_mainBubble.Init(m_mainIcon, container_rect, m_canvas);

            m_mainIcon.gameObject.SetActive(false);
            m_mainBubble.gameObject.SetActive(false);
        }

        public override void Pause(bool pause)
        {
            foreach (var d in m_datas)
            {
                d.Value.m_icon.Pause(pause);
                d.Value.m_bubble.Pause(pause);
            }
        }

        public void RemoveVoice(VoiceType type)
        {
            if (!m_datas.ContainsKey(type))
            {
                Debug.LogWarning(type + " is not unlocked");
            }
            else
            {
                if (m_datas[type].m_icon != null)
                    Destroy(m_datas[type].m_icon.gameObject);
                if (m_datas[type].m_bubble != null)
                    Destroy(m_datas[type].m_bubble.gameObject);
                
                m_datas.Remove(type);
            }
        }

        public void AddVoice(VoiceType type)
        {
            if (m_datas.ContainsKey(type))
            {
                Debug.LogWarning(type + " is already register");
                return;
            }

            GameObject icon = Instantiate(Resources.Load<GameObject>("GUI/Icon/IconFrame_Voice"), m_iconContainer);
            GameObject bubble = Instantiate(Resources.Load<GameObject>("GUI/IconBubble_Voice"), m_bubbleContainer);
 
            bubble.SetActive(false);
 
            m_datas.Add(type, new VoiceViewDatas());
            m_datas[type].m_icon = icon.GetComponent<NpcIcon>();
            m_datas[type].m_bubble = bubble.GetComponent<Bubble>();

            RectTransform container_rect = m_bubbleContainer.GetComponent<RectTransform>();
            m_datas[type].m_icon.Init(type, null);//GetSpriteByType(type));
            m_datas[type].m_bubble.Init(m_datas[type].m_icon, container_rect, m_canvas);
        }

        public IEnumerator TriggerVoiceDialogue(PartOfDialogueConfig config)
        {
            if (!m_datas.ContainsKey(config.m_speaker))
            {
                Debug.LogWarning("You try to trigger uninitialize dialogue");
            }
            else
            {
                m_datas[config.m_speaker].m_icon.SetIconSprite(m_iconSprites[config.m_iconType]);

                m_datas[config.m_speaker].m_bubble.SetupDialogue(config.m_associatedDialogue);

                yield return StartCoroutine(m_datas[config.m_speaker].m_bubble.DialogueCoroutine(config.m_intensity));
            }
        }
    }
}
