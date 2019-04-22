using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDateText : MonoBehaviour
{
    [SerializeField]
    Text complete = null;

    [SerializeField]
    Text reset = null;

    [SerializeField]
    Text cancel = null;

    [SerializeField]
    Text completeQuestion = null;

    [SerializeField]
    Text[] yes = new Text[2];

    [SerializeField]
    Text[] no = new Text[2];

    [SerializeField]
    Text completeResult = null;

    [SerializeField]
    Text resetQuestion = null;

    [SerializeField]
    Text resetResult = null;


    void Awake()
    {
        if (AppData.UsedLanguage == SystemLanguage.Japanese)
            return;
        complete.text = "Complete";
        reset.text = "Reset";
        cancel.text = "Cancel";
        completeQuestion.text = "Do you want to complete?";
        yes[0].text = yes[1].text = "Yes";
        no[0].text = no[1].text = "No";
        completeResult.text = "Completed";
        resetQuestion.text = "Do you want to reset?";
        resetResult.text = "Reset";
    }
}
