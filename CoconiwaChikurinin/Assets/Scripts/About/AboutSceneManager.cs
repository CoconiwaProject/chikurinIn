using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutSceneManager : MonoBehaviour
{
    [SerializeField]
    string[] fileIDList = null;

    ContentsData contentsData = null;

    [SerializeField]
    AboutItem itemPrefab = null;

    [SerializeField]
    Transform itemContainer = null;

    [SerializeField]
    DeveloperMenu developerMenu = null;

    [SerializeField]
    CreditController creditController = null;

    void Start()
    {
        contentsData = AppData.ContentsData;
        for(int i = 0;i< fileIDList.Length;i++)
        {
            AboutItem item = Instantiate(itemPrefab, itemContainer);
            item.Init(contentsData.ContentDictionary[fileIDList[i]].ContentsName, fileIDList[i]);
        }

        AboutItem credit = Instantiate(itemPrefab, itemContainer);
        credit.Init(AppData.UsedLanguage == SystemLanguage.Japanese ? "クレジット": "Credit",
            () =>
            {
                creditController.PushCredit();
            });


//#if DEVELOPMENT_BUILD || UNITY_EDITOR
//        AboutItem workSheet = Instantiate(itemPrefab, itemContainer);
//        workSheet.Init("アンケート回答",
//            () =>
//            {
//                developerMenu.StartWorkSheet();
//            });

        AboutItem developMenu = Instantiate(itemPrefab, itemContainer);

        developMenu.Init("木札が見つからない時は",
            () =>
            {
                developerMenu.Open();
            });
//#endif
    }
}
