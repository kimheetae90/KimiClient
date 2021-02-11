using System.Collections.Generic;
using UnityEngine;
using System;

namespace KimiClient
{
    namespace Utility
    {
        public class GameObjectPool : ObjectPool<GameObject>
        {
            private GameObject prefab;

            public void Initialize(GameObject inPrefab, int initSize = 0)
            {
                prefab = inPrefab;
                base.Initialize(initSize);
            }

            protected override void CreateNewObject()
            {
                GameObject newGameObject = GameObject.Instantiate(prefab);
                newGameObject.SetActive(false);
                objectPool.Add(newGameObject);
                unUsePool.Push(newGameObject);

                if (OnCreate != null)
                    OnCreate(newGameObject);
            }

            public override void Clear()
            {
                prefab = null;
                base.Clear();
            }
        }
    }
}