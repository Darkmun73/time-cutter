using System;
using System.Collections.Generic;
using UnityEngine;

public class AnglesCombinationEffect
{
    private readonly List<float> anglesCombination;
    private readonly Action effect;

    public AnglesCombinationEffect(List<float> anglesCombo, Action effect)
    {
        this.anglesCombination = anglesCombo;
        this.effect = effect;
    }

    // If end of angles is same as combination - effect applies
    public void TryApply(List<float> inputAngles, float angleTollerance)
    {
        var shouldApply = Angles.EndsWithPatternWithinTolerance(inputAngles, anglesCombination, angleTollerance);
        Debug.Log(shouldApply);
        if (shouldApply)
            effect?.Invoke();
    }
}