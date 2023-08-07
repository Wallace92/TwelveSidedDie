using System.Collections;
using UnityEngine;

public class DieMovement : MonoBehaviour
{
    [SerializeField]
    private DieMoveData m_dieMoveData;

    private TwelveSideDieController m_twelveSideDieController;

    private Vector3 m_startPosition;
    private Rigidbody m_rigidbody;
    
    private bool m_isHeld;
    private bool m_isReleased;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_twelveSideDieController = GetComponent<TwelveSideDieController>();
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

    private void FixedUpdate()
    {
        if (!m_isReleased) 
            return;
        
        if (m_rigidbody.velocity.sqrMagnitude < 0.0001f && m_rigidbody.angularVelocity.sqrMagnitude < 0.0001f)
        {
            m_isReleased = false;
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
            m_startPosition = transform.position;
            Cursor.visible = false;
        }
    }

    private void HandleMouseUp()
    {
        m_isHeld = false;
        m_rigidbody.isKinematic = false;
        
        var throwDirection = CalculateThrowDirection();
        var torqueVector = Vector3.Cross(throwDirection.normalized, Vector3.down) * m_dieMoveData.TorqueStrength;

        if (throwDirection.normalized.magnitude > m_dieMoveData.MinThrowVelocity)
        {
            m_rigidbody.AddForce(throwDirection *  throwDirection.normalized.magnitude * m_dieMoveData.ForceMagnitude, ForceMode.Impulse);
            m_rigidbody.AddTorque(torqueVector);
            
            m_twelveSideDieController.StartRollDieMovement();
            
            StartCoroutine(StopDieMovementCoroutine());
        }
        else
        {
            ResetCubePosition();
        }
    }

    private IEnumerator StopDieMovementCoroutine()
    {
        yield return new WaitForFixedUpdate();

        m_isReleased = true;
        Cursor.visible = true;
    }

    private Vector3 CalculateThrowDirection()
    {
        var cameraBlueAxisOffset = Camera.main.WorldToScreenPoint(transform.position);
        var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraBlueAxisOffset.z);
        var worldPos = Camera.main.ScreenToWorldPoint(position);
        
        Vector3 throwDirection = worldPos - transform.position;
        
        return throwDirection;
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
    
    private void ResetCubePosition()
    {
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.angularVelocity = Vector3.zero;
        transform.position = m_startPosition;
        Cursor.visible = true;
    }
}
