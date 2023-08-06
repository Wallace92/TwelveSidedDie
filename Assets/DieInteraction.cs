using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieInteraction : MonoBehaviour
{
    private TwelveSideDieController m_twelveSideDieController;
    private bool m_isHeld;
    private Vector3 m_startPosition;
    private Rigidbody m_rb;
    private bool m_isMouseOverDie;

    [SerializeField]
    private float m_throwForce = 100f;
    [SerializeField]
    private float m_minThrowVelocity = 1f;
    
   // private void 

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }

        if (Input.GetMouseButtonUp(0) && m_isHeld)
        {
            HandleMouseUp();
        }

        if (m_isHeld)
        {
            HandleMouseDrag();
        }
    }

    private void HandleMouseDown()
    {
        if (m_isMouseOverDie)
        {
            m_isHeld = true;
            m_rb.isKinematic = true;
            m_startPosition = transform.position;
        }
    }
    
    private void StartRollDie()
    {
        // var forceDir = Vector3.right - Vector3.up;
        // var torqueVector = Vector3.Cross(forceDir.normalized, Vector3.down) * m_torqueStrength;
        //
        // m_rigidbody.AddForce(forceDir * m_forceMagnitude, ForceMode.Impulse);
        // m_rigidbody.AddTorque(torqueVector);
        
    }

    private void HandleMouseUp()
    {
        m_isHeld = false;
        m_rb.isKinematic = false;

        Vector3 throwDirection = transform.position - m_startPosition;
        float throwSpeed = throwDirection.magnitude;

        if (throwSpeed > m_minThrowVelocity)
        {
            m_rb.AddForce(throwDirection * m_throwForce, ForceMode.Impulse);
        }
        else
        {
            ResetCubePosition();
        }
    }

    private void OnMouseEnter()
    {
        m_isMouseOverDie = true;
    }
    
    private void OnMouseExit()
    {
        m_isMouseOverDie = false;
    }

    private void HandleMouseDrag()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z);
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void ResetCubePosition()
    {
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
        transform.position = m_startPosition;
    }
}
