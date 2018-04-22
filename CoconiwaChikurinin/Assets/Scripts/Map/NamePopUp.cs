using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NamePopUp : MonoBehaviour
{
    string fileID = "";

    //Noneの場合はLの画像を使用する
    private enum MakerType { H, N, Y, None }
    public enum MakerSize { S, M, L, LL, None }

    void Start()
    {
    }

    public void SetText(string fileID)
    {
        this.fileID = fileID;
        SetPopUpTexture(GetMakerType(fileID), GetMakerSize(MapManager.I.GetContentName(fileID)));
        GetComponentInChildren<Text>().text = MapManager.I.GetContentName(fileID);
        //todo:テキストの長さに応じてサイズ変更
    }

    public void MapSceneLoad()
    {
        AppData.SelectTargetName = fileID;
        UnderBerMenu.I.ChangeScene("Content");
    }

    void SetPopUpTexture(MakerType type, MakerSize size)
    {
        //表示するテキストが存在しない
        if (size == MakerSize.None) return;
        if (size == MakerSize.LL)
        {
            size = MakerSize.L;
        }

        //Lに変換
        if (type == MakerType.None)
        {
            type = MakerType.Y;
        }
        int index = ((int)type * (int)MakerType.None) + (int)size;
        transform.GetChild(0).GetComponent<Image>().sprite = MapManager.I.balloonImageList[index];
    }

    MakerType GetMakerType(string fileID)
    {
        if (fileID[0] == 'N') return MakerType.N;
        else if (fileID[0] == 'Y') return MakerType.Y;
        else if (fileID[0] == 'H') return MakerType.H;

        return MakerType.None;
    }

    public static MakerSize GetMakerSize(string fileName)
    {
        Debug.Log("文字の長さ=" + fileName.Length);
        int strLength = fileName.Length;
        if (AppData.UsedLanguage == SystemLanguage.English)
            strLength /= 2;
        if (strLength > 20)
        {
            return MakerSize.LL;
        }
        else if (strLength > 7)
        {
            return MakerSize.L;
        }
        else if (strLength > 4)
        {
            return MakerSize.M;
        }
        else
        {
            return MakerSize.S;
        }
    }
}
