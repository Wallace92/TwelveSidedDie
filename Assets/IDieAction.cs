using UnityEngine;

public interface IDieAction
{
    public void Release(DieMoveData dieMoveData);
    public bool Take(LayerMask dieLayerMask);
    public void Hold();
}