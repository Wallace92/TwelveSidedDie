using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TwelveSideDieController : MonoBehaviour
{
    public event Action OnStartMovement;
    public event Action OnStopMovement;
    
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
        OnStopMovement += m_dieScores.StopMovement;
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
        OnStopMovement -= m_dieScores.StopMovement;
    }
}