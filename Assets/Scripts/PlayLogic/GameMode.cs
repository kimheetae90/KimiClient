namespace KimiClient
{
    using Utility;

    namespace PlayLogic
    {
        public class GameMode : FSMRoot<EGameState>
        {
            public bool Start()
            {
                return Run(CurrentState.ID);
            }

            public bool InitGameState(GameState inState)
            {
                return AddState(inState);
            }

            public bool ChangeGameState(EGameState inState)
            {
                return Transition(inState);
            }

            public GameState GetGameState(EGameState inState)
            {
                return GetState(inState) as GameState;
            }
        }
    }
}

