using System;
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
        RaycastHit[] hits = Physics.RaycastAll(m_twelveSideDieController.transform.position, Vector3.up, 4.0f);
        
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

        if (hits.Length > 0)
        {
            GameObject topFace = hits[0].collider.gameObject;
            
            if (topFace.GetComponent<OneSideDie>() == null)
                return;
            
            int topFaceNumber = topFace.GetComponent<OneSideDie>().Number;
            
            m_scorePresenter.IncreaseScore(topFaceNumber);
            m_scorePresenter.SetResult(topFaceNumber.ToString());
        }
    }

    public void StartMovement()
    {
        m_scorePresenter.SetResult(m_busySign);
    }
}