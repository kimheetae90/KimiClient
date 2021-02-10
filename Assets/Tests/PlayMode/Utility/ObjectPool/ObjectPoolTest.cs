using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using KimiClient.Utility;

namespace Tests
{
    public class ObjectPoolTest
    {
        ObjectPool objectPool;
        ObjectPoolTestRunnerConfig config;

        [UnityTest]
        public IEnumerator Initialize()
        {
            TestRunnerHelper.testBoolean = false;

            InitailizeObjectPool();

            Action onInitialize = () => { TestRunnerHelper.testBoolean = true; };
            objectPool.OnInitailize = onInitialize;
            objectPool.Initialize(config.initialize_prefab, config.initialize_count);

            Assert.AreEqual(TestRunnerHelper.testBoolean, true);
            Assert.AreEqual(config.initialize_count, objectPool.Count);

            objectPool.Clear();
            Assert.AreEqual(0, objectPool.Count);

            yield return null;
        }

        [UnityTest]
        public IEnumerator GetNReturn()
        {
            InitailizeObjectPool();

            objectPool.Initialize(config.initialize_prefab, config.initialize_count);
            GameObject poolObject = objectPool.Get();

            Assert.AreEqual(1, objectPool.ActiveCount);
            Assert.AreEqual(config.initialize_count - 1, objectPool.DeactiveCount);

            objectPool.Return(poolObject);

            Assert.AreEqual(config.initialize_count, objectPool.Count);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CreateNGetNReturnAction()
        {
            InitailizeObjectPool();

            Action<GameObject> onCreate = (x) => 
            {
                x.AddComponent<TestRunnerObject>(); 
            };
            Action<GameObject> onGet = (x) => 
            { 
                TestRunnerObject tro = x.GetComponent<TestRunnerObject>();
                tro.TestInt = 30;
            };
            Action<GameObject> onReturn = (x) => 
            {
                TestRunnerObject tro = x.GetComponent<TestRunnerObject>();
                tro.TestInt = -30;
            };

            objectPool.OnCreate = onCreate;
            objectPool.OnGet = onGet;
            objectPool.OnReturn = onReturn;

            objectPool.Initialize(config.initialize_prefab, config.initialize_count);

            GameObject poolObject = objectPool.Get();
            TestRunnerObject testRunnerObject = poolObject.GetComponent<TestRunnerObject>();
            Assert.IsTrue(testRunnerObject != null);
            Assert.AreEqual(testRunnerObject.TestInt, 30);

            objectPool.Return(poolObject);
            Assert.AreEqual(testRunnerObject.TestInt, -30);

            for(int i = 0; i< config.initialize_count + 1;i++)
            {
                objectPool.Get();
            }
            Assert.AreEqual(config.initialize_count + 1, objectPool.Count);

            yield return null;
        }

        [UnityTest]
        public IEnumerator ReturnAll()
        {
            InitailizeObjectPool();

            objectPool.Initialize(config.initialize_prefab, config.initialize_count);

            for(int i = 0 ; i < objectPool.Count;i++)
            {
                GameObject poolObject = objectPool.Get();
            }

            Assert.AreEqual(objectPool.Count, config.initialize_count);
            Assert.AreEqual(objectPool.DeactiveCount, 0);
            Assert.AreEqual(objectPool.ActiveCount, config.initialize_count);

            objectPool.ReturnAll();

            Assert.AreEqual(objectPool.Count, config.initialize_count);
            Assert.AreEqual(objectPool.DeactiveCount, config.initialize_count);
            Assert.AreEqual(objectPool.ActiveCount, 0);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Each()
        {
            InitailizeObjectPool();

            Action<GameObject> onCreate = (x) =>
            {
                TestRunnerObject tro = x.AddComponent<TestRunnerObject>();
                tro.TestInt = 10;
            };

            objectPool.OnCreate = onCreate;
            objectPool.Initialize(config.initialize_prefab, config.initialize_count);

            objectPool.Get();

            TestRunnerHelper.testInt = 0;
            objectPool.Each(x =>
            {
                TestRunnerObject tro = x.GetComponent<TestRunnerObject>();
                TestRunnerHelper.testInt += tro.TestInt;
            });
            Assert.AreEqual(TestRunnerHelper.testInt, 10);

            TestRunnerHelper.testInt = 0;
            objectPool.EachForUnUse(x =>
            {
                TestRunnerObject tro = x.GetComponent<TestRunnerObject>();
                TestRunnerHelper.testInt += tro.TestInt;
            });
            Assert.AreEqual(TestRunnerHelper.testInt, 20);

            TestRunnerHelper.testInt = 0;
            objectPool.EachForAll(x =>
            {
                TestRunnerObject tro = x.GetComponent<TestRunnerObject>();
                TestRunnerHelper.testInt += tro.TestInt;
            });
            Assert.AreEqual(TestRunnerHelper.testInt, 30);
            yield return null;
        }

        public void InitailizeObjectPool()
        {
            config = TestRunnerHelper.GetTestRunnerConfig(ETestRunnerConfigType.ObjectPool) as ObjectPoolTestRunnerConfig;
            objectPool = new ObjectPool();
        }
    }
}
