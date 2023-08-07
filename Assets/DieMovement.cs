using System;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IMoveData
{
    public DieMoveData GetData();
}

[Serializable]
public struct DieMoveData
{
    public ThrowMode ThrowMode;
    public float ForceMagnitude;
    public float TorqueStrength;
    public float MinThrowVelocity;

    public LayerMask DieLayerMask;
    public Vector3 StartPosition;

    public DieMoveData(float forceMagnitude, 
        float torqueStrength, 
        float minThrowVelocity, 
        Vector3 startPosition, 
        LayerMask dieLayerMask, 
        ThrowMode throwMode)
    {
        ForceMagnitude = forceMagnitude;
        TorqueStrength = torqueStrength;
        MinThrowVelocity = minThrowVelocity;
        DieLayerMask = dieLayerMask;
        StartPosition = startPosition;
        ThrowMode = throwMode;
    }
}

[Serializable]
public abstract class MoveData : IMoveData
{
    public ThrowMode ThrowMode;
    
    public float TorqueStrength;
    public float MinThrowVelocity;

    public LayerMask DieLayerMask;
    public Vector3 StartPosition;
    
    public abstract DieMoveData GetData();
}

[Serializable]
public class ManualMoveData : MoveData
{
    public float ForceMagnitude;
    public override DieMoveData GetData()
    {
        return new DieMoveData()
        {
            ForceMagnitude = ForceMagnitude,
            TorqueStrength = TorqueStrength,
            MinThrowVelocity = MinThrowVelocity,
            StartPosition = StartPosition,
            ThrowMode = ThrowMode,
            DieLayerMask = DieLayerMask
        };
    }
}

[Serializable]
public class AutoMoveData : MoveData
{
    public float MaxForceMagnitude;
    public float MinForceMagnitude;
    
    public float RedAxisMinPos;
    public float RedAxisMaxPos;
    public float BlueAxisMinPos;
    public float BlueAxisMaxPos;
    public float Height;
    public override DieMoveData GetData()
    {
        return new DieMoveData()
        {
            ForceMagnitude = Random.Range(MaxForceMagnitude, MinForceMagnitude),
            TorqueStrength = TorqueStrength,
            MinThrowVelocity = MinThrowVelocity,
            StartPosition = new Vector3(
                Random.Range(RedAxisMinPos, RedAxisMaxPos), 
                Height, 
                Random.Range(BlueAxisMinPos, BlueAxisMaxPos)),
            ThrowMode = ThrowMode,
            DieLayerMask = DieLayerMask
        };
    }
}

public class DieMovement : MonoBehaviour
{
    public bool IsReleased { get; set; }

    public IDieAction DieAction => m_dieAction;

    public IMoveData AutoMoveData => m_autoMoveData;

    [SerializeField]
    private DieMoveData m_dieMoveData;
    

    [SerializeField]
    private AutoMoveData m_autoMoveData = new AutoMoveData();

    [SerializeField]
    private ManualMoveData m_manualMoveData = new ManualMoveData();

    private TwelveSideDieController m_twelveSideDieController;
    private Rigidbody m_rigidbody;
    
    private IDieAction m_dieAction;
    
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
        if (IsReleased)
            return;
        
        if (!m_dieAction.Take(m_manualMoveData.GetData()))
            return;
        
        m_isHeld = true;
        Cursor.visible = false;
    }

    private void HandleMouseUp()
    {
        m_isHeld = false;
        m_dieAction.Release(m_manualMoveData.GetData());
    }
    
    private void HandleMouseDrag()
    {
        m_dieAction.Hold();
    }
}
