using System;
using Sirenix.Serialization;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using CustomArchitecture;
using System.Collections;
using ExtensionMethods;

namespace Comic
{
    public class DialogueManager : BaseBehaviour
    {
        [SerializeField] private DialogueView m_dialogueView;
        [SerializeField] private CreditView m_creditView;
        
        private JacobDialogueConfig m_dialogueConfig;
        private Coroutine m_dialogueCoroutine;
        private Action<PowerType> m_changePowerCallback;
        private Action<DialogueName> m_onEndDialogueCallback;
        private Dictionary<DialogueName, bool> m_mainStory;
        private int m_powerIndex = 0;

        public void SubscribeToEndDialogue(Action<DialogueName> function)
        {
            m_onEndDialogueCallback -= function;
            m_onEndDialogueCallback += function;
        }

        public void Init(DialogueView dialogue_view, CreditView credit_view)
        {
            m_dialogueView = dialogue_view;
            m_creditView = credit_view;

            ComicGameCore.Instance.MainGameMode.SubscribeToUnlockVoice(UnlockVoice);
            ComicGameCore.Instance.MainGameMode.SubscribeToLockVoice(LockVoice);

            ComicGameCore.Instance.MainGameMode.SubscribeToNextPower(() => OnSwitchPower(true));
            ComicGameCore.Instance.MainGameMode.SubscribeToPrevPower(() => OnSwitchPower(false));

            m_dialogueConfig = SerializedScriptableObject.CreateInstance<JacobDialogueConfig>();
            m_dialogueCoroutine = null;

            // must save this shit
            m_mainStory = new()
            {
                { DialogueName.Dialogue_Welcome, false},
                { DialogueName.Dialogue_ChangePage, false},
                { DialogueName.Dialogue_UnlockBF, false},
                { DialogueName.Dialogue_UnlockBully, false},
                { DialogueName.Dialogue_UnlockBL, false},
                { DialogueName.Dialogue_UnlockBoss, false},
            };
        }

        public void SubscribeToPowerSelected(Action<PowerType> function)
        {
            m_changePowerCallback -= function;
            m_changePowerCallback += function;
        }

        private void GetNextIndex(bool next)
        {
            var unlockChapters = ComicGameCore.Instance.MainGameMode.GetUnlockChaptersData();
            var gameConfig = ComicGameCore.Instance.MainGameMode.GetGameConfig();
            int chapterCount = unlockChapters.Count;
            int i = 0;
            do
            {
                if (i == chapterCount)
                    break;
                m_powerIndex = next ? (m_powerIndex + 1) % chapterCount : (m_powerIndex - 1 + chapterCount) % chapterCount;
                ++i;
            }
            while (gameConfig.GetPowerByChapter(unlockChapters[m_powerIndex].m_chapterType) == PowerType.Power_None);
        }

        public void OnSwitchPower(bool next)
        {
            GetNextIndex(next);

            ChapterSavedData data = ComicGameCore.Instance.MainGameMode.GetUnlockChaptersData()[m_powerIndex];

            if (m_dialogueView != null)
                m_dialogueView.Highlight(ComicGameCore.Instance.MainGameMode.GetGameConfig().GetVoiceByChapter(data.m_chapterType));

            m_changePowerCallback?.Invoke(ComicGameCore.Instance.MainGameMode.GetGameConfig().GetPowerByChapter(data.m_chapterType));
        }

        #region VoiceIcon

        private bool IsNpcAllowedToBeVoice(VoiceType type)
        {
            if (type == VoiceType.Voice_Beloved
                || type == VoiceType.Voice_Bully
                || type == VoiceType.Voice_Boss
                || type == VoiceType.Voice_BestFriend)
                return true;

            return false;
        }

        public void LockVoice(VoiceType type)
        {
            if (m_dialogueView != null)
                m_dialogueView.RemoveVoice(type);
        }

        public void UnlockVoice(VoiceType type)
        {
            if (!IsNpcAllowedToBeVoice(type))
            {
                Debug.LogWarning(type + " is not register");
                return;
            }

            if (m_dialogueView != null)
                m_dialogueView.AddVoice(type);
        }

        #endregion

        #region Dialogues

        public override void Pause(bool pause)
        {
            base.Pause(pause);
        }

        public bool StartDialogue(DialogueName type)
        {
            if (m_mainStory.ContainsKey(type) && m_mainStory[type])
            {
                Debug.LogWarning("Already trigger this dialogue");
                return false;
            }

            if (!m_dialogueConfig.GetConfig().ContainsKey(type))
            {
                Debug.LogWarning("Doesnt find dialogue");
                return false;
            }

            foreach (var t in m_dialogueConfig.GetConfig()[type])
            {
                // check that view has unlocked the icon & bubble if not main icon
                if (!t.m_isMainDialogue && !ProgressionUtils.HasUnlockVoice(t.m_speaker))
                {
                    Debug.LogWarning("You need to unlock " + t.m_speaker.ToString() + " before start " + type.ToString());
                    return false;
                }
            }

            if (m_mainStory.ContainsKey(type))
                m_mainStory[type] = true;

            m_dialogueCoroutine = StartCoroutine(DialogueCoroutine(type));

            return true;
        }

        public IEnumerator DialogueCoroutine(DialogueName type)
        {
            if (m_dialogueView == null)
            {
                m_onEndDialogueCallback?.Invoke(type);
                yield break;
            }

            foreach (var part in m_dialogueConfig.GetConfig()[type])
            {
                if (part.m_isMainDialogue)
                {
                    yield return StartCoroutine(m_dialogueView.TriggerMainDialogue(part));
                }
                else
                {
                    yield return StartCoroutine(m_dialogueView.TriggerVoiceDialogue(part));
                }

                yield return new WaitForSeconds(part.m_waitAfterDisappear);
            }

            m_onEndDialogueCallback?.Invoke(type);
        }

        public bool StartCreditDialogue()
        {
            m_dialogueCoroutine = StartCoroutine(CreditCoroutine(DialogueName.Dialogue_Credit));

            return true;
        }

        public IEnumerator CreditCoroutine(DialogueName type)
        {
            foreach (var part in m_dialogueConfig.GetConfig()[type])
            {
                yield return StartCoroutine(m_creditView.TriggerMainDialogue(part));

                yield return new WaitForSeconds(part.m_waitAfterDisappear);
            }
        }

        #endregion
    }
}
