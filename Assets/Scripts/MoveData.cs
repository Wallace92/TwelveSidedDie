using System;
using UnityEngine;

[Serializable]
public abstract class MoveData : IMoveData
{
    [SerializeField]
    protected ThrowMode m_throwMode;
    
    [SerializeField]
    protected float m_torqueStrength;
    
    [SerializeField]
    protected float m_minThrowVelocity;

    public abstract DieMoveData GetData();
}