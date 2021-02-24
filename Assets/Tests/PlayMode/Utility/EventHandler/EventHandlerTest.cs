using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using KimiClient.Utility;

namespace Tests
{
    public class EventHandlerTest
    {
        public class TestRunnerGameEvent1 : GameEvent
        {
            public int testInt;

            public TestRunnerGameEvent1()
            {
                EventID = (int)ETestRunenrEnum.Test1;
            }
        }

        public class TestRunnerGameEvent2 : GameEvent
        {
            public float testFloat;

            public TestRunnerGameEvent2()
            {
                EventID = (int)ETestRunenrEnum.Test2;
            }
        }

        public class EventHandlerSubscriber1 : IGameEventSubscriber
        {
            public int testInt;

            public void Receive(GameEvent inEvent)
            {
                if(inEvent.EventID == (int)ETestRunenrEnum.Test1)
                {
                    TestRunnerGameEvent1 event1 = inEvent as TestRunnerGameEvent1;
                    testInt = event1.testInt;
                }
            }
        }

        public class EventHandlerSubscriber2 : IGameEventSubscriber
        {
            public float testFloat;

            public void Receive(GameEvent inEvent)
            {
                if (inEvent.EventID == (int)ETestRunenrEnum.Test2)
                {
                    TestRunnerGameEvent2 event2 = inEvent as TestRunnerGameEvent2;
                    testFloat = event2.testFloat;
                }
            }
        }

        public class EventHandlerSubscriber3 : IGameEventSubscriber
        {
            public int testInt;
            public float testFloat;

            public void Receive(GameEvent inEvent)
            {
                if (inEvent.EventID == (int)ETestRunenrEnum.Test1)
                {
                    TestRunnerGameEvent1 event1 = inEvent as TestRunnerGameEvent1;
                    testInt = event1.testInt;
                }
                else if(inEvent.EventID == (int)ETestRunenrEnum.Test2)
                {
                    TestRunnerGameEvent2 event2 = inEvent as TestRunnerGameEvent2;
                    testFloat = event2.testFloat;
                }
            }
        }

        [UnityTest]
        public IEnumerator EventHandlerTestWithEnumeratorPasses()
        {
            GameEventHandler eventHandler = new GameEventHandler();

            EventHandlerSubscriber1 subscriber1 = new EventHandlerSubscriber1();
            EventHandlerSubscriber2 subscriber2 = new EventHandlerSubscriber2();
            EventHandlerSubscriber3 subscriber3 = new EventHandlerSubscriber3();

            eventHandler.Subscribe((int)ETestRunenrEnum.Test1 ,subscriber1);
            eventHandler.Subscribe((int)ETestRunenrEnum.Test2, subscriber2);
            eventHandler.Subscribe((int)ETestRunenrEnum.Test1, subscriber3);
            eventHandler.Subscribe((int)ETestRunenrEnum.Test2, subscriber3);

            int testInt = 3;
            TestRunnerGameEvent1 event1 = new TestRunnerGameEvent1();
            event1.testInt = testInt;
            float testFloat = 2.5f;
            TestRunnerGameEvent2 event2 = new TestRunnerGameEvent2();
            event2.testFloat = testFloat;

            eventHandler.Publish(event1);
            eventHandler.Publish(event2);

            eventHandler.FireAllEvent();

            Assert.AreEqual(testInt, subscriber1.testInt);
            Assert.AreEqual(testFloat, subscriber2.testFloat);
            Assert.AreEqual(testInt, subscriber3.testInt);
            Assert.AreEqual(testFloat, subscriber3.testFloat);

            eventHandler.UnSubscribe((int)ETestRunenrEnum.Test1, subscriber1);
            eventHandler.UnSubscribe((int)ETestRunenrEnum.Test2, subscriber3);

            int newTestInt = 999;
            float newTestFloat = 999.99f;
            event1.testInt = newTestInt;
            event2.testFloat = newTestFloat;
            eventHandler.Publish(event1);
            eventHandler.Publish(event2);

            eventHandler.FireAllEvent();

            Assert.AreEqual(testInt, subscriber1.testInt);
            Assert.AreEqual(newTestFloat, subscriber2.testFloat);
            Assert.AreEqual(newTestInt, subscriber3.testInt);
            Assert.AreEqual(testFloat, subscriber3.testFloat);

            yield return null;
        }
    }
}
