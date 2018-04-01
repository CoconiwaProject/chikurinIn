using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppData
{

    public static string SelectTargetName;
    public static Texture[] SelectThetaPictures;
    public static bool CanChangePicture;
    public static Vector3 differenceVec;

    public static SystemLanguage UsedLanguage;
    static ContentsData contentsData;
    public static ContentsData ContentsData
    {
        get
        {
            if (contentsData != null) return contentsData;

            contentsData = Resources.Load<ContentsData>("ContentsData/" + UsedLanguage.ToString());
            if(contentsData==null)
                contentsData = Resources.Load<ContentsData>("ContentsData/English");
            if (UsedLanguage != SystemLanguage.Japanese) FormatData();

            return contentsData;
        }
    }

    static void FormatData()
    {
        ContentsData japanData = Resources.Load<ContentsData>("ContentsData/" + SystemLanguage.Japanese.ToString());
        
        ContentsData.Params element;

        for (int i = contentsData.Elements.Count-1; i >= 0; i--)
        {
            if (japanData.ContentDictionary.TryGetValue(contentsData.Elements[i].FileID, out element)) continue;

            contentsData.RemoveContent(contentsData.Elements[i].FileID);
        }
    }
}
