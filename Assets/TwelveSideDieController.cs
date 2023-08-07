using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct DieData
{
    public float NumberFontSize;
    public float NumberAlignment;

    public DieData(float numberFontSize, float numberAlignment)
    {
        NumberFontSize = numberFontSize;
        NumberAlignment = numberAlignment;
    }
}

public class TwelveSideDieController : MonoBehaviour
{
    private List<OneSideDie> m_oneSideDices;
    
    private bool m_startMovement;

    [SerializeField]
    private DieData m_dieData;
    
    public event Action OnStartMovement;
    public event Action OnStopMovement;
    
    private void Awake()
    {
        GetComponents();
        AttachEvents();
    }

    private void GetComponents()
    {
        m_oneSideDices = new List<OneSideDie>(gameObject.GetComponentsInChildren<OneSideDie>()
            .ToList());
    }

    private void AttachEvents()
    {
        OnStartMovement += StartMovement;
        OnStopMovement += StopMovement;
    }

    private void Start()
    {
        SetNumbers();
    }
    private void SetNumbers()
    {
        m_oneSideDices.ForEach(oneSideDice => oneSideDice.Init(m_dieData));
    }

    private void StartMovement()
    {
        Debug.Log("Start");
    }

    private void StopMovement()
    {
        Debug.Log("Stop");
        
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.up, 4.0f);

        // Sort the raycast hits based on distance from the starting position of the raycast.
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

        if (hits.Length > 0)
        {
            GameObject topFace = hits[0].collider.gameObject;
            
            if (topFace.GetComponent<OneSideDie>() == null)
                return;
            
            int topFaceNumber = topFace.GetComponent<OneSideDie>().Number;
            
            Debug.Log("The number on the top face is: " + topFaceNumber);
        }
    }

    public void StartRollDieMovement()
    {
        m_startMovement = true;
        OnStartMovement?.Invoke();
    }

    private void OnDestroy()
    {
        DetachEvents();
    }

    private void DetachEvents()
    {
        OnStartMovement -= StartMovement;
        OnStopMovement -= StopMovement;
    }

    public void StopRollDieMovement()
    {
        m_startMovement = false;
        
        OnStopMovement?.Invoke();
    }
}