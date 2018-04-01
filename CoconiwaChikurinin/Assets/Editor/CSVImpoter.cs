using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class CSVImpoter : AssetPostprocessor
{

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {

        string targetFile = "Assets/CSV/Japanese.csv";
        string exportFile = "Assets/Resources/ContentsData/Japanese.asset";
       // 現在のデータだと余計なものも入ってしまう為コメントアウト

        foreach (string asset in importedAssets)
        {
            // 合致しないものはスルー
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

                // ヘッダをやり過ごす
                sr.ReadLine();

                // ファイルの終端まで繰り返す
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] dataStrs = line.Split(',');

                    // 追加するパラメータを生成
                    ContentsData.Params p = new ContentsData.Params();
                    // 値を設定する
                    p.FileID = dataStrs[0];
                    p.ContentsName = dataStrs[1];
                    p.ContentsText = dataStrs[2];

                    ////p.FileID = dataStrs[1];
                    ////p.ContentsName = dataStrs[2];
                    ////p.ContentsText = dataStrs[5];

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

            Debug.Log("Data updated.");
        }
    }
}