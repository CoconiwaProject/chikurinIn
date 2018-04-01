public enum QuestionType { Writing, DropDown, Choices_Five = 5, Choices_Four = 4, Choices_Three = 3, Choices_Two = 2 }

[System.Serializable]
public class MyQuestion
{
    /// <summary>
    /// 質問の内容
    /// </summary>
    public string question = "？";
    public QuestionType m_type;
    //解答済みか
    public bool isAnswered = false;
    /// <summary>
    /// 選択式の場合の選択肢
    /// </summary>
    public string[] choiceArray;
}
