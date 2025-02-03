using CustomArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomArchitecture
{
    public abstract class AGameMode<T> : BaseBehaviour where T : AGameCore<T>
    {
        protected string                            m_gameName;
        protected T                                 m_gameCore;
        [SerializeField, ReadOnly] protected bool   m_isCompute;
        protected string                            m_gameSceneName = null;
        protected string                            m_uiSceneName = null;

        // public AGameMode(string name)
        // {
        //     m_gameName = name;
        // }

        public abstract void StartGameMode();
        public abstract void OnLoadingEnded();
        public abstract void StopGameMode();
        public abstract void RestartGameMode();
        //        public abstract void Pause(bool pause);

        public bool Compute
        {
            get { return m_isCompute; }
            protected set { m_isCompute = value; }
        }

        public virtual void Init(T game_core, params object[] parameters)
        {
            m_gameCore = game_core;
        }

        public string GetName
        {
            get
            {
                return m_gameName;
            }
            set
            {
                m_gameName = value;
            }
        }
    }
}
