using System;
using UnityEngine;

[Serializable]
public class ManualMoveData : MoveData
{
    [SerializeField]
    private float m_forceMagnitude;
    public override DieMoveData GetData()
    {
        return new DieMoveData()
        {
            ForceMagnitude = m_forceMagnitude,
            TorqueStrength = m_torqueStrength,
            MinThrowVelocity = m_minThrowVelocity,
            StartPosition = Vector3.zero,
            ThrowMode = m_throwMode,
        };
    }
}