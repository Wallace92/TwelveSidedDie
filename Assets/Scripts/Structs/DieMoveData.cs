using System;
using UnityEngine;

[Serializable]
public struct DieMoveData
{
    public ThrowMode ThrowMode;
    public float ForceMagnitude;
    public float TorqueStrength;
    public float MinThrowVelocity;
    
    public Vector3 StartPosition;

    public DieMoveData(float forceMagnitude, float torqueStrength, float minThrowVelocity, Vector3 startPosition, ThrowMode throwMode)
    {
        ForceMagnitude = forceMagnitude;
        TorqueStrength = torqueStrength;
        MinThrowVelocity = minThrowVelocity;
        StartPosition = startPosition;
        ThrowMode = throwMode;
    }
}