using UnityEngine;
using System.IO;
using System.Text;

public class WorkSheetAnswer
{
    string[] answers;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public WorkSheetAnswer(WorkSheetData workSheetData)
    {
        answers = new string[workSheetData.questionList.Count];
    }

    /// <summary>
    /// 回答を格納する
    /// </summary>
    /// <param name="index">何問目か？</param>
    public void SetAnswer(int index, string answer)
    {
        answers[index] = answer;
    }

    public void SaveAnswer()
    {
        string saveText = "";
        for(int i = 0;i< answers.Length;i++)
        {
            saveText += answers[i];
            if (i < answers.Length-1) saveText += ',';
        }
        
        Save(saveText, "answerChikurinin");
    }

    void Save(string text, string fileName)
    {
        StreamWriter sw;
        FileInfo fi;
        fi = new FileInfo(Application.persistentDataPath + "/" + fileName + ".txt");
        sw = fi.AppendText();
        sw.WriteLine(text);
        sw.Flush();
        sw.Close();
    }

    public string GetAnswer(int index)
    {
        if(answers[index] == string.Empty)
        {
            return "";
        }

        return answers[index];
    }
}
