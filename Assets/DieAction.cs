using System.Collections;
using UnityEngine;

public interface IDieAction
{
    public void Release(DieMoveData dieMoveData);
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
        m_rigidbody.isKinematic = false;
        
        var throwDirection = CalculateThrowDirection();
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
    
    private Vector3 CalculateThrowDirection()
    {
        var cameraBlueAxisOffset = Camera.main.WorldToScreenPoint(transform.position);
        var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraBlueAxisOffset.z);
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
}