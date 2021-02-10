using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETestRunnerConfigType
{
    ObjectPool,
}

public class TestRunnerConfig : ScriptableObject
{
    public ETestRunnerConfigType Type;
}