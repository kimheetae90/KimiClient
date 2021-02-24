using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KimiClient
{
    namespace Utility
    {
        public class GameEventRouter : IGameEventSubscriber, IGameEventPublisher
        {
            private IGameEventPublisher publisher;
            private Dictionary<int, List<IGameEventSubscriber>> subscriberDic;

            public GameEventRouter()
            {
                subscriberDic = new Dictionary<int, List<IGameEventSubscriber>>();
            }

            ~GameEventRouter()
            {
                subscriberDic = null;
            }

            public void LinkPublisher(IGameEventPublisher inPublisher)
            {
                if(inPublisher == null)
                {
                    Debug.LogError("GameEventPublisher is Null in GameEventRouter!");
                    return;
                }

                publisher = inPublisher;
            }

            public bool Subscribe(int inID, IGameEventSubscriber inSubscriber)
            {
                List<IGameEventSubscriber> subscriberList;
                if (subscriberDic.TryGetValue(inID, out subscriberList))
                {
                    if (subscriberList.Contains(inSubscriber))
                    {
                        Debug.LogError("GameEventRouter already contains subscriber! : " + inID + "[" + inSubscriber + "]");
                        return false;
                    }
                }
                else
                {
                    subscriberList = new List<IGameEventSubscriber>();
                    subscriberDic.Add(inID, subscriberList);
                    publisher.Subscribe(inID, this);
                }

                subscriberList.Add(inSubscriber);

                return true;
            }

            public bool UnSubscribe(int inID, IGameEventSubscriber inSubscriber)
            {
                List<IGameEventSubscriber> subscriberList;
                if (subscriberDic.TryGetValue(inID, out subscriberList))
                {
                    if (!subscriberList.Contains(inSubscriber))
                    {
                        Debug.LogError("GameEventRouter not contains subscriber! : " + inID + "[" + inSubscriber + "]");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("GameEventRouter not contains subscriber! : " + inID + "[" + inSubscriber + "]");
                    return false;
                }

                subscriberList.Remove(inSubscriber);

                if (subscriberList.Count == 0)
                {
                    subscriberDic.Remove(inID);
                    publisher.UnSubscribe(inID, this);
                }

                return true;
            }

            public void Receive(GameEvent inEvent)
            {
                FireEvent(inEvent);
            }

            public void FireEvent(GameEvent inEvent)
            {
                List<IGameEventSubscriber> subscriberList;
                if (subscriberDic.TryGetValue(inEvent.EventID, out subscriberList))
                {
                    subscriberList.ForEach(x => x.Receive(inEvent));
                }
            }
        }

    }
}