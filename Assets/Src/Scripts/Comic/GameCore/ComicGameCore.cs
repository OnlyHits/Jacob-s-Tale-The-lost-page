using UnityEngine;
using CustomArchitecture;
using UnityEngine.InputSystem;

namespace Comic
{
    [DefaultExecutionOrder(-2)]
    public class ComicGameCore : AGameCore
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void InstantiateGameModes()
        {
            CreateGameMode<MainGameMode>();

            SetStartingGameMode<MainGameMode>();
        }
    }
}
