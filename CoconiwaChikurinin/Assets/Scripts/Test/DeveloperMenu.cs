using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperMenu : MonoBehaviour
{
    ContentsData contentsData = null;

    [SerializeField]
    GameObject popUpMessage = null;
    Coroutine popUpCoroutine;

    [SerializeField]
    GameObject content = null;
    [SerializeField]
    GameObject developMenu = null;
    [SerializeField]
    GameObject workSheetManager = null;

    [SerializeField]
    GameObject firstWindow = null;

    [SerializeField]
    GameObject completeWindow = null;
    [SerializeField]
    GameObject resultCompleteWindow = null;


    [SerializeField]
    GameObject resetWindow = null;
    [SerializeField]
    GameObject resultResetWindow = null;

    GameObject nowDraw = null;

    private void Start()
    {
        nowDraw = firstWindow;
        contentsData = AppData.ContentsData;
    }

    public void Open()
    {
        developMenu.SetActive(true);
        content.SetActive(false);
    }

    public void Close()
    {
        nowDraw = firstWindow;
        developMenu.SetActive(false);
        content.SetActive(true);
        resetWindow.SetActive(false);
        resultResetWindow.SetActive(false);
        completeWindow.SetActive(false);
        resultCompleteWindow.SetActive(false);
        firstWindow.SetActive(true);
    }

    public void DeleteSaveData()
    {
        //PlayerPrefs.DeleteAll();
        for (int i = 0; i < contentsData.Elements.Count; i++)
        {
            char kind = contentsData.Elements[i].FileID[0];

            if (kind == 'H' || kind == 'N' || kind == 'Y' || kind == 'I')
            {
                PlayerPrefs.DeleteKey("GetContents" + contentsData.Elements[i].FileID);
            }
        }


        //if (popUpCoroutine != null)
        //{
        //    StopCoroutine(popUpCoroutine);
        //}
        //   popUpCoroutine = StartCoroutine(MessagePopUp("全てのデータを削除しました。", 1.0f));
    }

    public void SetCompleteSaveData()
    {
        for (int i = 0; i < contentsData.Elements.Count; i++)
        {
            char kind = contentsData.Elements[i].FileID[0];

            if (kind == 'H' || kind == 'N' || kind == 'Y' || kind == 'I')
            {
                PlayerPrefs.SetInt("GetContents" + contentsData.Elements[i].FileID, 1);
            }
        }
        //if (popUpCoroutine != null)
        //{
        //    StopCoroutine(popUpCoroutine);
        //}
        //  popUpCoroutine = StartCoroutine(MessagePopUp("全てのデータを取得しました。", 1.0f));
    }

    IEnumerator MessagePopUp(string message, float duration)
    {
        popUpMessage.SetActive(true);
        popUpMessage.GetComponentInChildren<Text>().text = message;
        yield return new WaitForSeconds(duration);
        popUpMessage.SetActive(false);

        popUpCoroutine = null;
    }

    public void StartWorkSheet()
    {
        content.gameObject.SetActive(false);
        workSheetManager.SetActive(true);
        workSheetManager.GetComponent<WorkSheetManager>().Initialize();
    }
    public void EndWorkSheet()
    {
        content.gameObject.SetActive(true);
        workSheetManager.SetActive(false);
    }

    public void ChangeNextWindow(GameObject newWindow)
    {
        nowDraw.SetActive(false);
        newWindow.SetActive(true);
        nowDraw = newWindow;
    }
}
