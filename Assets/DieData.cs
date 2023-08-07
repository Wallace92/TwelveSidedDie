using System;

[Serializable]
public struct DieData
{
    public float NumberFontSize;
    public float NumberAlignment;

    public DieData(float numberFontSize, float numberAlignment)
    {
        NumberFontSize = numberFontSize;
        NumberAlignment = numberAlignment;
    }
}