using CustomArchitecture;

namespace Comic
{
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
