using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimiClient
{
    namespace Utility
    {
        public class EventHandler<GameEventType>
        {
            public static int GAMEEVENT_DEFAULT_POOLSIZE = 20;

            public int QueueCount { get { return gameEventQueue.Count; } }

            private Queue<GameEvent<GameEventType>> gameEventQueue;
            private Dictionary<GameEventType, List<IEventSubscriber<GameEventType>>> subscriberDic;
            
            public EventHandler()
            {
                gameEventQueue = new Queue<GameEvent<GameEventType>>();
                subscriberDic = new Dictionary<GameEventType, List<IEventSubscriber<GameEventType>>>();
            }

            ~EventHandler()
            {
                gameEventQueue = null;
                subscriberDic = null;
            }

            public void Publish(GameEvent<GameEventType> inEvent)
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
                GameEvent<GameEventType> eventData = gameEventQueue.Dequeue();
                List<IEventSubscriber<GameEventType>> subscriberList;
                if (subscriberDic.TryGetValue(eventData.Type, out subscriberList))
                {
                    subscriberList.ForEach(x => x.Receive(eventData));
                }
            }

            public bool Subscribe(GameEventType inType, IEventSubscriber<GameEventType> inSubscriber)
            {
                List<IEventSubscriber<GameEventType>> subscriberList;
                if(subscriberDic.TryGetValue(inType, out subscriberList))
                {
                    if(subscriberList.Contains(inSubscriber))
                    {
                        Debug.LogError("EventHandler already contains subscriber! : " + inType + "[" + inSubscriber + "]");
                        return false;
                    }

                    subscriberList.Add(inSubscriber);
                }
                else
                {
                    subscriberList = new List<IEventSubscriber<GameEventType>>();
                    subscriberList.Add(inSubscriber);
                    subscriberDic.Add(inType, subscriberList);
                }

                return true;
            }

            public bool UnSubscribe(GameEventType inType, IEventSubscriber<GameEventType> inSubscriber)
            {
                List<IEventSubscriber<GameEventType>> subscriberList;
                if (subscriberDic.TryGetValue(inType, out subscriberList))
                {
                    if (!subscriberList.Contains(inSubscriber))
                    {
                        Debug.LogError("EventHandler not contains subscriber! : " + inType + "[" + inSubscriber + "]");
                        return false;
                    }

                    subscriberList.Remove(inSubscriber);

                    if (subscriberList.Count == 0)
                        subscriberDic.Remove(inType);
                }
                else
                {
                    Debug.LogError("EventHandler not contains subscriber! : " + inType + "[" + inSubscriber + "]");
                    return false;
                }

                return true;
            }
        }
    }
}


