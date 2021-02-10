using System.Collections.Generic;
using UnityEngine;
using System;

namespace KimiClient
{
    namespace Utility
    {
        public class ObjectPool
        {
            protected List<GameObject> objectPool;
            protected Stack<GameObject> unUsePool;

            public int Count { get { return objectPool.Count; } }
            public int ActiveCount { get { return objectPool.Count - unUsePool.Count; } }
            public int DeactiveCount { get { return unUsePool.Count; } }

            public Action OnInitailize;
            public Action<GameObject> OnGet;
            public Action<GameObject> OnReturn;
            public Action<GameObject> OnCreate;

            private GameObject prefab;

            public void Initialize(GameObject inPrefab, int initSize = 0)
            {
                objectPool = new List<GameObject>();
                unUsePool = new Stack<GameObject>();
                prefab = inPrefab;

                if (OnInitailize != null)
                    OnInitailize();

                for (int i = 0; i < initSize; i++)
                {
                    CreateNewObject();
                }
            }

            public GameObject Get()
            {
                if (unUsePool.Count == 0)
                {
                    CreateNewObject();
                }

                GameObject getObject = unUsePool.Pop();
                if (OnGet != null)
                    OnGet(getObject);

                return getObject;
            }

            protected virtual void CreateNewObject()
            {
                GameObject newGameObject = GameObject.Instantiate(prefab);
                newGameObject.SetActive(false);
                objectPool.Add(newGameObject);
                unUsePool.Push(newGameObject);

                if (OnCreate != null)
                    OnCreate(newGameObject);
            }

            public void Return(GameObject inReturnObject)
            {
                if (!objectPool.Contains(inReturnObject))
                {
                    Debug.LogError("Pool doesn't contain New Object!");
                    return;
                }

                if (OnReturn != null)
                    OnReturn(inReturnObject);

                unUsePool.Push(inReturnObject);
            }

            public void Each(Action<GameObject> func)
            {
                foreach (var node in objectPool)
                {
                    if(!unUsePool.Contains(node))
                    {
                        func(node);
                    }
                }
            }

            public void EachForUnUse(Action<GameObject> func)
            {
                foreach (var node in unUsePool)
                {
                    func(node);
                }
            }

            public void EachForAll(Action<GameObject> func)
            {
                foreach (var node in objectPool)
                {
                    func(node);
                }
            }

            public void ReturnAll()
            {
                foreach (var node in objectPool)
                {
                    if(!unUsePool.Contains(node))
                    {
                        Return(node);
                    }
                }
            }

            public void Clear()
            {
                prefab = null;

                OnInitailize = null;
                OnGet = null;
                OnReturn = null;
                OnCreate = null;

                objectPool.Clear();
                unUsePool.Clear();

            }
        }
    }
}