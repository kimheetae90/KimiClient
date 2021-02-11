using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimiClient
{
    namespace Utility
    {
        public interface IEventSubscriber<GameEventType>
        {
            public void Receive(GameEvent<GameEventType> inEvent);
        }


        public struct GameEvent<GameEventType>
        {
            public GameEventType Type;

            private Dictionary<string, bool> boolDic;
            private Dictionary<string, int> intDic;
            private Dictionary<string, long> longDic;
            private Dictionary<string, float> floatDic;
            private Dictionary<string, double> doubleDic;
            private Dictionary<string, Vector2> vector2Dic;
            private Dictionary<string, Vector3> vector3Dic;
            private Dictionary<string, object> objectDic;

            public void Clear()
            {
                boolDic = null;
                intDic = null;
                longDic = null;
                floatDic = null;
                doubleDic = null;
                vector2Dic = null;
                vector3Dic = null;
            }

            public ReturnType Get<ReturnType>(string key)
            {
                if(typeof(ReturnType) == typeof(bool))
                {
                    if(boolDic.ContainsKey(key))
                        return (ReturnType)boolDic[key];
                }
                else if (typeof(ReturnType) == typeof(int))
                {

                }
                else if (typeof(ReturnType) == typeof(long))
                {

                }
                else if (typeof(ReturnType) == typeof(float))
                {

                }
                else if (typeof(ReturnType) == typeof(double))
                {

                }
                else if (typeof(ReturnType) == typeof(Vector2))
                {

                }
                else if (typeof(ReturnType) == typeof(Vector3))
                {

                }
                else
                {

                }
            }

            bool Add(string key, bool data)
            {
                if (boolDic == null)
                    boolDic = new Dictionary<string, bool>();

                if (boolDic.ContainsKey(key))
                    return false;

                boolDic.Add(key, data);
                return true;
            }

            bool Add(string key, int data)
            {
                if (intDic == null)
                    intDic = new Dictionary<string, int>();

                if (intDic.ContainsKey(key))
                    return false;

                intDic.Add(key, data);
                return true;
            }

            bool Add(string key, long data)
            {
                if (longDic == null)
                    longDic = new Dictionary<string, long>();

                if (longDic.ContainsKey(key))
                    return false;

                longDic.Add(key, data);
                return true;
            }

            bool Add(string key, float data)
            {
                if (floatDic == null)
                    floatDic = new Dictionary<string, float>();

                if (floatDic.ContainsKey(key))
                    return false;

                floatDic.Add(key, data);
                return true;
            }

            bool Add(string key, double data)
            {
                if (doubleDic == null)
                    doubleDic = new Dictionary<string, double>();

                if (doubleDic.ContainsKey(key))
                    return false;

                doubleDic.Add(key, data);
                return true;
            }

            bool Add(string key, Vector2 data)
            {
                if (vector2Dic == null)
                    vector2Dic = new Dictionary<string, Vector2>();

                if (vector2Dic.ContainsKey(key))
                    return false;

                vector2Dic.Add(key, data);
                return true;
            }

            bool Add(string key, Vector3 data)
            {
                if (vector3Dic == null)
                    vector3Dic = new Dictionary<string, Vector3>();

                if (vector3Dic.ContainsKey(key))
                    return false;

                vector3Dic.Add(key, data);
                return true;
            }

            bool Add(string key, object data)
            {
                if (objectDic == null)
                    objectDic = new Dictionary<string, object>();

                if (objectDic.ContainsKey(key))
                    return false;

                objectDic.Add(key, data);
                return true;
            }
        }

        public class EventHandler<GameEventType>
        {
            private Queue<GameEvent<GameEventType>> eventQueue;
            private Dictionary<GameEventType, List<IEventSubscriber<GameEventType>>> eventSubscriberDic;
            
            public EventHandler()
            {
                eventQueue = new Queue<GameEvent<GameEventType>>();
                eventSubscriberDic = new Dictionary<GameEventType, List<IEventSubscriber<GameEventType>>>();
            }

            public void Publish(GameEvent<GameEventType> inEvent)
            {
                eventQueue.Enqueue(inEvent);
            }

            public void FireEvent()
            {
                GameEvent<GameEventType> eventData = eventQueue.Dequeue();
                List<IEventSubscriber<GameEventType>> subscriberList;
                if (eventSubscriberDic.TryGetValue(eventData.Type, out subscriberList))
                {
                    subscriberList.ForEach(x => x.Receive(eventData));
                }
            }

            public bool Subscribe(GameEventType inType, IEventSubscriber<GameEventType> inSubscriber)
            {
                List<IEventSubscriber<GameEventType>> subscriberList;
                if(eventSubscriberDic.TryGetValue(inType, out subscriberList))
                {
                    if(subscriberList.Contains(inSubscriber))
                    {
                        Debug.LogError("EventHandler already contains subscriber! : " + inSubscriber);
                        return false;
                    }

                    subscriberList.Add(inSubscriber);
                }
                else
                {
                    subscriberList = new List<IEventSubscriber<GameEventType>>();
                    subscriberList.Add(inSubscriber);
                    eventSubscriberDic.Add(inType, subscriberList);
                }

                return true;
            }
        }
    }
}


