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
    }

    public class BubbleGraphicData
    {
        public GameObject m_regularBubble;
        public GameObject m_choiceBubble;
    }

    public class DialogueView : AView
    {
        [SerializeField] protected RectTransform m_bubbleAnchor;
        [SerializeField] protected Transform m_bubbleContainer;
        [SerializeField] protected Transform m_iconContainer;
        [SerializeField] protected Transform m_mainIconContainer;
        
        private Dictionary<DialogueBubbleType, BubbleGraphicData> m_bubbles;

        private Dictionary<VoiceType, VoiceViewDatas> m_datas;

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
                { NpcIconType.Icon_Mom, Resources.Load<Sprite>("GUI/Icon/Sprites/Face-Mom") },
                { NpcIconType.Icon_The_Lost_Page, Resources.Load<Sprite>("GUI/Icon/Sprites/Face-LostPage") },
            };

            m_bubbles = new()
            {
                { DialogueBubbleType.BubbleType_Speech, new BubbleGraphicData() },
                { DialogueBubbleType.BubbleType_Exclamation, new BubbleGraphicData() },
                { DialogueBubbleType.BubbleType_Thinking, new BubbleGraphicData() },
            };
            
            m_bubbles[DialogueBubbleType.BubbleType_Speech].m_regularBubble = Instantiate(Resources.Load<GameObject>("GUI/Bubble/Bubble_Speech_Regular"), m_bubbleContainer);
            m_bubbles[DialogueBubbleType.BubbleType_Speech].m_choiceBubble = Instantiate(Resources.Load<GameObject>("GUI/Bubble/Bubble_Speech_Choice"), m_bubbleContainer);

            m_bubbles[DialogueBubbleType.BubbleType_Exclamation].m_regularBubble = Instantiate(Resources.Load<GameObject>("GUI/Bubble/Bubble_Exclamation_Regular"), m_bubbleContainer);
            m_bubbles[DialogueBubbleType.BubbleType_Exclamation].m_choiceBubble = Instantiate(Resources.Load<GameObject>("GUI/Bubble/Bubble_Exclamation_Choice"), m_bubbleContainer);

            m_bubbles[DialogueBubbleType.BubbleType_Thinking].m_regularBubble = Instantiate(Resources.Load<GameObject>("GUI/Bubble/Bubble_Thinking_Regular"), m_bubbleContainer);
            m_bubbles[DialogueBubbleType.BubbleType_Thinking].m_choiceBubble = Instantiate(Resources.Load<GameObject>("GUI/Bubble/Bubble_Thinking_Choice"), m_bubbleContainer);

            foreach (var d in m_bubbles)
            {
                d.Value.m_regularBubble.SetActive(false);
                d.Value.m_regularBubble.GetComponent<Bubble>().Init(m_bubbleContainer.GetComponent<RectTransform>());
                d.Value.m_choiceBubble.SetActive(false);
                d.Value.m_choiceBubble.GetComponent<Bubble>().Init(m_bubbleContainer.GetComponent<RectTransform>());
            }

            InitMainIcon();

            foreach (var data in ComicGameCore.Instance.MainGameMode.GetUnlockChaptersData())
            {
                if (data.m_hasUnlockVoice)
                {
                    AddVoice(ComicGameCore.Instance.MainGameMode.GetGameConfig().GetVoiceByChapter(data.m_chapterType));
                }
            }
        }

        private void InitMainIcon()
        {
            GameObject main_icon = Instantiate(Resources.Load<GameObject>("GUI/Icon/IconFrame_Speaker"), m_mainIconContainer);
            m_mainIcon = main_icon.GetComponent<NpcIcon>();

            RectTransform container_rect = m_bubbleContainer.GetComponent<RectTransform>();
            m_mainIcon.Init(VoiceType.Voice_None, m_iconSprites[NpcIconType.Icon_Jacob_0]);

            m_mainIcon.SetBubbleAnchor(m_bubbleAnchor);

            m_mainIcon.gameObject.SetActive(false);

            foreach (var d in m_bubbles)
            {
                d.Value.m_choiceBubble.GetComponent<Bubble>().SubscribeToAppearCallback(AppearIcon);
                d.Value.m_choiceBubble.GetComponent<Bubble>().SubscribeToDisappearCallback(DisappearIcon);

                d.Value.m_regularBubble.GetComponent<Bubble>().SubscribeToAppearCallback(AppearIcon);
                d.Value.m_regularBubble.GetComponent<Bubble>().SubscribeToDisappearCallback(DisappearIcon);
            }
        }

        public override void Pause(bool pause)
        {
            foreach (var d in m_datas)
            {
                d.Value.m_icon.Pause(pause);
            }
            
            foreach (var b in m_bubbles)
            {
                b.Value.m_regularBubble.GetComponent<Bubble>().Pause(pause);
                b.Value.m_choiceBubble.GetComponent<Bubble>().Pause(pause);
            }


        }

        public void Highlight(VoiceType type)
        {
            foreach (var data in m_datas)
            {
                if (data.Value.m_icon.GetVoiceType() == type)
                {
                    data.Value.m_icon.Highlight(true);
                }
                else
                {
                    data.Value.m_icon.Highlight(false);
                }
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
  
            m_datas.Add(type, new VoiceViewDatas());
            m_datas[type].m_icon = icon.GetComponent<NpcIcon>();

            RectTransform container_rect = m_bubbleContainer.GetComponent<RectTransform>();

            if (type == VoiceType.Voice_BestFriend)
                m_datas[type].m_icon.Init(type, m_iconSprites[NpcIconType.Icon_BestFriend]);
            else if (type == VoiceType.Voice_Beloved)
                m_datas[type].m_icon.Init(type, m_iconSprites[NpcIconType.Icon_Beloved]);
            else if (type == VoiceType.Voice_Bully)
                m_datas[type].m_icon.Init(type, m_iconSprites[NpcIconType.Icon_Bully]);
            else if (type == VoiceType.Voice_Boss)
                m_datas[type].m_icon.Init(type, m_iconSprites[NpcIconType.Icon_Boss_1]);
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

        private Bubble GetBubbleByType(DialogueBubbleType type, bool choice)
        {
            return (choice ? m_bubbles[type].m_choiceBubble.GetComponent<Bubble>() : m_bubbles[type].m_regularBubble.GetComponent<Bubble>());
        }

        private IEnumerator SetupAndStartDialogue(PartOfDialogueConfig config, bool target_main)
        {
            if (config.IsMultipleChoice())
            {
                GetBubbleByType(config.m_bubbleType, true).SetupDialogue(config.m_associatedDialogue);
                ((BubbleChoice)GetBubbleByType(config.m_bubbleType, true)).SetupChoiceOne(((PartOfDialogueChoiceConfig)config).m_choiceOneDialogue);
                ((BubbleChoice)GetBubbleByType(config.m_bubbleType, true)).SetupChoiceTwo(((PartOfDialogueChoiceConfig)config).m_choiceTwoDialogue);
        
                if (target_main)
                    GetBubbleByType(config.m_bubbleType, true).SetTarget(m_mainIcon);
                else
                    GetBubbleByType(config.m_bubbleType, true).SetTarget(m_datas[config.m_speaker].m_icon);
        
                yield return StartCoroutine(GetBubbleByType(config.m_bubbleType, true).DialogueCoroutine(config.m_intensity, true, target_main));
            }
            else
            {
                GetBubbleByType(config.m_bubbleType, false).SetupDialogue(config.m_associatedDialogue);

                if (target_main)
                    GetBubbleByType(config.m_bubbleType, false).SetTarget(m_mainIcon);
                else
                    GetBubbleByType(config.m_bubbleType, false).SetTarget(m_datas[config.m_speaker].m_icon);

                yield return StartCoroutine(GetBubbleByType(config.m_bubbleType, false).DialogueCoroutine(config.m_intensity, false, target_main));
            }
        }

        public IEnumerator TriggerMainDialogue(PartOfDialogueConfig config)
        {
            m_mainIcon.SetIconSprite(m_iconSprites[config.m_iconType]);

            if (config.IsMultipleChoice())
                GetBubbleByType(config.m_bubbleType, true).gameObject.SetActive(true);
            else
                GetBubbleByType(config.m_bubbleType, false).gameObject.SetActive(true);

            yield return StartCoroutine(SetupAndStartDialogue(config, true));
        }

        public IEnumerator TriggerVoiceDialogue(PartOfDialogueConfig config)
        {
            if (!m_datas.ContainsKey(config.m_speaker))
            {
                Debug.LogWarning("You try to trigger uninitialize dialogue");
            }
            else
            {
                if (config.IsMultipleChoice())
                    GetBubbleByType(config.m_bubbleType, true).gameObject.SetActive(true);
                else
                    GetBubbleByType(config.m_bubbleType, false).gameObject.SetActive(true);

                m_datas[config.m_speaker].m_icon.SetIconSprite(m_iconSprites[config.m_iconType]);
    
                yield return StartCoroutine(SetupAndStartDialogue(config, false));
            }
        }
    }
}
