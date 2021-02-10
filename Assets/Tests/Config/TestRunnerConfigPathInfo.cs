using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TestRunnerKeyValue
{
    public ETestRunnerConfigType type;
    public string path;
}

[CreateAssetMenu(fileName = "TestRunnerConfigPathInfo", menuName = "TestRunner Config Path Info")]
public class TestRunnerConfigPathInfo : ScriptableObject
{
    public List<TestRunnerKeyValue> testRunnerConfigPathList;
}