using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimiClient
{
    namespace Utility
    {
        public class EventHandler
        {
            public static int GAMEEVENT_DEFAULT_POOLSIZE = 20;

            public int QueueCount { get { return gameEventQueue.Count; } }

            private Queue<GameEvent> gameEventQueue;
            private Dictionary<int, List<IEventSubscriber>> subscriberDic;
            
            public EventHandler()
            {
                gameEventQueue = new Queue<GameEvent>();
                subscriberDic = new Dictionary<int, List<IEventSubscriber>>();
            }

            ~EventHandler()
            {
                gameEventQueue = null;
                subscriberDic = null;
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
                List<IEventSubscriber> subscriberList;
                if (subscriberDic.TryGetValue(eventData.EventID, out subscriberList))
                {
                    subscriberList.ForEach(x => x.Receive(eventData));
                }
            }

            public bool Subscribe(int inID, IEventSubscriber inSubscriber)
            {
                List<IEventSubscriber> subscriberList;
                if(subscriberDic.TryGetValue(inID, out subscriberList))
                {
                    if(subscriberList.Contains(inSubscriber))
                    {
                        Debug.LogError("EventHandler already contains subscriber! : " + inID + "[" + inSubscriber + "]");
                        return false;
                    }

                    subscriberList.Add(inSubscriber);
                }
                else
                {
                    subscriberList = new List<IEventSubscriber>();
                    subscriberList.Add(inSubscriber);
                    subscriberDic.Add(inID, subscriberList);
                }

                return true;
            }

            public bool UnSubscribe(int inID, IEventSubscriber inSubscriber)
            {
                List<IEventSubscriber> subscriberList;
                if (subscriberDic.TryGetValue(inID, out subscriberList))
                {
                    if (!subscriberList.Contains(inSubscriber))
                    {
                        Debug.LogError("EventHandler not contains subscriber! : " + inID + "[" + inSubscriber + "]");
                        return false;
                    }

                    subscriberList.Remove(inSubscriber);

                    if (subscriberList.Count == 0)
                        subscriberDic.Remove(inID);
                }
                else
                {
                    Debug.LogError("EventHandler not contains subscriber! : " + inID + "[" + inSubscriber + "]");
                    return false;
                }

                return true;
            }
        }
    }
}


