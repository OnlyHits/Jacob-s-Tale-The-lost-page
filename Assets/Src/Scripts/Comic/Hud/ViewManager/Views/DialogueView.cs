using CustomArchitecture;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

namespace Comic
{
    public static class RectTransformUtils
    {
        public static Vector3 GetPivotInWorldSpace(this RectTransform source)
        {
            // Rewrite Rect.NormalizedToPoint without any clamping.
            Vector2 pivot = new Vector2(
                source.rect.xMin + source.pivot.x * source.rect.width,
                source.rect.yMin + source.pivot.y * source.rect.height);
            // Apply scaling and rotations.
            return source.TransformPoint(new Vector3(pivot.x, pivot.y, 0f));
        }

        // Set the RectTransform's pivot point in world coordinates, without moving the position.
        // This is like dragging the pivot handle in the editor.
        //
        public static void SetPivotInWorldSpace(this RectTransform source, Vector3 pivot)
        {
            // Strip scaling and rotations.
            pivot = source.InverseTransformPoint(pivot);
            Vector2 pivot2 = new Vector2(
                (pivot.x - source.rect.xMin) / source.rect.width,
                (pivot.y - source.rect.yMin) / source.rect.height);

            // Now move the pivot, keeping and restoring the position which is based on it.
            Vector2 offset = pivot2 - source.pivot;
            offset.Scale(source.rect.size);
            Vector3 worldPos = source.position + source.TransformVector(offset);
            source.pivot = pivot2;
            source.position = worldPos;
        }
    }

    public class DialogueView : AView
    {
        [SerializeField] protected Transform m_iconContainer;
        private Dictionary<VoiceType, GameObject> m_icons;
        private Dictionary<VoiceType, GameObject> m_currentIcons;

        [SerializeField] protected Transform m_bubbleContainer;
        private Dictionary<VoiceType, GameObject> m_currentBubbles;
        private Dictionary<VoiceType, Vector3> m_iconLastPositions;

        public override void Init()
        {
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToUnlockVoice(UnlockVoice);

            m_currentIcons = new();
            m_currentBubbles = new();
            m_iconLastPositions = new();
            m_icons = new()
            {
                { VoiceType.Voice_BestFriend,   Resources.Load<GameObject>("GUI/Icon_Gaetan") },
                { VoiceType.Voice_Beloved,      Resources.Load<GameObject>("GUI/Icon_Bethany") },
                { VoiceType.Voice_Bully,        Resources.Load<GameObject>("GUI/Icon_Dylan") },
                { VoiceType.Voice_Boss,         Resources.Load<GameObject>("GUI/Icon_Ivyc") },
            };

            foreach (var data in ComicGameCore.Instance.GetGameMode<MainGameMode>().GetSavedValues())
            {
                if (data.m_hasUnlockVoice)
                {
                    UnlockVoice(ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig().GetVoiceByChapter(data.m_chapterType));
                }
            }
        }

        private void Start()
        {
            StartCoroutine(CoroutineUtils.InvokeOnDelay(1f, StartDialogue));
        }

        public void UnlockVoice(VoiceType type)
        {
            if (!m_icons.ContainsKey(type) || m_currentIcons.ContainsKey(type))
            {
                Debug.LogWarning(type + " is not register");
                return;
            }

            GameObject icon = Instantiate(m_icons[type], m_iconContainer);
            icon.GetComponent<VoiceIcon>().Init(type);

            m_currentIcons.Add(type, icon);

            m_iconLastPositions.Add(type, icon.GetComponent<RectTransform>().position);

            InstantiateBubble(type);
        }

        private void InstantiateBubble(VoiceType type)
        {
            GameObject bubble = Instantiate(Resources.Load<GameObject>("GUI/Bubble_Dialogue"), m_bubbleContainer);
            bubble.SetActive(false);

            m_currentBubbles.Add(type, bubble);
        }

        private void SetupBubble(VoiceType type)
        {

            RectTransform icon_rect = m_currentIcons[type].GetComponent<RectTransform>();
            RectTransform container_rect = m_bubbleContainer.GetComponent<RectTransform>();
            RectTransform bubble_rect = m_currentBubbles[type].GetComponent<RectTransform>();

            bubble_rect.pivot = new Vector2(.5f, .5f);

            Vector3 offset_x = new Vector3(icon_rect.rect.width + bubble_rect.rect.width, 0, 0);
            Vector3 offset_y = new Vector3(0, container_rect.rect.height - bubble_rect.rect.height, 0);
            Vector3 top_position = container_rect.position + container_rect.TransformPoint(offset_y * .5f);
            Vector3 bot_position = container_rect.position - container_rect.TransformPoint(offset_y * .5f);

            float y_position = Mathf.Clamp(icon_rect.position.y, bot_position.y, top_position.y);

            bubble_rect.position = new Vector3(0, y_position, bubble_rect.position.z);

            bubble_rect.SetPivotInWorldSpace(icon_rect.position);
        }

        protected override void OnUpdate(float elapsed_time)
        {
            foreach (var icon in m_currentIcons)
            {
                if (icon.Value.GetComponent<RectTransform>().position
                   != m_iconLastPositions[icon.Key])
                {
                    SetupBubble(icon.Key);
                    m_iconLastPositions[icon.Key] = icon.Value.GetComponent<RectTransform>().position;
                }
            }
        }

        public void StartDialogue()
        {
            //StartCoroutine(TriggerDialogue());
        }

        private IEnumerator TriggerDialogue()
        {
            m_currentBubbles[VoiceType.Voice_Beloved].gameObject.SetActive(true);

            Bubble bubble = m_currentBubbles[VoiceType.Voice_Beloved].GetComponent<Bubble>();

            bubble.Appear(BubbleAppearIntensity.Intensity_Normal);

            while (bubble.IsCompute())
                yield return null;

            yield return StartCoroutine(bubble.TriggerAndWaitDialogue("Welcome"));
        }
    }
}
