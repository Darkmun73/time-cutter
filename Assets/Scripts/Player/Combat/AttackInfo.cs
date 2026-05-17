using System.Collections.Generic;
using UnityEngine;

public class AttackInfo
{
    private readonly PlayerAttackData data;
    private int numberOfHits = 0;

    public List<float> HitAngles {get; private set;} // TODO: сделать приватным, и добавить сюда методы, через которые и изменять HitAngles
    public float DamageCoef {get; private set;}

    
    public AttackInfo(float firstHitAngle, PlayerAttackData data)
    {
        this.data = data;

        HitAngles = new() { firstHitAngle };
        DamageCoef = data.MinDamageCoef;
        numberOfHits++;
    }

    // (minCoef, maxCoef): clamps damage accumulation coef to this interval
    // thresholdAngle: if difference of last angles < then this value then coef decreases, else increases
    public void RegisterHit(float angle)
    {
        // Always have at least two hits before recalculating (from constructor + first call of this RegisterHit method)
        HitAngles.Add(angle);
        numberOfHits++;
        if (HitAngles.Count > data.MaxHitAngles)
            HitAngles.RemoveAt(0);
        RecalculateDamageCoef();
    }

    private void RecalculateDamageCoef()
    {
        float lastAngle = HitAngles[^1];
        float preLastAngle = HitAngles[^2];

        float acuteAngleDiff = Angles.GetAcuteAngleBetween(lastAngle, preLastAngle);
        
        if (acuteAngleDiff > data.DamageThresholdAngle)
        {
            var normalizedDiff = (acuteAngleDiff - data.DamageThresholdAngle) / (Angles.RightAngle - data.DamageThresholdAngle);
            float baseGrowth = Mathf.Lerp(0f, data.DamageReward, normalizedDiff);

            float repetitionMultiplier = CalculateRepetitionMultiplier();
            float growth = baseGrowth * repetitionMultiplier;

            DamageCoef += growth;
            
            //Debug.Log($"✓ Num: {numberOfHits} Last angle: {lastAngle}° Diff:{acuteAngleDiff:F1}° Base:{baseGrowth:F3} Repetition:{repetitionMultiplier:F3} Final:{growth:F3} → {DamageCoef:F3}");
        }
        else
        {
            var normalizedAngleDiff = 1f - (acuteAngleDiff / data.DamageThresholdAngle);
            float penalty = Mathf.Lerp(0f, data.DamagePenalty, normalizedAngleDiff);
            DamageCoef -= penalty;
            
            //Debug.Log($"✗ Num: {numberOfHits} Last angle: {lastAngle}° Diff:{acuteAngleDiff:F1}° Penalty:{penalty:F3} → {DamageCoef:F3}");
        }
        
        DamageCoef = Mathf.Clamp(DamageCoef,
                                    data.MinDamageCoef,
                                    data.MaxDamageCoef);
    }

    private float CalculateRepetitionMultiplier()
    {
        float lastAngle = HitAngles[^1];
        float totalPenalty = 0f;
        
        var anglesCountWithoutLastTwo = HitAngles.Count - 2;
        for (int i = 0; i < anglesCountWithoutLastTwo; i++)
        {
            float historicAngle = HitAngles[i];
            float acuteAngleDiff = Angles.GetAcuteAngleBetween(lastAngle, historicAngle);
            
            if (acuteAngleDiff < data.DamageThresholdAngle)
            {
                float similarity = 1f - (acuteAngleDiff / data.DamageThresholdAngle);
                
                int positionFromEnd = anglesCountWithoutLastTwo - i;
                float decayFactor = Mathf.Exp(-positionFromEnd * data.DecayCoef);
                
                totalPenalty += similarity * decayFactor;
            }
        }
        
        float multiplier = 1f - Mathf.Clamp01(totalPenalty) * Mathf.Clamp01(data.RepetitionPenalty);
        return multiplier;
    }
}