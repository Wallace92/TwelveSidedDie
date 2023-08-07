using System;
using UnityEngine;

[Serializable]
public struct DieMoveData
{
    public float ForceMagnitude;
    public float TorqueStrength;
    public float MinThrowVelocity;

    public LayerMask DieLayerMask;
    public Vector3 StartPosition;

    public DieMoveData(float forceMagnitude, float torqueStrength, float minThrowVelocity, Vector3 startPosition, LayerMask dieLayerMask)
    {
        ForceMagnitude = forceMagnitude;
        TorqueStrength = torqueStrength;
        MinThrowVelocity = minThrowVelocity;
        DieLayerMask = dieLayerMask;
        StartPosition = startPosition;
    }
}