using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TwelveSideDieController : MonoBehaviour
{
    [SerializeField]
    private float m_numberFontSize;
    
    private Rigidbody m_rigidbody;

    private List<OneSideDie> m_oneSideDices;
    
    private bool m_isMoved;
    private bool m_startMovement;
    
    [SerializeField]
    private float m_forceMagnitude;

    [SerializeField]
    private float m_torqueStrength;

    public event Action OnStartMovement;
    public event Action OnStopMovement;
    private void Awake()
    {
        GetComponents();
        AttachEvents();
    }

    private void GetComponents()
    {
        m_rigidbody = gameObject.GetComponent<Rigidbody>();
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

    private void Update()
    {
        if (!m_startMovement) 
            return;
        
        m_isMoved = m_rigidbody.velocity.magnitude != 0;

        if (m_isMoved) 
            return;
        
        m_startMovement = false;
        
        OnStopMovement?.Invoke();
    }

    private void SetNumbers()
    {
        m_oneSideDices.ForEach(oneSideDice => oneSideDice.Init(m_numberFontSize));
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
        System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

        if (hits.Length > 0)
        {
            GameObject topFace = hits[0].collider.gameObject;

            int topFaceNumber = topFace.GetComponent<OneSideDie>().Number;
            
            Debug.Log("The number on the top face is: " + topFaceNumber);
        }
    }

    public void StartRollDieMovement()
    {
        m_startMovement = true;
        OnStartMovement?.Invoke();
    }

    private void StartRollDie()
    {
        var forceDir = Vector3.right - Vector3.up;
        var torqueVector = Vector3.Cross(forceDir.normalized, Vector3.down) * m_torqueStrength;
        
        m_rigidbody.AddForce(forceDir * m_forceMagnitude, ForceMode.Impulse);
        m_rigidbody.AddTorque(torqueVector);
        
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
}