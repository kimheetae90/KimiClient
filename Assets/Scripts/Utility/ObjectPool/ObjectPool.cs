using System.Collections.Generic;
using UnityEngine;
using System;

namespace KimiClient
{
    namespace Utility
    {
        public class ObjectPool<Template> where Template : new()
        {
            protected List<Template> objectPool;
            protected Stack<Template> unUsePool;

            public int Count { get { return objectPool.Count; } }
            public int ActiveCount { get { return objectPool.Count - unUsePool.Count; } }
            public int DeactiveCount { get { return unUsePool.Count; } }

            public Action OnInitailize;
            public Action<Template> OnGet;
            public Action<Template> OnReturn;
            public Action<Template> OnCreate;

            public virtual void Initialize(int initSize = 0)
            {
                objectPool = new List<Template>();
                unUsePool = new Stack<Template>();

                if (OnInitailize != null)
                    OnInitailize();

                for (int i = 0; i < initSize; i++)
                {
                    CreateNewObject();
                }
            }

            public Template Get()
            {
                if (unUsePool.Count == 0)
                {
                    CreateNewObject();
                }

                Template getObject = unUsePool.Pop();
                if (OnGet != null)
                    OnGet(getObject);

                return getObject;
            }

            protected virtual void CreateNewObject()
            {
                Template newGameObject = new Template();
                objectPool.Add(newGameObject);
                unUsePool.Push(newGameObject);

                if (OnCreate != null)
                    OnCreate(newGameObject);
            }

            public void Return(Template inReturnObject)
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

            public void Each(Action<Template> func)
            {
                foreach (var node in objectPool)
                {
                    if (!unUsePool.Contains(node))
                    {
                        func(node);
                    }
                }
            }

            public void EachForUnUse(Action<Template> func)
            {
                foreach (var node in unUsePool)
                {
                    func(node);
                }
            }

            public void EachForAll(Action<Template> func)
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
                    if (!unUsePool.Contains(node))
                    {
                        Return(node);
                    }
                }
            }

            public virtual void Clear()
            {
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