using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TwelveSideDieController : MonoBehaviour
{
    public event Action OnStartMovement;
    public event Action OnStopMovement;
    
    public DieMovement DieMovement => m_dieMovement;

    [SerializeField]
    private DieData m_dieData;
    
    private List<OneSideDie> m_oneSideDices;
    
    private DieAction m_dieAction;
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
        m_dieAction = gameObject.GetComponent<DieAction>();
        
        m_oneSideDices = new List<OneSideDie>(gameObject.GetComponentsInChildren<OneSideDie>()
            .ToList());
    }

    private void AttachEvents()
    {
        OnStartMovement += m_dieScores.StartMovement;
        OnStopMovement += m_dieScores.StopMovement;
        
        OnStopMovement += m_dieAction.StopMovement;
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
        OnStartMovement -= m_dieScores.StartMovement;
        OnStopMovement -= m_dieScores.StopMovement;
    }
}