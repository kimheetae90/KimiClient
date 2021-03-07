using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimiClient
{
    namespace Utility
    {
        public class GameEventHandler : IGameEventPublisher
        {
            public int QueueCount { get { return gameEventQueue.Count; } }

            private Queue<GameEvent> gameEventQueue;
            private GameEventRouter gameEventRouter;
            
            public GameEventHandler()
            {
                gameEventQueue = new Queue<GameEvent>();
                gameEventRouter = new GameEventRouter();
            }

            ~GameEventHandler()
            {
                gameEventQueue = null;
                gameEventRouter = null;
            }

            public void Publish(GameEvent inEvent)
            {
                gameEventQueue.Enqueue(inEvent);
            }

            public void FireAllEvent()
            {
                int count = QueueCount;
                for (int i = 0; i < count; i++)
                {
                    FireEvent();
                }
            }

            public void FireEvent()
            {
                GameEvent eventData = gameEventQueue.Dequeue();
                gameEventRouter.FireEvent(eventData);
            }

            public bool Subscribe(int inID, IGameEventSubscriber inSubscriber)
            {
                return gameEventRouter.Subscribe(inID, inSubscriber);
            }

            public bool UnSubscribe(int inID, IGameEventSubscriber inSubscriber)
            {
                return gameEventRouter.UnSubscribe(inID, inSubscriber);
            }

            public void FireEvent(GameEvent inEvent)
            {
                gameEventRouter.FireEvent(inEvent);
            }
        }
    }
}


