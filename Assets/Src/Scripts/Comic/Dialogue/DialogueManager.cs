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

        public void Init()
        {
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToUnlockVoice(UnlockVoice);
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToLockVoice(LockVoice);

            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToNextPower(() => OnSwitchPower(true));
            ComicGameCore.Instance.GetGameMode<MainGameMode>().SubscribeToPrevPower(() => OnSwitchPower(false));

            m_dialogueConfig = SerializedScriptableObject.CreateInstance<JacobDialogueConfig>();
            m_dialogueCoroutine = null;

            m_mainStory = new()
            {
                { DialogueName.Dialogue_Welcome, false},
                { DialogueName.Dialogue_ChangePage, false},
                { DialogueName.Dialogue_UnlockBF, false},
                { DialogueName.Dialogue_UnlockBully, false},
                { DialogueName.Dialogue_UnlockBL, false},
            };

            InitDialogueView();
        }

        public void SubscribeToPowerSelected(Action<PowerType> function)
        {
            m_changePowerCallback -= function;
            m_changePowerCallback += function;
        }

        private void GetNextIndex(bool next)
        {
            var unlockChapters = ComicGameCore.Instance.GetGameMode<MainGameMode>().GetUnlockChaptersData();
            var gameConfig = ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig();
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

            ChapterSavedData data = ComicGameCore.Instance.GetGameMode<MainGameMode>().GetUnlockChaptersData()[m_powerIndex];

            m_dialogueView.Highlight(ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig().GetVoiceByChapter(data.m_chapterType));

            m_changePowerCallback?.Invoke(ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig().GetPowerByChapter(data.m_chapterType));
        }

        #region VoiceIcon

        private void InitDialogueView()
        {
            foreach (var data in ComicGameCore.Instance.GetGameMode<MainGameMode>().GetUnlockChaptersData())
            {
                if (data.m_hasUnlockVoice)
                {
                    m_dialogueView.AddVoice(ComicGameCore.Instance.GetGameMode<MainGameMode>().GetGameConfig().GetVoiceByChapter(data.m_chapterType));
                }
            }
        }

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
            m_dialogueView.RemoveVoice(type);
        }

        public void UnlockVoice(VoiceType type)
        {
            if (!IsNpcAllowedToBeVoice(type))
            {
                Debug.LogWarning(type + " is not register");
                return;
            }

            m_dialogueView.AddVoice(type);
        }

        #endregion

        #region Dialogues

        public override void Pause(bool pause)
        {
            base.Pause(pause);

            Debug.Log("DialogueManager is paused : " + pause.ToString());
        }

        private void Start()
        {
        }

        public void StartDialogue(DialogueName type)
        {
            if (m_mainStory.ContainsKey(type) && m_mainStory[type])
            {
                Debug.LogWarning("Already trigger this dialogue");
                return;
            }

            if (!m_dialogueConfig.GetConfig().ContainsKey(type))
            {
                Debug.LogWarning("Doesnt find dialogue");
                return;
            }

            foreach (var t in m_dialogueConfig.GetConfig()[type])
            {
                // check that view has unlocked the icon & bubble if not main icon
                if (!t.m_isMainDialogue && !ProgressionUtils.HasUnlockVoice(t.m_speaker))
                {
                    Debug.LogWarning("You need to unlock " + t.m_speaker.ToString() + " before starting this dialogue");
                    return;
                }
            }
            
            if (m_mainStory.ContainsKey(type))
                m_mainStory[type] = true;
 
            m_dialogueCoroutine = StartCoroutine(DialogueCoroutine(type));
        }

        public IEnumerator DialogueCoroutine(DialogueName type)
        {
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

        #endregion
    }
}
