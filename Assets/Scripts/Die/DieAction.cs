using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DieAction: MonoBehaviour, IDieAction
{
    [SerializeField]
    private Button m_rollBtn;
    
    private Rigidbody m_rigidbody;
    
    private TwelveSideDieController m_twelveSideDieController;
    
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_twelveSideDieController = GetComponent<TwelveSideDieController>();
        
        m_rollBtn.onClick.AddListener(AutoRelease);
    }
    
    private void AutoRelease()
    {
        Release(m_twelveSideDieController.DieMovement.AutoMoveData.GetData());
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
            
            StartCoroutine(StartDieMovementCoroutine());
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
        
        var throwDirection = worldPos - transform.position;
        
        return throwDirection;
    }

    private IEnumerator StartDieMovementCoroutine()
    {
        yield return new WaitForFixedUpdate();

        m_twelveSideDieController.DieMovement.IsReleased = true;
        m_rollBtn.interactable = false;
        
        Cursor.visible = true;
    }

    private void ResetCubePosition(DieMoveData dieMoveData)
    {
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.angularVelocity = Vector3.zero;
        
        transform.position = dieMoveData.StartPosition;
        
        Cursor.visible = true;
    }

    public bool Take(LayerMask dieLayerMask)
    {
        RaycastHit hit  = PerformRaycastThroughDie(dieLayerMask);

        if (hit.collider == null) 
            return false;
        
        m_rigidbody.isKinematic = true;

        return true;
    }

    private RaycastHit PerformRaycastThroughDie(LayerMask dieLayerMask)
    {
        var mainCam = Camera.main;

        var screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCam.farClipPlane);
        var screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCam.nearClipPlane);

        var worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        var worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        Ray ray = new Ray(worldMousePosNear, worldMousePosFar - worldMousePosNear);

        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, dieLayerMask);
        
        return hit;
    }

    public void Hold()
    {
        var cameraBlueAxisOffset = Camera.main.WorldToScreenPoint(transform.position);
        var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraBlueAxisOffset.z);
        var worldPos = Camera.main.ScreenToWorldPoint(position);

        transform.position = new Vector3(worldPos.x, 2f, worldPos.z);
    }
    
    public void StopMovement()
    {
        m_rollBtn.interactable = true;
    }

    private void OnDestroy()
    {
        m_rollBtn.onClick.AddListener(AutoRelease);
    }
}