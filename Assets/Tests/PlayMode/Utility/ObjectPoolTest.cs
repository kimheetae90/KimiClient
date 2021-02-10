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

        [UnityTest, Order(1)]
        public IEnumerator Initialize()
        {
            TestRunnerHelper.testBoolean = false;

            config = TestRunnerHelper.GetTestRunnerConfig(ETestRunnerConfigType.ObjectPool) as ObjectPoolTestRunnerConfig;
            objectPool = new ObjectPool();
            Action onInitialize = () => { TestRunnerHelper.testBoolean = true; };
            objectPool.OnInitailize = onInitialize;
            objectPool.Initialize(config.initialize_prefab, config.initialize_initializeCount);

            Assert.AreEqual(TestRunnerHelper.testBoolean, true);
            Assert.AreEqual(config.initialize_initializeCount, objectPool.Count);

            yield return null;
        }
    }
}
