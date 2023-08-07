using System.Collections;
using UnityEngine;

public interface IDieAction
{
    public void Release(DieMoveData dieMoveData);
    public bool Take(DieMoveData dieMoveData);
    public void Hold();
}

public class DieAction: MonoBehaviour, IDieAction
{
    private Rigidbody m_rigidbody;
    
    private TwelveSideDieController m_twelveSideDieController;
    
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_twelveSideDieController = GetComponent<TwelveSideDieController>();
    }

    public void Release(DieMoveData dieMoveData)
    {
        if (dieMoveData.ThrowMode == ThrowMode.AUTO)
            transform.position = dieMoveData.StartPosition;
        
        m_rigidbody.isKinematic = false;
        
        var throwDirection = CalculateThrowDirection(dieMoveData.ThrowMode);
        var torqueVector = Vector3.Cross(throwDirection.normalized, Vector3.down) * dieMoveData.TorqueStrength;

        if (throwDirection.normalized.magnitude > dieMoveData.MinThrowVelocity)
        {
            m_rigidbody.AddForce(throwDirection *  throwDirection.normalized.magnitude * dieMoveData.ForceMagnitude, ForceMode.Impulse);
            m_rigidbody.AddTorque(torqueVector);
            
            m_twelveSideDieController.StartRollDieMovement();
            
            StartCoroutine(StopDieMovementCoroutine());
        }
        else
        {
            ResetCubePosition(dieMoveData);
        }
    }

    private Vector3 CalculateThrowDirection(ThrowMode throwMode)
    {
        var cameraBlueAxisOffset = Camera.main.WorldToScreenPoint(transform.position);
        
        var position = throwMode == ThrowMode.MANUAL 
            ? new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraBlueAxisOffset.z)
            : new Vector3(Random.Range(100, 1800), Random.Range(100, 1000), cameraBlueAxisOffset.z);
        
        var worldPos = Camera.main.ScreenToWorldPoint(position);
        
        Vector3 throwDirection = worldPos - transform.position;
        
        return throwDirection;
    }

    private IEnumerator StopDieMovementCoroutine()
    {
        yield return new WaitForFixedUpdate();

        m_twelveSideDieController.DieMovement.IsReleased = true;
        Cursor.visible = true;
    }

    private void ResetCubePosition(DieMoveData dieMoveData)
    {
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.angularVelocity = Vector3.zero;
        transform.position = dieMoveData.StartPosition;
        Cursor.visible = true;
    }

    public bool Take(DieMoveData dieMoveData)
    {
        RaycastHit hit  = PerformRaycastThroughDie(dieMoveData);

        if (hit.collider == null) 
            return false;
        
        m_rigidbody.isKinematic = true;

        return true;
    }

    private RaycastHit PerformRaycastThroughDie(DieMoveData dieMoveData)
    {
        var mainCam = Camera.main;

        var screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCam.farClipPlane);
        var screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCam.nearClipPlane);

        var worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        var worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        Ray ray = new Ray(worldMousePosNear, worldMousePosFar - worldMousePosNear);

        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, dieMoveData.DieLayerMask);
        
        return hit;
    }

    public void Hold()
    {
        var cameraBlueAxisOffset = Camera.main.WorldToScreenPoint(transform.position);
        var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraBlueAxisOffset.z);
        var worldPos = Camera.main.ScreenToWorldPoint(position);

        transform.position = new Vector3(worldPos.x, 2f, worldPos.z);
    }
}