using UnityEngine;
using CustomArchitecture;

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
