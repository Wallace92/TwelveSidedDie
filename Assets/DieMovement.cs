using System;
using UnityEngine;

public class DieMovement : MonoBehaviour
{
    [SerializeField]
    private DieMoveData m_dieMoveData;

    private TwelveSideDieController m_twelveSideDieController;
    private IDieAction m_dieAction;
    private Rigidbody m_rigidbody;

    public bool IsReleased { get; set; }

    private bool m_isHeld;

    private void Awake()
    {
        m_dieAction = GetComponent<DieAction>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_twelveSideDieController = GetComponent<TwelveSideDieController>();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
            HandleMouseDown();

        if (Input.GetMouseButtonUp(0) && m_isHeld) 
            HandleMouseUp();

        if (m_isHeld) 
            HandleMouseDrag();
    }

    private void FixedUpdate()
    {
        if (!IsReleased) 
            return;
        
        if (m_rigidbody.velocity.sqrMagnitude < 0.0001f && m_rigidbody.angularVelocity.sqrMagnitude < 0.0001f)
        {
            IsReleased = false;
            m_twelveSideDieController.StopRollDieMovement();
        }
    }

    private void HandleMouseDown()
    {
        RaycastHit hit  = PerformRaycastThroughDie();

        if (hit.collider != null)
        {
            m_isHeld = true;
            m_rigidbody.isKinematic = true;
            m_dieMoveData.StartPosition = transform.position;
            Cursor.visible = false;
        }
    }

    private void HandleMouseUp()
    {
        m_isHeld = false;
        m_dieAction.Release(m_dieMoveData);
    }
    
    private void HandleMouseDrag()
    {
        var cameraBlueAxisOffset = Camera.main.WorldToScreenPoint(transform.position);
        var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraBlueAxisOffset.z);
        var worldPos = Camera.main.ScreenToWorldPoint(position);

        transform.position = new Vector3(worldPos.x, 2f, worldPos.z);
    }

    private RaycastHit PerformRaycastThroughDie()
    {
        var mainCam = Camera.main;

        var screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCam.farClipPlane);
        var screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCam.nearClipPlane);

        var worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        var worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        Ray ray = new Ray(worldMousePosNear, worldMousePosFar - worldMousePosNear);

        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_dieMoveData.DieLayerMask);
        
        return hit;
    }
}
