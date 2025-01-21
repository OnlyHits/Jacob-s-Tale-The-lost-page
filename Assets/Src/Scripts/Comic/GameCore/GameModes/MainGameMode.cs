using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using CustomArchitecture;

namespace Comic
{
    public class MainGameMode : AGameMode
    {
        private GameConfig m_gameConfig;
        private GameProgression m_gameProgression;
        private Setting m_settings;
        private PauseInput m_pauseInput;
        private NavigationInput m_hudNavigationInput;

        public override void OnLoadingEnded()
        {
            return;
        }

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

        public override void Init(AGameCore game_core, params object[] parameters)
        {
            base.Init(game_core, parameters);

            m_gameConfig = SerializedScriptableObject.CreateInstance<GameConfig>();
            m_gameProgression = new GameProgression();
            m_settings = new Setting();
            m_pauseInput = GetComponent<PauseInput>();
            m_hudNavigationInput = GetComponent<NavigationInput>();
            m_pauseInput.Init();
            m_hudNavigationInput.Init();
        }

        protected override void OnUpdate(float elapsed_time)
        {
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

        // Pause every managed objects
        //public override void Pause(bool pause)
        //{}
    }
}