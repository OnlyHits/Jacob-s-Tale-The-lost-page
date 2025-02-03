using UnityEngine;
using CustomArchitecture;
using UnityEngine.InputSystem;

namespace Comic
{
   [DefaultExecutionOrder(-2)]
    public class ComicGameCore : AGameCore<ComicGameCore>
    {
        [SerializeField] private InputActionAsset m_inputActionAsset;
        private SceneLoader m_sceneLoader;

        public InputActionAsset GetInputAsset() => m_inputActionAsset;

        public MainGameMode MainGameMode
        {
            get { return GetGameMode<MainGameMode>(); }
            protected set {}
        }

        protected override void InstantiateGameModes()
        {
            m_sceneLoader = gameObject.GetComponent<SceneLoader>();
            
            if (m_sceneLoader == null)
            {
                m_sceneLoader = gameObject.AddComponent<SceneLoader>();
            }

            CreateGameMode<MainGameMode>(m_sceneLoader);

            // SetStartingGameMode<MainGameMode>();
        }
    }
}
