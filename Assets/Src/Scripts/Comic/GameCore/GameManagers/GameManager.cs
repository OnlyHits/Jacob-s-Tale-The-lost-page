using System;
using CustomArchitecture;
using UnityEngine;

namespace Comic
{
    public class GameManager : BaseBehaviour
    {
        private PageManager m_pageManager;
        private CharacterManager m_characterManager;
        private PowerManager m_powerManager;
        private GameCameraRegister m_cameras;
        private DialogueManager m_dialogueManager;

        public PageManager GetPageManager() => m_pageManager;
        public CharacterManager GetCharacterManager() => m_characterManager;
        public PowerManager GetPowerManager() => m_powerManager;
        public GameCameraRegister GetRegisteredCameras() => m_cameras;
        public DialogueManager GetDialogueManager() => m_dialogueManager;

        public void Init()
        {
            m_pageManager = gameObject.GetComponent<PageManager>();
            m_characterManager = gameObject.GetComponent<CharacterManager>();
            m_powerManager = gameObject.GetComponent<PowerManager>();
            m_dialogueManager = gameObject.GetComponent<DialogueManager>();
            m_cameras = gameObject.GetComponent<GameCameraRegister>();

            m_pageManager.Init();
            m_characterManager.Init();
            m_powerManager.Init();

            if (ComicGameCore.Instance.MainGameMode.GetViewManager() != null)
            {
                DialogueView dialogue_view = ComicGameCore.Instance.MainGameMode.GetViewManager().GetView<DialogueView>();
                CreditView credit_view = ComicGameCore.Instance.MainGameMode.GetViewManager().GetView<CreditView>();

                m_dialogueManager.Init(dialogue_view, credit_view);
            }
            else
            {
                // null case is handle inside (not for credit lmao)
                m_dialogueManager.Init(null, null);
            }
        }
    }
}