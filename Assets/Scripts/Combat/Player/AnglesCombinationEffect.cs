using System;
using System.Collections.Generic;
using UnityEngine;

public class AnglesCombinationEffect
{
    public List<float> AnglesCombination {get;}
    private readonly Action effect;

    public AnglesCombinationEffect(List<float> anglesCombo, Action effect)
    {
        AnglesCombination = anglesCombo;
        this.effect = effect;
    }

    // If end of angles is same as combination - effect applies
    public void TryApply(List<float> inputAngles, float angleTollerance)
    {
        var shouldApply = Angles.EndsWithPatternWithinTolerance(inputAngles, AnglesCombination, angleTollerance);
        //Debug.Log(shouldApply);
        if (shouldApply)
            effect?.Invoke();
    }
}