using System;
using Sirenix.OdinInspector;
using CustomArchitecture;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

namespace Comic
{
    public interface MainGameModeProvider
    {
        // public Page GetCurrentPage();
        // public Case GetCurrentPanel();

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

        public void SubscribeToPowerSelected(Action<PowerType> function);
        public void SubscribeToNextPower(Action function);
        public void SubscribeToPrevPower(Action function);

        public void SubscribeToBeforeSwitchPage(Action<bool, Page, Page> function);
        public void SubscribeToMiddleSwitchPage(Action<bool, Page, Page> function);
        public void SubscribeToAfterSwitchPage(Action<bool, Page, Page> function);
        public void SubscribeToAfterCloneCanvasCallback(Action<bool> function);

        public void TriggerDialogue(DialogueName type);

        public void ClearSaveDebug();
    }

    public class MainGameMode : AGameMode<ComicGameCore>, MainGameModeProvider
    {
        // globals datas
        private GameConfig m_gameConfig;
        private GameProgression m_gameProgression;
        private SceneLoader m_sceneLoader;
        private URP_CameraManager m_cameraManager;
        private NavigationInput m_navigationInput;

        // local datas
        private HudManager m_hudManager;
        private GameManager m_gameManager;

        private Action<VoiceType> m_onUnlockVoiceCallback;
        private Action<PowerType> m_onUnlockPowerCallback;
        private Action<Chapters> m_onUnlockChapterCallback;

        private Action<VoiceType> m_onLockVoiceCallback;
        private Action<PowerType> m_onLockPowerCallback;
        private Action<Chapters> m_onLockChapterCallback;

        private Action m_onEndGame;

        // ---- really necessary? ----

        public List<ChapterSavedData> GetUnlockChaptersData() => m_gameProgression.GetUnlockedChaptersDatas();

        // ---- MainGameCore dependencies ----

        public InputActionAsset GetInputAsset() => m_gameCore.GetInputAsset();
        public NavigationInput GetNavigationInput() => m_navigationInput;
        public GameConfig GetGameConfig() => m_gameConfig;
        public URP_CameraManager GetCameraManager() => m_cameraManager;

        // ---- Sub managers ----

        public PageManager GetPageManager() => m_gameManager?.GetPageManager();
        public CharacterManager GetCharacterManager() => m_gameManager?.GetCharacterManager();
        public PowerManager GetPowerManager() => m_gameManager?.GetPowerManager();
        public DialogueManager GetDialogueManager() => m_gameManager?.GetDialogueManager();
        public ViewManager GetViewManager() => m_hudManager?.GetViewManager();


        // This function is called right after Init()
        public override void StartGameMode()
        {
            // // should be done in stop game mode but w/e
            // m_cameraManager.ClearCameraStack();

            m_sceneLoader.LoadGameModeScenes("HudScene", "GameScene");
            Compute = true;
        }

        // This function is called first
        public override void Init(ComicGameCore game_core, params object[] parameters)
        {
            base.Init(game_core, parameters);

            if (parameters.Length > 0 && parameters[0] is SceneLoader)
            {
                m_sceneLoader = (SceneLoader)parameters[0];
            }

            m_gameConfig = SerializedScriptableObject.CreateInstance<GameConfig>();
            m_gameProgression = new GameProgression();
            m_navigationInput = GetComponent<NavigationInput>();
            m_cameraManager = GetComponentInChildren<URP_CameraManager>();
            // init here cause there no dependency and its a global object
            m_cameraManager.Init();

            m_sceneLoader.SubscribeToEndLoading(OnLoadingEnded);
        }

        // make all dynamic instantiation here
        public override void OnLoadingEnded()
        {
            // if (GetUnlockChaptersData().Count == 0)
            // {
            //     UnlockChapter(Chapters.The_Prequel, false, false);
            // }

            InitGame();
            InitHud();

            // tricky but w/e for the moment
            m_navigationInput.Init();

            // same
            GetDialogueManager().SubscribeToEndDialogue(OnEndMainDialogue);

            ComicGameCore.Instance.GetSettings().m_settingDatas.m_language = Language.French;
        }

        public void InitHud()
        {
            m_hudManager = SceneUtils.FindObjectAcrossScenes<HudManager>();
            
            if (m_hudManager == null)
            {
                Debug.LogWarning("Can't find HudManager. Try to load the scene before initialize");
                return;
            }

            m_hudManager.Init();
            m_cameraManager.RegisterCameras(m_hudManager.GetRegisteredCameras());
        }

        public void InitGame()
        {
            m_gameManager = SceneUtils.FindObjectAcrossScenes<GameManager>();
            
            if (m_gameManager == null)
            {
                Debug.LogWarning("Can't find GameManager. Try to load the scene before initialize");
                return;
            }

            m_gameManager.Init();
            m_cameraManager.RegisterCameras(m_gameManager.GetRegisteredCameras());
        }

