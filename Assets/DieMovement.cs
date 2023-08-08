using UnityEngine;


public class DieMovement : MonoBehaviour
{
    public bool IsReleased { get; set; }

    public IDieAction DieAction => m_dieAction;

    public IMoveData AutoMoveData => m_autoMoveData;
    
    [SerializeField]
    private LayerMask m_dieLayerMask;
    
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

    private void HandleMouseDown()
    {
        if (IsReleased)
            return;
        
        if (!m_dieAction.Take(m_dieLayerMask))
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
}
