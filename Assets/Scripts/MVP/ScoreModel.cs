public class ScoreModel : Model
{
    private int m_totalScore;
    public int TotalScore
    {
        get => m_totalScore; 
        set => SetValue(value, ref m_totalScore);
    }
    
    private string m_result;
    public string Result
    {
        get => m_result; 
        set => SetValue(value, ref m_result);
    }

    public void IncreaseScore(int amount) => TotalScore += amount;
    public void SetResult(string value) => Result = value;
}