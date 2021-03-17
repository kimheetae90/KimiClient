using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using KimiClient.PlayLogic;

namespace Tests
{
    public class GameModeTest
    {
        public class DefaultGameMode : GameMode
        {
            public DefaultGameMode() : base(EGameMode.DefaultGameMode1) { }
        }

        public class DefaultGameState1 : GameState
        {
            public DefaultGameState1() : base(EGameState.DefaultGameState1_1) { }
        }

        public class DefaultGameState2 : GameState
        {
            public DefaultGameState2() : base(EGameState.DefaultGameState1_2) { }
        }

        DefaultGameMode defaultGameMode;
        DefaultGameState1 defaultGameState1;
        DefaultGameState2 defaultGameState2;

        [UnityTest]
        public IEnumerator InitailizeGameState()
        {
            Initiallize();

            Assert.AreEqual(2, defaultGameMode.StateCount);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Start()
        {
            defaultGameMode = new DefaultGameMode();

            Assert.AreEqual(false, defaultGameMode.Start());

            defaultGameState1 = new DefaultGameState1();
            defaultGameState2 = new DefaultGameState2();

            defaultGameMode.InitailizeGameState(defaultGameState1);
            defaultGameMode.InitailizeGameState(defaultGameState2);

            Assert.AreEqual(true, defaultGameMode.Start());

            GameState tempGameState = defaultGameMode.GetGameState(EGameState.DefaultGameState1_1);
            Assert.AreEqual(EGameState.DefaultGameState1_1, tempGameState.ID);
            tempGameState = defaultGameMode.GetGameState(EGameState.DefaultGameState1_2);
            Assert.AreEqual(EGameState.DefaultGameState1_2, tempGameState.ID);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GetGameState()
        {
            Initiallize();

            GameState tempGameState = defaultGameMode.GetGameState(EGameState.DefaultGameState1_1);
            Assert.AreEqual(EGameState.DefaultGameState1_1, tempGameState.ID);
            tempGameState = defaultGameMode.GetGameState(EGameState.DefaultGameState1_2);
            Assert.AreEqual(EGameState.DefaultGameState1_2, tempGameState.ID);

            yield return null;
        }

        [UnityTest]
        public IEnumerator ChangeGameState()
        {
            Initiallize();

            Assert.AreEqual(EGameState.DefaultGameState1_1, defaultGameMode.CurrentState.ID);
            defaultGameMode.ChangeGameState(EGameState.DefaultGameState1_2);
            Assert.AreEqual(EGameState.DefaultGameState1_2, defaultGameMode.CurrentState.ID);

            yield return null;
        }

        private void Initiallize()
        {
            defaultGameMode = new DefaultGameMode();
            defaultGameState1 = new DefaultGameState1();
            defaultGameState2 = new DefaultGameState2();

            defaultGameMode.InitailizeGameState(defaultGameState1);
            defaultGameMode.InitailizeGameState(defaultGameState2);

            TestRunnerHelper.Reset();
        }
    }
}
