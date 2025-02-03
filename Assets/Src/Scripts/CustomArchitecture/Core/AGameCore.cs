using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace CustomArchitecture
{
    public abstract class AGameCore<T> : BaseBehaviour where T : AGameCore<T>
    {
        private static T m_instance;

        private List<AGameMode<T>>  m_gameModes = new();
        private AGameMode<T>        m_currentGameMode = null;
        private AGameMode<T>        m_startingGameMode = null;
        private Settings m_settings = null;
        public Settings GetSettings() => m_settings;

        // Prevent direct instantiation
        protected AGameCore() { }

        #region Singleton
        public static T Instance
        {
            get
            {
                // kind of depreciated, until agamecore is abstract
                if (m_instance == null)
                {
                    m_instance = FindFirstObjectByType<T>();

                    if (m_instance == null)
                    {
                        GameObject singletonObject = new GameObject("GameCore");
                        m_instance = singletonObject.AddComponent<T>();
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

            Instance = (T)this;
            DontDestroyOnLoad(gameObject);

            m_settings = new Settings();

            InstantiateGameModes();
        }

        private void Start()
        {
            if (m_startingGameMode != null)
            {
                StartGameMode(m_startingGameMode);
            }
        }

        protected void CreateGameMode<U>(params object[] parameters) where U : AGameMode<T>
        {
            if (Exist<U>())
            {
                Debug.LogError("Game mode already exist");
                return;
            }

            U game_mode = gameObject.AddComponent<U>();

            if (game_mode != null)
            {
                m_gameModes.Add(game_mode);
                game_mode.Init((T)this, parameters);
            }
            else
            {
                Debug.LogError("Add Component fail");
            }
        }

        protected void SetStartingGameMode<U>() where U : AGameMode<T>
        {
            AGameMode<T> game_mode = GetGameMode<U>();

            if (game_mode != null)
                m_startingGameMode = game_mode;
            else
                Debug.LogError("Game mode doesn't exist");
        }

        private bool Exist(AGameMode<T> game_mode)
        {
            foreach (AGameMode<T> mode in m_gameModes)
            {
                if (mode == game_mode)
                    return true;
            }
            return false;
        }

        private bool Exist<U>() where U : AGameMode<T>
        {
            foreach (AGameMode<T> game_mode in m_gameModes)
            {
                if (game_mode is U)
                    return true;
            }
            return false;
        }

        public U GetGameMode<U>() where U : AGameMode<T>
        {
            foreach (AGameMode<T> game_mode in m_gameModes)
            {
                if (game_mode is U)
                    return (U)game_mode;
            }

            Debug.LogError("Game mode doesn't exist");
            return null;
        }

        protected void StartGameMode(AGameMode<T> game_mode)
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

        public void StartGameMode<U>() where U : AGameMode<T>
        {
            if (!Exist<U>())
            {
                Debug.LogError("Game mode doesn't exist");
                return;
            }

            StopGameMode();

            m_currentGameMode = GetGameMode<U>();
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
