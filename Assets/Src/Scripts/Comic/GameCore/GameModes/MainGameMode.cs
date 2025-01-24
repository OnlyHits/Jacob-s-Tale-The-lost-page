using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using CustomArchitecture;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Comic
{
    public interface MainGameModeProvider
    {
        public List<ChapterSavedData> GetUnlockChaptersData();
        public GameConfig GetGameConfig();
        public PageManager GetPageManager();
        public CharacterManager GetCharacterManager();
        public void UnlockVoice(VoiceType type, bool force_unlock);
        public void UnlockPower(PowerType type, bool force_unlock);
        public void UnlockChapter(Chapters type, bool unlock_voice, bool unlock_power);

        public void LockChapter(Chapters type);

        public void SubscribeToLockVoice(Action<VoiceType> function);
        public void SubscribeToLockPower(Action<PowerType> function);
        public void SubscribeToLockChapter(Action<Chapters> function);

        public void SubscribeToUnlockVoice(Action<VoiceType> function);
        public void SubscribeToUnlockPower(Action<PowerType> function);
        public void SubscribeToUnlockChapter(Action<Chapters> function);

        public void ClearSaveDebug();
    }

    public class MainGameMode : AGameMode, MainGameModeProvider
    {
        private GameConfig m_gameConfig;
        private GameProgression m_gameProgression;// regrouper avec settings dans une subclass
        private PauseInput m_pauseInput; // ca va etre integré direct dans gamemode
        private NavigationInput m_hudNavigationInput; // ca degage

        private ViewManager m_viewManager;
        private PageManager m_pageManager;
        private CharacterManager m_characterManager;

        private Action<VoiceType> m_onUnlockVoiceCallback;
        private Action<PowerType> m_onUnlockPowerCallback;
        private Action<Chapters> m_onUnlockChapterCallback;

        private Action<VoiceType> m_onLockVoiceCallback;
        private Action<PowerType> m_onLockPowerCallback;
        private Action<Chapters> m_onLockChapterCallback;

        public List<ChapterSavedData> GetUnlockChaptersData() => m_gameProgression.GetUnlockedChaptersDatas();
        public GameConfig GetGameConfig() => m_gameConfig;
        public PageManager GetPageManager() => m_pageManager;
        public CharacterManager GetCharacterManager() => m_characterManager;
        public override void OnLoadingEnded() { }

        public override void Init(AGameCore game_core, params object[] parameters)
        {
            base.Init(game_core, parameters);

            m_gameConfig = SerializedScriptableObject.CreateInstance<GameConfig>();
            m_gameProgression = new GameProgression();
            m_pauseInput = GetComponent<PauseInput>();
            m_hudNavigationInput = GetComponent<NavigationInput>();
            m_viewManager = GetComponent<ViewManager>();
            m_pageManager = GetComponent<PageManager>();
            m_characterManager = GetComponent<CharacterManager>();

            if (GetUnlockChaptersData().Count == 0)
            {
                UnlockChapter(Chapters.The_Prequel, false, false);
            }

            m_pauseInput.Init();
            m_hudNavigationInput.Init();
            m_viewManager.Init();
            m_pageManager.Init();
            m_characterManager.Init();

            ComicGameCore.Instance.GetSettings().m_settingDatas.m_language = Language.French;
        }

        #region Progression

        public void UnlockVoice(VoiceType type, bool force_unlock = false)
        {
            Chapters target_chapter = m_gameConfig.GetChapterByVoice(type);

            if (!m_gameProgression.HasUnlockChapter(target_chapter))
            {
                if (force_unlock)
                {
                    UnlockChapter(target_chapter, false, false);
                }
                else
                {
                    Debug.LogWarning("Couldn't unlock voice, unlock chapter first");
                    return;
                }
            }

            if (m_gameProgression.HasUnlockVoice(target_chapter))
            {
                Debug.LogWarning("You already unlock this voice");
                return;
            }

            m_gameProgression.UnlockVoice(target_chapter);
            m_onUnlockVoiceCallback?.Invoke(type);
        }

        public void UnlockPower(PowerType type, bool force_unlock = false)
        {
            Chapters target_chapter = m_gameConfig.GetChapterByPower(type);

            if (!m_gameProgression.HasUnlockChapter(target_chapter))
            {
                if (force_unlock)
                {
                    UnlockChapter(target_chapter, false, true);
                    return;
                }
                else
                {
                    Debug.LogWarning("Couldn't unlock voice, unlock chapter first");
                    return;
                }
            }

            if (m_gameProgression.HasUnlockPower(target_chapter))
            {
                Debug.LogWarning("You already unlock this power");
                return;
            }

            m_gameProgression.UnlockPower(target_chapter);
            m_onUnlockPowerCallback?.Invoke(type);
        }

        public void UnlockChapter(Chapters type, bool unlock_voice, bool unlock_power)
        {
            if (m_gameProgression.HasUnlockChapter(type))
            {
                Debug.LogWarning("You already unlock this chapter");
                return;
            }

            m_gameProgression.UnlockChapter(type);

            if (unlock_voice)
                UnlockVoice(m_gameConfig.GetChapterDatas(type).m_voiceType, false);

            if (unlock_power)
                UnlockPower(m_gameConfig.GetChapterDatas(type).m_powerType, false);

            m_onUnlockChapterCallback?.Invoke(type);
        }

        public void LockChapter(Chapters type)
        {
            if (!m_gameProgression.HasUnlockChapter(type))
            {
                Debug.LogWarning("Chapter already locked");
                return;
            }

            m_gameProgression.LockChapter(type);

            m_onLockChapterCallback?.Invoke(type);
            m_onLockVoiceCallback?.Invoke(m_gameConfig.GetVoiceByChapter(type));
            m_onLockPowerCallback?.Invoke(m_gameConfig.GetPowerByChapter(type));
        }

        #endregion

        public void ClearSaveDebug()
        {
            m_gameProgression.ClearSaveDebug();
        }

        #region Callbacks

        public void SubscribeToLockChapter(Action<Chapters> function)
        {
            m_onLockChapterCallback += function;
        }

        public void SubscribeToLockPower(Action<PowerType> function)
        {
            m_onLockPowerCallback += function;
        }

        public void SubscribeToLockVoice(Action<VoiceType> function)
        {
            m_onLockVoiceCallback += function;
        }

        public void SubscribeToUnlockChapter(Action<Chapters> function)
        {
            m_onUnlockChapterCallback += function;
        }

        public void SubscribeToUnlockPower(Action<PowerType> function)
        {
            m_onUnlockPowerCallback += function;
        }

        public void SubscribeToUnlockVoice(Action<VoiceType> function)
        {
            m_onUnlockVoiceCallback += function;
        }

        #endregion

        public override void Pause(bool pause)
        {
            if (pause)
            {
                Debug.Log("Pause game");

                m_hudNavigationInput.Pause(false);
                // pause player
            }
            else
            {
                m_hudNavigationInput.Pause(true);
                // resume player
            }
        }

        protected override void OnUpdate(float elapsed_time)
        {
            base.OnUpdate(elapsed_time);

            // if (m_pause)
            // {
            //     m_hudNavigationInput.Update();
            // }

        }

        // Instantiate gameObjects
        public override void StartGameMode()
        {
            Compute = true;
        }

        // destroy all managed objects
        public override void StopGameMode()
        {
            Compute = false;
        }

        // restart all managed gameObject or destroy & instantiate
        public override void RestartGameMode()
        {
            Compute = true;
        }
    }
}
