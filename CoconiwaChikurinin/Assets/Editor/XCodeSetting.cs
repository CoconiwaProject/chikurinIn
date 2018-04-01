using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;

/// <summary>
/// XCodeビルド出力後に呼び出されるメソッド
/// XCode側で別途設定が必要な場合は、こちらで対応
/// </summary>
public class XCodeSetting
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
#if !UNITY_CLOUD_BUILD
        int i = int.Parse(PlayerSettings.iOS.buildNumber);
        i++;
        PlayerSettings.iOS.buildNumber = i.ToString();
#endif
    }
}