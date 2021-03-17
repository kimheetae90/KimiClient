namespace KimiClient
{
    using Utility;

    namespace PlayLogic
    {
        public class GameState : FSMState<EGameState>
        {
            public GameState(EGameState inID) : base(inID) {}
        }
    }
}

