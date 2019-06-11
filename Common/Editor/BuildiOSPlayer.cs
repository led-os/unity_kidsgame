#if UNITY_IOS

using UnityEngine;

using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using UnityEditor.iOS.Xcode;
using System.Collections.Generic;
using System.Text.RegularExpressions;
//Unity iOS 一键打包 https://www.jianshu.com/p/f347e00abb9c
//https://www.cnblogs.com/laugher/p/6951232.html
public static class BuildiOSPlayer
{
    ////该属性是在build完成后，被调用的callback
    [PostProcessBuild]
    static void OnPostProcessBuild (BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log("BuildiOSPlayer:"+pathToBuiltProject);

        EditProj(pathToBuiltProject);
        //EditInfoPlist(pathToBuiltProject);
        //EditUnityAppController(pathToBuiltProject);
    }

   //添加lib方法
    static void AddLibToProject(PBXProject inst, string targetGuid, string lib)
    {
        string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
        inst.AddFileToBuild(targetGuid, fileGuid);
    }

  static void AddFileToProject(string projPath,PBXProject inst, string targetGuid, string filepath)
    {
        string fileGuid = inst.AddFile(filepath, filepath, PBXSourceTree.Source);
        inst.AddFileToBuild(targetGuid, fileGuid);
    }

    static void EditProj(string pathToBuiltProject)
    {
        string projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";

        PBXProject pbxProj = new PBXProject();
        pbxProj.ReadFromFile(projPath);

        string targetGuid = pbxProj.TargetGuidByName("Unity-iPhone");
        //string debugConfig = pbxProj.BuildConfigByName(target, "Debug");
        //string releaseConfig = pbxProj.BuildConfigByName(target, "Release");
        //pbxProj.SetBuildProperty(targetGuid, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
        //pbxProj.SetBuildPropertyForConfig(debugConfig, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
        //pbxProj.SetBuildPropertyForConfig(releaseConfig, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");

        pbxProj.AddFrameworkToProject(targetGuid, "AdSupport.framework", false);
        pbxProj.AddFrameworkToProject(targetGuid, "StoreKit.framework", false);
        pbxProj.AddFrameworkToProject(targetGuid, "CoreLocation.framework", false);
        pbxProj.AddFrameworkToProject(targetGuid, "CoreTelephony.framework", false);
        pbxProj.AddFrameworkToProject(targetGuid, "WebKit.framework", false);
   
        //添加lib
        AddLibToProject(pbxProj, targetGuid, "libz.tbd"); 
        AddLibToProject(pbxProj, targetGuid, "libxml2.tbd");

//多国语言
        AddFileToProject(projPath,pbxProj, targetGuid, "appname/en.lproj/InfoPlist.strings");
        AddFileToProject(projPath,pbxProj, targetGuid, "appname/zh-Hans.lproj/InfoPlist.strings");

        //pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("usr/lib/libsqlite3.dylib", "Frameworks/libsqlite3.dylib", PBXSourceTree.Sdk));
        //pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("usr/lib/libz.dylib", "Frameworks/libz.dylib", PBXSourceTree.Sdk));

        //pbxProj.SetBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(SRCROOT)/Frameworks");
        //pbxProj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");

       

        // 添加flag
        pbxProj.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");

        // 打开选项
        //pbxProj.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "YES");
        pbxProj.SetBuildProperty(targetGuid, "CLANG_ENABLE_MODULES", "YES");

        //teamid 
         pbxProj.SetTeamId(targetGuid, "Y9ZUK2WTEE");
        
#region 添加资源文件(中文路径 会导致 project.pbxproj 解析失败)
        // string frameworksPath = Application.dataPath + "/Frameworks";
        // string[] directories = Directory.GetDirectories(frameworksPath, "*", SearchOption.TopDirectoryOnly);
        // for (int i = 0; i < directories.Length; i++)
        // {
        //     string path = directories[i];

        //     string name = path.Replace(frameworksPath + "/", "");
        //     string destDirName = pathToBuiltProject + "/" + name;

        //     if (Directory.Exists(destDirName))
        //         Directory.Delete(destDirName, true);

        //     Debug.Log(path + " => " + destDirName);
        //     Utility.CopyDirectory(path, destDirName, new string[] { ".meta", ".framework", ".mm", ".c", ".m", ".h", ".xib", ".a", ".plist", ".org", "" }, false);

        //     foreach (string file in Directory.GetFiles(destDirName, "*.*", SearchOption.AllDirectories))
        //         pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile(file, file.Replace(pathToBuiltProject + "/", ""), PBXSourceTree.Source));
        // }
#endregion

        pbxProj.WriteToFile(projPath);
    }

