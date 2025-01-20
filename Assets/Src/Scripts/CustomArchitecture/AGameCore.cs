using UnityEngine;
using System.Collections.Generic;

namespace CustomArchitecture
{
    public abstract class AGameCore : BaseBehaviour
    {
        private static AGameCore m_instance;
        private List<AGameMode> m_gameModes = new();
        private AGameMode m_currentGameMode = null;
        private AGameMode m_startingGameMode = null;

        // Prevent direct instantiation
        protected AGameCore() { }

        #region Singleton
        public static AGameCore Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindFirstObjectByType<AGameCore>();

                    if (m_instance == null)
                    {
                        GameObject singletonObject = new GameObject("GameCore");
                        m_instance = singletonObject.AddComponent<AGameCore>();
                    }
                }
                return m_instance;
            }
            private set
            {
                m_instance = value;
            }
        }
        #endregion

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.Log("Already instantiate");
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InstantiateGameModes();
        }

        private void Start()
        {
            if (m_startingGameMode != null)
            {
                StartGameMode(m_startingGameMode);
            }
        }

        protected void CreateGameMode<T>(params object[] parameters) where T : AGameMode
        {
            if (Exist<T>())
            {
                Debug.LogError("Game mode already exist");
                return;
            }

            T game_mode = gameObject.AddComponent<T>();

            if (game_mode != null)
            {
                game_mode.Init(this, parameters);
                m_gameModes.Add(game_mode);
            }
            else
            {
                Debug.LogError("Add Component fail");
            }
        }

        protected void SetStartingGameMode<T>() where T : AGameMode
        {
            AGameMode game_mode = GetGameMode<T>();

            if (game_mode != null)
                m_startingGameMode = game_mode;
            else
                Debug.LogError("Game mode doesn't exist");
        }

        private bool Exist(AGameMode game_mode)
        {
            foreach (AGameMode mode in m_gameModes)
            {
                if (mode == game_mode)
                    return true;
            }
            return false;
        }

        private bool Exist<T>() where T : AGameMode
        {
            foreach (AGameMode game_mode in m_gameModes)
            {
                if (game_mode is T)
                    return true;
            }
            return false;
        }

        public T GetGameMode<T>() where T : AGameMode
        {
            foreach (AGameMode game_mode in m_gameModes)
            {
                if (game_mode is T)
                    return (T)game_mode;
            }

            Debug.LogError("Game mode doesn't exist");
            return null;
        }

        protected void StartGameMode(AGameMode game_mode)
        {
            if (!Exist(game_mode))
            {
                Debug.LogError("Game mode doesn't exist");
                return;
            }
            
            StopGameMode();

            m_currentGameMode = game_mode;
            m_currentGameMode.StartGameMode();
        }

        public void StartGameMode<T>() where T : AGameMode
        {
            if (!Exist<T>())
            {
                Debug.LogError("Game mode doesn't exist");
                return;
            }

            StopGameMode();

            m_currentGameMode = GetGameMode<T>();
            m_currentGameMode.StartGameMode();
        }

        private void StopGameMode()
        {
            if (m_currentGameMode != null)
                m_currentGameMode.StopGameMode();
        }

        protected abstract void InstantiateGameModes();
    }
}
