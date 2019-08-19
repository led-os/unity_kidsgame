using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor.Build.Reporting;

//Unity命令行模式，也能「日志实时输出」
//https://www.jianshu.com/p/bd97cb8042a9

//http://www.xuanyusong.com/archives/2418
public class BuildPlayer
{
    //得到工程中所有场景名称
    static string[] SCENES = FindEnabledEditorScenes();
    //一系列批量build的操作


    [MenuItem("Custom/Build Android")]
    static void PerformAndroidBuild()
    {
        Debug.Log("PerformAndroidBuild start");
        BulidTarget("UC", "Android");
        Debug.Log("PerformAndroidBuild end");
    }




    [MenuItem("Custom/Build iPhone")]
    static void PerformiPhoneQQBuild()
    {
        BulidTarget("QQ", "IOS");
    }

    //这里封装了一个简单的通用方法。
    static void BulidTarget(string name, string target)
    {
        string app_name = name;
        string target_dir = Application.dataPath + "/OutPut";
        string target_name = name + ".apk";
        BuildTargetGroup targetGroup = BuildTargetGroup.Android;
        BuildTarget buildTarget = BuildTarget.Android;
        string applicationPath = Application.dataPath.Replace("/Assets", "");

        if (target == "Android")
        {
            target_dir = applicationPath + "/OutPut";
            target_name = app_name + ".apk";
            target_name = "Android";
            targetGroup = BuildTargetGroup.Android;
        }
        if (target == "IOS")
        {
            target_dir = applicationPath + "/OutPut";
            target_name = app_name;
            target_name = "iOS";
            targetGroup = BuildTargetGroup.iOS;
            buildTarget = BuildTarget.iOS;
        }

        //每次build删除之前的残留
        if (Directory.Exists(target_dir))
        {
            if (File.Exists(target_name))
            {
                File.Delete(target_name);
            }
        }
        else
        {
            Directory.CreateDirectory(target_dir);
        }

        //==================这里是比较重要的东西=======================

        //PlayerSettings.applicationIdentifier = "com.moonma.kidsgame";
        PlayerSettings.bundleVersion = "v0.0.1";
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, "QQ");


        //==================这里是比较重要的东西=======================

        //开始Build场景，等待吧～
        GenericBuild(SCENES, target_dir + "/" + target_name, targetGroup,buildTarget, BuildOptions.None);

    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    //https://docs.unity3d.com/ScriptReference/BuildPipeline.BuildPlayer.html

    static void GenericBuild(string[] scenes, string target_dir, BuildTargetGroup targetGroup,BuildTarget build_target, BuildOptions build_options)
    {

        if (Directory.Exists(target_dir))
        {
            FileUtil.DeleteDir(target_dir);
        }
        else
        {
            Directory.CreateDirectory(target_dir);
        }

        EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup,build_target);
        // string res =
        BuildReport report = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);

        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
        // if (res.Length > 0)
        // {
        //     throw new Exception("BuildPlayer failure: " + res);
        // }
    }

}