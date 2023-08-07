using System.ComponentModel;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ScoreModel))]
public class ScorePresenter : Presenter<ScoreModel>
{
    [SerializeField]
    private TextMeshProUGUI m_totalScoreText;
    
    [SerializeField]
    private TextMeshProUGUI m_resultText;

    public void IncreaseScore(int amount) => Model.IncreaseScore(amount);
    public void SetResult(string value) => Model.SetResult(value);

    protected override void OnPropertyChange(object sender, PropertyChangedEventArgs e)
    {
        if (sender is not ScoreModel) 
            return;
        
        if (e.PropertyName == nameof(ScoreModel.TotalScore))
            UpdateTotalScoreView();
        
        if (e.PropertyName == nameof(ScoreModel.Result))
            UpdateResultView();
    }

    private void Start() => UpdateScoresView();

    private void UpdateScoresView()
    {
        m_totalScoreText.text = $"Total: {Model.TotalScore}";
        m_resultText.text = $"Result: {Model.Result}";
    }

    private void UpdateTotalScoreView() => m_totalScoreText.text = $"Total: {Model.TotalScore}";

    private void UpdateResultView() => m_resultText.text = $"Result: {Model.Result}";
}