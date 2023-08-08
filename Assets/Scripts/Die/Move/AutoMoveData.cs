using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class AutoMoveData : MoveData
{
    [Header("Random Force")]
    [SerializeField]
    private float m_maxForceMagnitude;
    [SerializeField]
    private float m_minForceMagnitude;
    
    [Header("Random Position")]
    [SerializeField]
    private float m_redAxisMinPos;
    [SerializeField]
    private float m_redAxisMaxPos;
    [SerializeField]
    private float m_blueAxisMinPos;
    [SerializeField]
    private float m_blueAxisMaxPos;
    [SerializeField]
    private float m_height;

    public override DieMoveData GetData()
    {
        return new DieMoveData()
        {
            ForceMagnitude = Random.Range(m_maxForceMagnitude, m_minForceMagnitude),
            TorqueStrength = m_torqueStrength,
            MinThrowVelocity = m_minThrowVelocity,
            StartPosition = new Vector3(
                Random.Range(m_redAxisMinPos, m_redAxisMaxPos), 
                m_height, 
                Random.Range(m_blueAxisMinPos, m_blueAxisMaxPos)),
            ThrowMode = m_throwMode,
        };
    }
}