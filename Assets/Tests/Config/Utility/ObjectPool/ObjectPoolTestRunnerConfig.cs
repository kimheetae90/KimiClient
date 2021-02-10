using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPoolTestRunnerConfig", menuName = "TestRunner Config/ObjectPool")]
public class ObjectPoolTestRunnerConfig : TestRunnerConfig
{
    public int initialize_count = 3;
    public GameObject initialize_prefab = null;
}