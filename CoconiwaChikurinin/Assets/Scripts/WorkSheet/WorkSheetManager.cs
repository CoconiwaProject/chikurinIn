using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkSheetManager : MonoBehaviour
{
    [SerializeField]
    Text questionText = null;
    [SerializeField]
    GameObject endButton = null;

    Text nextButton, backButton, titleText;


    //どの選択肢が採用されるかわからないので消すのが容易なように分ける
    GameObject choices_five = null;
    GameObject choices_four = null;
    GameObject choices_three = null;
    GameObject choices_two = null;

    //記述式用
    InputField inputField = null;

    Dropdown dropDown = null;

    [SerializeField]
    WorkSheetData workSheetData = null;

    //何問目か
    int currentQuestionIndex;
    MyQuestion currentQuestion;

    WorkSheetAnswer answer;

    public List<Toggle> toggleList = new List<Toggle>();

    void Awake()
    {
        choices_five = transform.Find("Choices_Five").gameObject;
        choices_four = transform.Find("Choices_Four").gameObject;
        choices_three = transform.Find("Choices_Three").gameObject;
        choices_two = transform.Find("Choices_Two").gameObject;
        inputField = transform.Find("InputField").GetComponent<InputField>();
        dropDown = transform.Find("Dropdown").GetComponent<Dropdown>();

        nextButton = transform.Find("NextButton").GetComponent<Text>();
        backButton = transform.Find("BackButton").GetComponent<Text>();
        titleText = transform.Find("TitleText").GetComponent<Text>();
    }

    public void Initialize()
    {
        backButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
        titleText.gameObject.SetActive(true);
        endButton.SetActive(false);

        answer = new WorkSheetAnswer(workSheetData);

        Load();

        //最初の質問をセットする

        SetQuestion(workSheetData.questionList[currentQuestionIndex]);
    }

    //中断されていた場合はロードする
    void Load()
    {
        int answerNum = PlayerPrefs.GetInt("currentQuestionIndex");
        if (answerNum == 0)
        {
            currentQuestionIndex = 0;
        }
        else
        {
            currentQuestionIndex = answerNum;
        }

        for (int i = 0; i < answerNum; i++)
        {
            workSheetData.questionList[i].isAnswered = true;
            answer.SetAnswer(i, PlayerPrefs.GetString("answer" + i.ToString()));
        }
        for (int i = answerNum; i < workSheetData.questionList.Count; i++)
        {
            workSheetData.questionList[i].isAnswered = false;
        }
    }

    public void Next()
    {
        string answerText = GetAnswer();
        if (answerText == "") return;
        currentQuestion.isAnswered = true;
        //回答を保存する
        answer.SetAnswer(currentQuestionIndex, answerText);
        PlayerPrefs.SetInt("currentQuestionIndex", currentQuestionIndex + 1);
        PlayerPrefs.SetString("answer" + currentQuestionIndex.ToString(), answerText);

        if (currentQuestionIndex + 1 >= workSheetData.questionList.Count)
        {
            //終了
            Stop();
            return;
        }

        //次の問題
        currentQuestionIndex++;
        SetQuestion(workSheetData.questionList[currentQuestionIndex]);
    }

    public void Back()
    {
        //前の問題
        currentQuestionIndex--;
        SetQuestion(workSheetData.questionList[currentQuestionIndex]);
    }

    void SetQuestion(MyQuestion question)
    {
        currentQuestion = question;
        questionText.text = currentQuestion.question;

        //解答欄を表示する
        ClearChoices();
        ShowChoices();
        titleText.text = "質問" + (currentQuestionIndex + 1).ToString();

        //ボタンのチェック

        //あるべき姿
        backButton.gameObject.SetActive(true);
        nextButton.text = "次へ";

        if (currentQuestionIndex + 1 >= workSheetData.questionList.Count)
        {
            //最後の問題
            nextButton.text = "回答する";
        }
        if (currentQuestionIndex <= 0)
        {
            //最初の問題
            backButton.gameObject.SetActive(false);
        }
    }

    //解答欄を表示する
    void ShowChoices()
    {
        if (currentQuestion.m_type == QuestionType.Writing)
        {
            if (currentQuestion.isAnswered) inputField.text = answer.GetAnswer(currentQuestionIndex);
            else inputField.text = "";
            inputField.gameObject.SetActive(true);
            return;
        }
        if (currentQuestion.m_type == QuestionType.DropDown)
        {
            if (currentQuestion.isAnswered) dropDown.value = int.Parse(answer.GetAnswer(currentQuestionIndex));
            else dropDown.value = 0;
            dropDown.gameObject.SetActive(true);
            SetDropDown();
        }
        if (currentQuestion.m_type == QuestionType.Choices_Five)
            ShowTggle(choices_five);
        if (currentQuestion.m_type == QuestionType.Choices_Four)
            ShowTggle(choices_four);
        if (currentQuestion.m_type == QuestionType.Choices_Three)
            ShowTggle(choices_three);
        if (currentQuestion.m_type == QuestionType.Choices_Two)
            ShowTggle(choices_two);
    }

    void ShowTggle(GameObject choices)
    {
        choices.SetActive(true);
        toggleList.AddRange(choices.GetComponentsInChildren<Toggle>());

        for (int i = 0; i < toggleList.Count; i++)
        {
            //指定がないと解釈する
            if (currentQuestion.choiceArray[i] == "")
            {
                currentQuestion.choiceArray[i] = toggleList[i].GetComponentInChildren<Text>().text;
                continue;
            }
            //トグルのラベルにセットする
            toggleList[i].GetComponentInChildren<Text>().text = currentQuestion.choiceArray[i];
        }

        if (!currentQuestion.isAnswered) return;
        for (int i = 0; i < toggleList.Count; i++)
        {
            if (toggleList[i].GetComponentInChildren<Text>().text == answer.GetAnswer(currentQuestionIndex))
            {
                toggleList[i].isOn = true;
            }
        }
    }

    void SetDropDown()
    {
        dropDown.ClearOptions();
        List<Dropdown.OptionData> tempList = new List<Dropdown.OptionData>();
        for (int i = 0; i < currentQuestion.choiceArray.Length; i++)
        {
            Dropdown.OptionData data = new Dropdown.OptionData(currentQuestion.choiceArray[i]);
            tempList.Add(data);
        }

        dropDown.AddOptions(tempList);
    }

    //解答欄を消す
    void ClearChoices()
    {
        for (int i = 0; i < toggleList.Count; i++)
        {
            toggleList[i].isOn = false;
        }
        toggleList.Clear();

        choices_five.SetActive(false);
        choices_four.SetActive(false);
        choices_three.SetActive(false);
        choices_two.SetActive(false);
        inputField.gameObject.SetActive(false);
        dropDown.gameObject.SetActive(false);
    }

    string GetAnswer()
    {
        if (currentQuestion.m_type == QuestionType.Writing)
        {
            if (inputField.text == "") return "　";
            return inputField.text;
        }
        else if (currentQuestion.m_type == QuestionType.DropDown)
        {
            return dropDown.value.ToString();
        }
        else
        {
            for (int i = 0; i < toggleList.Count; i++)
            {
                if (toggleList[i].isOn) return toggleList[i].gameObject.name;
            }
        }

        //回答したことにはならない
        return "";
    }

    //すべて回答されたら保存する
    void Stop()
    {
        backButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
        questionText.text = "回答ありがとうございました。";
        ClearChoices();
        answer.SaveAnswer();
        endButton.SetActive(true);

        //PlayerPrefsは消す
        PlayerPrefs.DeleteKey("currentQuestionIndex");
        for(int i = 0;i < workSheetData.questionList.Count;i++)
        {
            PlayerPrefs.DeleteKey("answer" + i.ToString());
        }
    }
}
