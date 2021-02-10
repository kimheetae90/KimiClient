using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class TestRunnerHelper
{
    public static bool testBoolean = false;
    public static string testString;
    public static GameObject testGameObject = null;

    public static string testRunnerConfigPathInfo_path = "Assets/Tests/Config/TestRunnerConfigPathInfo.asset";

    public static TestRunnerConfig GetTestRunnerConfig(ETestRunnerConfigType inType)
    {
        var pathInfo = AssetDatabase.LoadAssetAtPath<TestRunnerConfigPathInfo>(testRunnerConfigPathInfo_path);
        TestRunnerKeyValue info = pathInfo.testRunnerConfigPathList.Find(x => x.type == inType);
        return AssetDatabase.LoadAssetAtPath<TestRunnerConfig>(info.path);
    }
}
