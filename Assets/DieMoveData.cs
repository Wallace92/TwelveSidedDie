using System;
using UnityEngine;

public enum ThrowMode
{
    NONE = -1,
    MANUAL = 0,
    AUTO = 1
};

[Serializable]
public struct DieMoveDataStruct
{
    public ThrowMode ThrowMode;
    public float ForceMagnitude;
    public float TorqueStrength;
    public float MinThrowVelocity;

    public LayerMask DieLayerMask;
    public Vector3 StartPosition;

    public DieMoveDataStruct(float forceMagnitude, 
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