    static void EditInfoPlist(string filePath)
    {
//         string path = filePath + "/Info.plist";

//         PlistDocument plistDocument = new PlistDocument();
//         plistDocument.ReadFromFile(path);

//         PlistElementDict dict = plistDocument.root.AsDict();

//         PlistElementArray array = dict.CreateArray("CFBundleURLTypes");
//         PlistElementDict dict2 = array.AddDict();
//         dict2.SetString("CFBundleURLName", PlayerSettings.bundleIdentifier);
//         PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
//         array2.AddString(PlayerSettings.bundleIdentifier);

//         dict2 = array.AddDict();
//         dict2.SetString("CFBundleURLName", "weixin");
//         array2 = dict2.CreateArray("CFBundleURLSchemes");
//         array2.AddString(BabybusConst.WEIXIN_ID);

//         dict2 = array.AddDict();
//         dict2.SetString("CFBundleURLName", "");
//         array2 = dict2.CreateArray("CFBundleURLSchemes");
//         array2.AddString("QQ" + BabybusConst.QQ_ID.ToString("X"));

//         dict2 = array.AddDict();
//         dict2.SetString("CFBundleURLName", "");
//         array2 = dict2.CreateArray("CFBundleURLSchemes");
//         array2.AddString("tencent" + BabybusConst.QQ_ID);


// #region quick action
//         string[] quickActions = { "Poem", "Pet", "Movie", "Telephone" };
//         string[] quickActionsIcon = { "PoemIcon", "PetIcon", "MovieIcon", "TelephoneIcon" };
//         //string[] icons = { "UIApplicationShortcutIconTypeBookmark", "UIApplicationShortcutIconTypeLove", "UIApplicationShortcutIconTypeCaptureVideo", "UIApplicationShortcutIconTypeFavorite" };
//         array = dict.CreateArray("UIApplicationShortcutItems");
//         for(int i=0; i<quickActions.Length; ++i)
//         {
//             dict2 = array.AddDict();
//             //dict2.SetString("UIApplicationShortcutItemIconType", icons[i]);
//             dict2.SetString("UIApplicationShortcutItemIconFile", quickActionsIcon[i]);
//             dict2.SetString("UIApplicationShortcutItemTitle", quickActions[i] + "Title");
//             dict2.SetString("UIApplicationShortcutItemType", quickActions[i]);
//             dict2.CreateDict("UIApplicationShortcutItemUserInfo");
//             //dict2.SetString("UIApplicationShortcutItemSubtitle", quickActions[i]);
//         }
// #endregion

//         dict.SetString("CFBundleIdentifier", PlayerSettings.bundleIdentifier);

//         var assetInfos = Utility.DeserializeXmlFromFile<List<AssetInfo>>(Application.dataPath + "/Resources/配置/APP.xml");
//         array = dict.CreateArray("LSApplicationQueriesSchemes");
//         foreach (var assetInfo in assetInfos)
//         {
//             if (string.IsNullOrEmpty(assetInfo.bundleIdentifier4iOS))
//                 array.AddString(assetInfo.extra);
//             else
//                 array.AddString(assetInfo.bundleIdentifier4iOS);
//         }

//         plistDocument.WriteToFile(path);
    }

    static void EditUnityAppController(string pathToBuiltProject)
    {
        // string unityAppControllerPath = pathToBuiltProject + "/Classes/UnityAppController.mm";
        // if (File.Exists(unityAppControllerPath))
        // {
        //     string headerCode = "#include \"../Libraries/Plugins/iOS/SDKPlatformIOS.h\"\n" +
        //                         "#import <AVFoundation/AVAudioSession.h>\n\n";
        //     string unityAppController = headerCode + File.ReadAllText(unityAppControllerPath);

        //     Match match = Regex.Match(unityAppController, @"- \(void\)startUnity:\(UIApplication\*\)application\s+\{[^}]+\}");
        //     if(match.Success)
        //     {
        //         string newCode = match.Groups[0].Value.Remove(match.Groups[0].Value.Length - 1);
        //         newCode += "\n" +
        //                    "    [[AVAudioSession sharedInstance] setCategory: AVAudioSessionCategoryPlayback error: nil];\n" +
        //                    "    [[AVAudioSession sharedInstance] setActive:YES error:nil];\n" +
        //                    "}\n\n" +
        //                    "- (void)application:(UIApplication*)application performActionForShortcutItem: (UIApplicationShortcutItem*)shortcutItem completionHandler: (void(^)(BOOL))completionHandler\n" +
        //                    "{\n" +
        //                    "    [[SDKPlatform share] performActionForShortcutItem:shortcutItem];\n" +
        //                    "}";
        //         unityAppController = unityAppController.Replace(match.Groups[0].Value, newCode);
        //     }

        //     File.WriteAllText(unityAppControllerPath, unityAppController);
        // }
    }
}

#endif