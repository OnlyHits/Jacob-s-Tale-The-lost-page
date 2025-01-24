using CustomArchitecture;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

namespace Comic
{
    public enum NpcIconType
    {
        Icon_Beloved,
        Icon_BestFriend,
        Icon_Boss_1,
        Icon_Boss_2,
        Icon_Bully,
        Icon_Jacob_0,
        Icon_Jacob_1,
        Icon_Jacob_2,
        Icon_Jacob_3,
        Icon_Jacob_4,
    }

    public class DialogueView : AView
    {
        private List<VoiceType> m_allowedVoices = new() { VoiceType.Voice_Beloved, VoiceType.Voice_BestFriend, VoiceType.Voice_Boss, VoiceType.Voice_Bully };

        [SerializeField] protected Transform m_bubbleContainer;
        [SerializeField] protected Transform m_iconContainer;
        [SerializeField] protected Transform m_mainIconContainer;

        private Dictionary<VoiceType, NpcIcon> m_icons;
        private Dictionary<VoiceType, Bubble> m_bubbles;

        private NpcIcon m_mainIcon;
        private Bubble m_mainBubble;

        [SerializeField] protected Canvas m_canvas;
        private Dictionary<NpcIconType, Sprite> m_iconSprites;

        public override void Init()
        {
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToUnlockVoice(UnlockVoice);
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToLockVoice(LockVoice);

            m_icons = new();
            m_bubbles = new();
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

            foreach (var data in ComicGameCore.Instance.GetGameMode<MainGameMode>().GetUnlockChaptersData())
            {
                if (data.m_hasUnlockVoice)
                {
                    UnlockVoice(ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig().GetVoiceByChapter(data.m_chapterType));
                }
            }

            InitMainIcon();
        }

        private void Start()
        {
            StartCoroutine(CoroutineUtils.InvokeOnDelay(1f, StartDialogue));
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

            // m_mainIcon.gameObject.SetActive(false);
            m_mainBubble.gameObject.SetActive(false);
        }

        public void LockVoice(VoiceType type)
        {
            if (!m_icons.ContainsKey(type))
            {
                Debug.LogWarning(type + " is not unlocked");
            }
            else
            {
                Destroy(m_icons[type].gameObject);
                m_icons.Remove(type);
            }

            if (!m_bubbles.ContainsKey(type))
            {
                Debug.LogWarning(type + " is not unlocked");
            }
            else
            {
                Destroy(m_bubbles[type].gameObject);
                m_bubbles.Remove(type);
            }
        }

        public void UnlockVoice(VoiceType type)
        {
            if (!IsVoiceAllow(type))
            {
                Debug.LogWarning(type + " is not register");
                return;
            }
            if ( m_icons.ContainsKey(type))
            {
                Debug.LogWarning(type + " is already register");
                return;
            }

            GameObject icon = Instantiate(Resources.Load<GameObject>("GUI/Icon/IconFrame_Voice"), m_iconContainer);
            GameObject bubble = Instantiate(Resources.Load<GameObject>("GUI/IconBubble_Voice"), m_bubbleContainer);
 
            bubble.SetActive(false);
 
            m_icons.Add(type, icon.GetComponent<NpcIcon>());
            m_bubbles.Add(type, bubble.GetComponent<Bubble>());

            RectTransform container_rect = m_bubbleContainer.GetComponent<RectTransform>();
            m_icons[type].Init(type, GetSpriteByType(type));
            m_bubbles[type].Init(m_icons[type], container_rect, m_canvas);
        }

        private void InstantiateBubble(VoiceType type)
        {

        }

        protected override void OnUpdate(float elapsed_time)
        {}

        public void StartDialogue()
        {
            StartCoroutine(TriggerDialogue());
        }

        private IEnumerator TriggerDialogue()
        {
            if (m_bubbles.TryGetValue(VoiceType.Voice_Beloved, out Bubble obj))
            {
                obj.gameObject.SetActive(true);

                Bubble bubble = m_bubbles[VoiceType.Voice_Beloved];

                bubble.Appear(BubbleAppearIntensity.Intensity_Normal);

                // while (bubble.IsCompute())
                //     yield return null;

                yield return StartCoroutine(bubble.TriggerAndWaitDialogue(DialogueType.Bethany_Welcome));
            }
        }

        private bool IsVoiceAllow(VoiceType type)
        {
            foreach (var allowed_icon in m_allowedVoices)
            {
                if (type == allowed_icon)
                return true;
            }

            return false;
        }

        private Sprite GetSpriteByType(VoiceType type)
        {
            if (type == VoiceType.Voice_Beloved)
                return m_iconSprites[NpcIconType.Icon_Beloved];
            else if (type == VoiceType.Voice_BestFriend)
                return m_iconSprites[NpcIconType.Icon_BestFriend];
            else if (type == VoiceType.Voice_Boss)
                return m_iconSprites[NpcIconType.Icon_Boss_2];
            else if (type == VoiceType.Voice_Bully)
                return m_iconSprites[NpcIconType.Icon_Bully];
            else if (type == VoiceType.Voice_None)
                return m_iconSprites[NpcIconType.Icon_Jacob_0];
            else
                return null;
        }
    }
}
