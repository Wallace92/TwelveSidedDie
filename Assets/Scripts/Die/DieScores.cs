using UnityEngine;

public class DieScores : MonoBehaviour
{
    [SerializeField]
    private ScorePresenter m_scorePresenter;
    
    [SerializeField]
    private string m_busySign;
    
    private TwelveSideDieController m_twelveSideDieController;

    private void Awake()
    {
        m_twelveSideDieController = GetComponent<TwelveSideDieController>();
    }
    
    public void StopMovement()
    {
        if (!Physics.Raycast(m_twelveSideDieController.transform.position, Vector3.up, out RaycastHit hit,4.0f)) 
            return;
        
        var topFace = hit.collider.gameObject;
            
        if (topFace.GetComponent<OneSideDie>() == null)
            return;
            
        var topFaceNumber = topFace.GetComponent<OneSideDie>().Number;
            
        m_scorePresenter.IncreaseScore(topFaceNumber);
        m_scorePresenter.SetResult(topFaceNumber.ToString());

    }

    public void StartMovement()
    {
        m_scorePresenter.SetResult(m_busySign);
    }
}