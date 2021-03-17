using UnityEngine;

namespace KimiClient
{
    using Utility;

    namespace PlayLogic
    {
        public class GameMode : FSMRoot<EGameState>
        {
            public EGameMode ID { get; private set; }

            public GameMode(EGameMode inID) { ID = inID; }

            public bool Start()
            {
                if (CurrentState == null)
                {
                    Debug.LogWarning("[GameMode]Initialize GameState Before Call Start : ");
                    return false;
                }

                //가장 먼저 넣은 State가 CurrentState로 세팅됨
                return Run(CurrentState.ID);
            }

            public bool InitailizeGameState(GameState inState)
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

