using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using KimiClient.Utility;

namespace Tests
{
    public class FSMTest
    {
        public class TestFSMRoot : FSMRoot<ETestRunenrEnum>
        {
            public int Counter { get; private set; }

            protected override void OnRun()
            {
                Counter = 1;
            }

            protected override void OnUpdate()
            {
                Counter++;
            }

            protected override void OnStop()
            {
                Counter = -1;
            }
        }

        public class TestFSMState1 : FSMState<ETestRunenrEnum>
        {
            public TestFSMState1() : base(ETestRunenrEnum.Test1) { }

            protected override void OnEnter()
            {
                TestRunnerHelper.testInt = 1; 
            }

            protected override void OnStay()
            {
                TestRunnerHelper.testInt += 1;
            }

            protected override void OnExit()
            {
                TestRunnerHelper.testInt = -1;
            }

            protected override void OnDispose()
            {
            }
        }

        public class TestFSMState2 : FSMState<ETestRunenrEnum>
        {
            public TestFSMState2() : base(ETestRunenrEnum.Test2) { }

            protected override void OnEnter()
            {
                TestRunnerHelper.testFloat = 2.0f;
            }

            protected override void OnStay()
            {
                TestRunnerHelper.testFloat += 2.0f;
            }

            protected override void OnExit()
            {
                TestRunnerHelper.testFloat = -2.0f;
            }

            protected override void OnDispose()
            {
            }
        }

        public class TestFSMState3 : FSMState<ETestRunenrEnum>
        {
            public TestFSMState3() : base(ETestRunenrEnum.Test3) { }

            protected override void OnEnter()
            {
            }

            protected override void OnStay()
            {
            }

            protected override void OnExit()
            {
            }

            protected override void OnDispose()
            {
            }
        }

        TestFSMRoot root;
        TestFSMState1 state1;
        TestFSMState2 state2;
        TestFSMState3 state3;

