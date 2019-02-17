using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class JSONImpoter : AssetPostprocessor
{

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        string targetFile = "Assets/CSV/English.json";
        string exportFile = "Assets/Resources/ContentsData/English.asset";

        foreach (string asset in importedAssets)
        {
            //合致しないものはスルー
            if (!targetFile.Equals(asset)) continue;

            // 既存のマスタを取得
            ContentsData data = AssetDatabase.LoadAssetAtPath<ContentsData>(exportFile);

            // 見つからなければ作成する
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<ContentsData>();
                AssetDatabase.CreateAsset((ScriptableObject)data, exportFile);
                AssetDatabase.SaveAssets();
            }
            else
            {

                // 中身を削除
                data.Elements.Clear();
            }
            //変更ここから//
            AssetDatabase.StartAssetEditing();
            // CSVファイルをオブジェクトへ保存
            using (StreamReader sr = new StreamReader(targetFile))
            {
                string line = sr.ReadToEnd();

                string[] cutStr = { ":\"" };
                string[] dataStrs = line.Split(cutStr, System.StringSplitOptions.None);
                int nowCount = 1;

                char[] removeChar = new char[2]{' ','　' };
                // ファイルの終端まで繰り返す
                while (nowCount < dataStrs.Length)
                {
                    // 追加するパラメータを生成
                    ContentsData.Params p = new ContentsData.Params();
                    // 値を設定する
                    p.FileID = ChangeString(ReturnIntervalString(RemoveTopChar(dataStrs[nowCount], removeChar)));
                    nowCount++;
                    p.ContentsName = ChangeString(ReturnIntervalString(RemoveNumberChar(RemoveTopChar(dataStrs[nowCount], removeChar))));
                    nowCount++;
                    p.ContentsText = ChangeString(ReturnIntervalString(RemoveTopChar(dataStrs[nowCount], removeChar),"\"}"));
                    nowCount++;
                    // 追加
                    data.Elements.Add(p);
                }
            }

            //変更ここまで//
            AssetDatabase.StopAssetEditing();

            //変更をUnityEditorに伝える//
            EditorUtility.SetDirty(data);

            //すべてのアセットを保存//
            AssetDatabase.SaveAssets();

            Debug.Log("Json updated.");
        }
    }

    //先頭から指定の文字以外が見つかるまで削除
    static string RemoveTopChar(string originalString, char[] removeChar)
    {
        string returnString = originalString;
        bool isEnd = false;

        for (int i = 0; i < originalString.Length; i++)
        {
            for (int j = 0; j < removeChar.Length; j++)
            {
                if(removeChar[j]==originalString[i])
                {
                    returnString = returnString.Remove(0, 1);
                    break;
                }

                if(j==removeChar.Length-1)
                {
                    isEnd = true;
                }
            }
            if (isEnd == true)
                break;
        }
        return returnString;
    }
    //文字列に数字が入っていたら削除
    static string RemoveNumberChar(string originalString)
    {
        string returnString = "";

        char judgeChar;
        for(int i=0;i<originalString.Length;i++)
        {
            judgeChar = originalString[i];
            if(((judgeChar<='９' && originalString[i] >= '０')||
                (judgeChar <= '9' && originalString[i] >= '0'))==false)
            {
                returnString += originalString[i];
            }
        }
        return returnString;
    }




    static string ReturnIntervalString(string originalString,string sarchString= "\"")
    {
        string returnString = originalString; 
        int selectStrNum = returnString.IndexOf(sarchString);
        return returnString.Remove(selectStrNum);
    }
    static string ChangeString(string originalString)
    {
        string returnString = "";
        for (int i = 0; i < originalString.Length; i++)
        {
            switch (originalString[i])
            {
                case 'ｌ':
                    returnString += 'I';
                    break;
                case '－':
                    returnString += '-';
                    break;
                case 'ー':
                    returnString += '-';
                    break;
                default:
                    if (originalString[i] <= '９' && originalString[i] >= '０')
                    {
                        int interval = originalString[i] - '０';
                        returnString += interval.ToString();
                    }
                    else
                    {
                        returnString += originalString[i];
                    }
                    break;
            }
        }
        return returnString;
    }

}