        public void OnEndMainDialogue(DialogueName type)
        {
            GetCharacterManager().GetPlayer().Pause(false);

            if (type == DialogueName.Dialogue_UnlockBF)
                UnlockChapter(Chapters.The_First_Chapter, true, true);
            else if (type == DialogueName.Dialogue_UnlockBully)
                UnlockChapter(Chapters.The_Second_Chapter, true, true);
            else if (type == DialogueName.Dialogue_UnlockBL)
                UnlockChapter(Chapters.The_Third_Chapter, true, true);
            else if (type == DialogueName.Dialogue_UnlockBoss)
                PlayEndGame();
        }

        public void TriggerDialogue(DialogueName type)
        {
            if (GetDialogueManager().StartDialogue(type))
                GetCharacterManager().GetPlayer().Pause(true);
        }

        #region END GAME

        public void PlayEndGame()
        {
            // fix some issues
            m_navigationInput.Pause(true);
            GetCharacterManager().GetPlayer().Pause(true);

            GetViewManager().Show<CreditView>();
            GetDialogueManager().StartCreditDialogue();

            m_onEndGame?.Invoke();
        }

        public void SubscribeToEndGame(Action function)
        {
            m_onEndGame -= function;
            m_onEndGame += function;
        }

        #endregion END GAME

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

        public void SubscribeToPowerSelected(Action<PowerType> function)
        {
            if (GetDialogueManager() == null)
            {
                Debug.LogWarning("DialogueManager could not be found");
                return;
            }

            GetDialogueManager().SubscribeToPowerSelected(function);
        }

        public void SubscribeToNextPower(Action function)
        {
            if (GetCharacterManager() == null)
            {
                Debug.LogWarning("Player could not be found");
                return;
            }

            GetCharacterManager().GetPlayer().SubscribeToNextPower(function);
        }

        public void SubscribeToPrevPower(Action function)
        {
            if (GetCharacterManager() == null)
            {
                Debug.LogWarning("Page manager could not be found");
                return;
            }

            GetCharacterManager().GetPlayer().SubscribeToPrevPower(function);
        }

        public void SubscribeToBeforeSwitchPage(Action<bool, Page, Page> function)
        {
            if (GetPageManager() == null)
            {
                Debug.LogWarning("Page manager could not be found");
                return;
            }

            GetPageManager().SubscribeToBeforeSwitchPage(function);
        }

        public void SubscribeToMiddleSwitchPage(Action<bool, Page, Page> function)
        {
            if (GetPageManager() == null)
            {
                Debug.LogWarning("Page manager could not be found");
                return;
            }

            GetPageManager().SubscribeToMiddleSwitchPage(function);
        }

        public void SubscribeToAfterSwitchPage(Action<bool, Page, Page> function)
        {
            if (GetPageManager() == null)
            {
                Debug.LogWarning("Page manager could not be found");
                return;
            }

            GetPageManager().SubscribeToAfterSwitchPage(function);
        }

        public void SubscribeToAfterCloneCanvasCallback(Action<bool> function)
        {
            if (GetPageManager() == null)
            {
                Debug.LogWarning("Page manager could not be found");
                return;
            }

            GetPageManager().SubscribeToAfterCloneCanvasCallback(function);
        }

        public void SubscribeToLockChapter(Action<Chapters> function)
        {
            m_onLockChapterCallback -= function;
            m_onLockChapterCallback += function;
        }

        public void SubscribeToLockPower(Action<PowerType> function)
        {
            m_onLockPowerCallback -= function;
            m_onLockPowerCallback += function;
        }

        public void SubscribeToLockVoice(Action<VoiceType> function)
        {
            m_onLockVoiceCallback -= function;
            m_onLockVoiceCallback += function;
        }

        public void SubscribeToUnlockChapter(Action<Chapters> function)
        {
            m_onUnlockChapterCallback -= function;
            m_onUnlockChapterCallback += function;
        }

        public void SubscribeToUnlockPower(Action<PowerType> function)
        {
            m_onUnlockPowerCallback -= function;
            m_onUnlockPowerCallback += function;
        }

        public void SubscribeToUnlockVoice(Action<VoiceType> function)
        {
            m_onUnlockVoiceCallback -= function;
            m_onUnlockVoiceCallback += function;
        }

        #endregion

        public override void Pause(bool pause)
        {
            if (pause)
            {
                Debug.Log("Pause game");

                m_navigationInput.Pause(false);
                // pause player
            }
            else
            {
                m_navigationInput.Pause(true);
                // resume player
            }
        }

        protected override void OnUpdate(float elapsed_time)
        {
            base.OnUpdate(elapsed_time);

            // if (m_pause)
            // {
            //     m_navigationInput.OnUpdate();
            // }

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
