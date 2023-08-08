using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TwelveSideDieController : MonoBehaviour
{
    public ScorePresenter ScorePresenter => m_scorePresenter;
    public event Action OnStartMovement;
    public event Action OnStopMovement;
    
    [SerializeField]
    private ScorePresenter m_scorePresenter;

    [SerializeField]
    private Button m_rollBtn;
    
    [SerializeField]
    private DieData m_dieData;

    public DieMovement DieMovement => m_dieMovement;
    
    private List<OneSideDie> m_oneSideDices;
    private DieScores m_dieScores;
    private DieMovement m_dieMovement;
    
    public void StartRollDieMovement() => OnStartMovement?.Invoke();

    public void StopRollDieMovement() => OnStopMovement?.Invoke();

    private void Awake()
    {
        GetComponents();
        AttachEvents();
    }

    private void GetComponents()
    {
        m_dieScores = gameObject.GetComponent<DieScores>();
        m_dieMovement = gameObject.GetComponent<DieMovement>();
        m_oneSideDices = new List<OneSideDie>(gameObject.GetComponentsInChildren<OneSideDie>()
            .ToList());
    }

    private void AttachEvents()
    {
        m_rollBtn.onClick.AddListener(AutoRelease);
        
        OnStartMovement += m_dieScores.StartMovement;
        OnStopMovement += m_dieScores.StopMovement;
    }

    private void AutoRelease()
    {
        m_dieMovement.DieAction.Release(m_dieMovement.AutoMoveData.GetData());
    }

    private void Start()
    {
        SetNumbers();
    }
    private void SetNumbers()
    {
        m_oneSideDices.ForEach(oneSideDice => oneSideDice.Init(m_dieData));
    }
    
    private void OnDestroy()
    {
        DetachEvents();
    }

    private void DetachEvents()
    {
        m_rollBtn.onClick.AddListener(AutoRelease);
        
        OnStartMovement -= m_dieScores.StartMovement;
        OnStopMovement -= m_dieScores.StopMovement;
    }
}