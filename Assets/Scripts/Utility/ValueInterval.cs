using System;

[Serializable]
public struct ValueInterval
{
    public bool startIsNegativeInfinity;
    public bool endIsPositiveInfinity;
    public float start;
    public float end;

    public bool Contains(float value)
    {
        return (startIsNegativeInfinity || value >= start) &&
               (endIsPositiveInfinity || value <= end);
    }
}