        [UnityTest]
        public IEnumerator Add()
        {
            root = new TestFSMRoot();
            state1 = new TestFSMState1();
            state2 = new TestFSMState2();
            state3 = new TestFSMState3();

            Assert.AreEqual(false, root.AddState(null));
            Assert.AreEqual(true, root.AddState(state1));
            Assert.AreEqual(ETestRunenrEnum.Test1, root.CurrentState.ID);
            Assert.AreEqual(true, root.AddState(state2));
            Assert.AreEqual(true, root.AddState(state3));
            Assert.AreEqual(ETestRunenrEnum.Test1, root.CurrentState.ID);

            TestFSMState1 sameState1 = new TestFSMState1();
            Assert.AreEqual(false, root.AddState(sameState1));
            Assert.AreEqual(3, root.StateCount);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GetState()
        {
            InitiallizeFSM();

            FSMState<ETestRunenrEnum> state = root.GetState(ETestRunenrEnum.Test1);
            Assert.AreEqual(ETestRunenrEnum.Test1, state.ID);
            Assert.AreEqual(null, root.GetState(ETestRunenrEnum.None));

            yield return null;
        }

        [UnityTest]
        public IEnumerator Run()
        {
            InitiallizeFSM();

            TestRunnerHelper.testBoolean = false;
            System.Action onRun = () => { TestRunnerHelper.testBoolean = true; };
            root.OnRunAction += onRun;

            Assert.AreEqual(false, TestRunnerHelper.testBoolean);
            Assert.AreEqual(0, root.Counter);
            Assert.AreEqual(false, root.Run(ETestRunenrEnum.None));
            Assert.AreEqual(false, TestRunnerHelper.testBoolean);
            Assert.AreEqual(ETestRunenrEnum.Test1, root.CurrentState.ID);
            Assert.AreEqual(true, root.Run(ETestRunenrEnum.Test2));
            Assert.AreEqual(true, TestRunnerHelper.testBoolean);
            Assert.AreEqual(false, root.Run(ETestRunenrEnum.Test3));            
            Assert.AreEqual(ETestRunenrEnum.Test2, root.CurrentState.ID);
            Assert.AreEqual(1, root.Counter);

            TestRunnerHelper.testBoolean = false;
            root.Stop();
            Assert.AreEqual(false, TestRunnerHelper.testBoolean);
            Assert.AreEqual(-1, root.Counter);
            root.OnRunAction -= onRun;
            root.Run(ETestRunenrEnum.Test1);
            Assert.AreEqual(false, TestRunnerHelper.testBoolean);            

            yield return null;
        }

        [UnityTest]
        public IEnumerator Update()
        {
            InitiallizeFSM();

            System.Action onUpdate = () => { TestRunnerHelper.testInt++; };
            root.OnUpdateAction += onUpdate;

            Assert.AreEqual(false, root.Update());
            Assert.AreEqual(true, root.Run(ETestRunenrEnum.Test1));

            while (root.Update())
            {
                if (root.Counter == 30)
                {
                    break;
                }

                yield return null;
            }

            Assert.AreEqual(30, root.Counter);
            Assert.AreEqual(59, TestRunnerHelper.testInt);
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(30, root.Counter);
            Assert.AreEqual(59, TestRunnerHelper.testInt);
            Assert.AreEqual(true, root.Update());
            Assert.AreEqual(31, root.Counter);
            Assert.AreEqual(61, TestRunnerHelper.testInt);

            TestRunnerHelper.testInt = 0;
            root.OnUpdateAction -= onUpdate;
            Assert.AreEqual(true, root.Update());
            Assert.AreEqual(32, root.Counter);
            Assert.AreEqual(1, TestRunnerHelper.testInt);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Transition()
        {
            InitiallizeFSM();

            root.Run(ETestRunenrEnum.Test1);
            root.Transition(ETestRunenrEnum.Test2);

            Assert.AreEqual(ETestRunenrEnum.Test2, root.CurrentState.ID);

            yield return null;
        }


        [UnityTest]
        public IEnumerator StateEnter()
        {
            InitiallizeFSM();

            root.Run(ETestRunenrEnum.Test1);
            Assert.AreEqual(1, TestRunnerHelper.testInt);
            Assert.AreEqual(0.0f, TestRunnerHelper.testFloat);
            root.Transition(ETestRunenrEnum.Test2);
            Assert.AreEqual(2.0f, TestRunnerHelper.testFloat);

            yield return null;
        }


        [UnityTest]
        public IEnumerator StateStay()
        {
            InitiallizeFSM();

            root.Run(ETestRunenrEnum.Test1);
            while (root.Update())
            {
                if (TestRunnerHelper.testInt >= 30)
                {
                    TestRunnerHelper.testInt = 0;
                    root.Transition(ETestRunenrEnum.Test2);
                }

                if (TestRunnerHelper.testFloat >= 30.0f)
                {
                    TestRunnerHelper.testFloat = 30.0f;
                    break;
                }

                yield return null;
            }

            Assert.IsTrue(true);

            yield return null;
        }


        [UnityTest]
        public IEnumerator StateExit()
        {
            InitiallizeFSM();

            root.Run(ETestRunenrEnum.Test1);
            root.Transition(ETestRunenrEnum.Test2);
            Assert.AreEqual(-1, TestRunnerHelper.testInt);
            Assert.AreEqual(2.0f, TestRunnerHelper.testFloat);
            yield return null;
        }

        [UnityTest]
        public IEnumerator NotOverrideTest()
        {
            FSMRoot<ETestRunenrEnum> fSMRoot = new FSMRoot<ETestRunenrEnum>();
            fSMRoot.AddState(new FSMState<ETestRunenrEnum>(ETestRunenrEnum.Test1));
            fSMRoot.AddState(new FSMState<ETestRunenrEnum>(ETestRunenrEnum.Test2));
            fSMRoot.AddState(new FSMState<ETestRunenrEnum>(ETestRunenrEnum.Test3));

            TestRunnerHelper.Reset();
            TestRunnerHelper.testBoolean = false;
            fSMRoot.OnRunAction = () => { TestRunnerHelper.testBoolean = true; };
            fSMRoot.OnUpdateAction = () => { TestRunnerHelper.testFloat += 1.0f; };
            fSMRoot.OnStopAction = () => { TestRunnerHelper.testBoolean = false; };

            FSMState<ETestRunenrEnum> fsmState = fSMRoot.GetState(ETestRunenrEnum.Test1);
            fsmState.OnEnterAction = () => { TestRunnerHelper.testInt = 1; };
            fsmState.OnStayAction = () => { TestRunnerHelper.testInt += 1; };
            fsmState.OnExitAction = () => { TestRunnerHelper.testFloat = 0.1f; };

            fsmState = fSMRoot.GetState(ETestRunenrEnum.Test2);
            fsmState.OnEnterAction = () => { TestRunnerHelper.testInt = 2; };
            fsmState.OnStayAction = () => { TestRunnerHelper.testInt -= 2; };
            fsmState.OnExitAction = () => { TestRunnerHelper.testFloat = 0.2f; };

            fsmState = fSMRoot.GetState(ETestRunenrEnum.Test3);
            fsmState.OnEnterAction = () => { TestRunnerHelper.testInt = 3; };
            fsmState.OnStayAction = () => { TestRunnerHelper.testInt += 3; };
            fsmState.OnExitAction = () => { TestRunnerHelper.testFloat = 0.3f; };

            fSMRoot.Run(ETestRunenrEnum.Test1);

            Assert.AreEqual(true, TestRunnerHelper.testBoolean);
            Assert.AreEqual(1, TestRunnerHelper.testInt);

            while (fSMRoot.Update())
            {
                if (TestRunnerHelper.testInt > 30)
                {
                    Assert.AreEqual(true, TestRunnerHelper.testFloat >= 30.0f);
                    fSMRoot.Transition(ETestRunenrEnum.Test2);
                    break;
                }
                yield return null;
            }

            Assert.AreEqual(2, TestRunnerHelper.testInt);
            Assert.AreEqual(0.1f, TestRunnerHelper.testFloat);

            while (fSMRoot.Update())
            {
                if (TestRunnerHelper.testInt < -60)
                {
                    Assert.AreEqual(true, TestRunnerHelper.testFloat >= 30.0f);
                    fSMRoot.Transition(ETestRunenrEnum.Test3);
                    break;
                }
                yield return null;
            }

            Assert.AreEqual(3, TestRunnerHelper.testInt);
            Assert.AreEqual(0.2f, TestRunnerHelper.testFloat);

            while (fSMRoot.Update())
            {
                if (TestRunnerHelper.testInt > 90)
                {
                    Assert.AreEqual(true, TestRunnerHelper.testFloat >= 30.0f);
                    fSMRoot.Stop();
                    break;
                }
                yield return null;
            }

            Assert.AreEqual(0.3f, TestRunnerHelper.testFloat);
            Assert.AreEqual(false, TestRunnerHelper.testBoolean);

            yield return null;
        }

        private void InitiallizeFSM()
        {
            root = new TestFSMRoot();
            state1 = new TestFSMState1();
            state2 = new TestFSMState2();
            state3 = new TestFSMState3();

            root.AddState(state1);
            root.AddState(state2);
            root.AddState(state3);

            TestRunnerHelper.Reset();
        }
    }
}
