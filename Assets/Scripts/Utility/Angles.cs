using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains helper methods and common constants for working with angles in degrees.
/// </summary>
public static class Angles
{
    /// <summary>
    /// Represents an angle of <c>0</c> degrees.
    /// </summary>
    public const float ZeroAngle = 0f;

    /// <summary>
    /// Represents a right angle of <c>90</c> degrees.
    /// </summary>
    public const float RightAngle = 90f;

    /// <summary>
    /// Represents a straight angle of <c>180</c> degrees.
    /// </summary>
    public const float StraightAngle = 180f;

    /// <summary>
    /// Converts an angle in the range from <c>0</c> to <c>180</c> degrees to its acute equivalent.
    /// </summary>
    /// <param name="angle">The angle in degrees.</param>
    /// <returns>
    /// The acute angle value in degrees.
    /// Returns the original value when it is less than <see cref="RightAngle"/>;
    /// otherwise returns its supplementary angle.
    /// </returns>
    public static float ToAcuteAngle(float angle)
    {
        return angle < RightAngle ? angle : StraightAngle - angle;
    }

    /// <summary>
    /// Calculates the acute difference between two angles.
    /// </summary>
    /// <param name="angle1">The first angle in degrees.</param>
    /// <param name="angle2">The second angle in degrees.</param>
    /// <returns>
    /// The smallest acute angle between <paramref name="angle1"/> and <paramref name="angle2"/> in degrees.
    /// </returns>
    public static float GetAcuteAngleBetween(float angle1, float angle2)
    {
        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(angle1, angle2));
        return ToAcuteAngle(angleDiff);
    }

    /// <summary>
    /// Determines whether two angles differ by no more than the specified tolerance.
    /// </summary>
    /// <param name="angle1">The first angle in degrees.</param>
    /// <param name="angle2">The second angle in degrees.</param>
    /// <param name="tolerance">The maximum allowed difference in degrees.</param>
    /// <returns>
    /// <see langword="true"/> if the absolute difference between the angles is less than
    /// or equal to <paramref name="tolerance"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool AreWithinTolerance(float angle1, float angle2, float tolerance)
    {
        return Mathf.Abs(angle1 - angle2) <= tolerance;
    }

    /// <summary>
    /// Determines whether the last angles in a sequence match a target pattern within the specified tolerance.
    /// </summary>
    /// <param name="angles">The source angle sequence.</param>
    /// <param name="pattern">The target angle pattern to compare against.</param>
    /// <param name="tolerance">The maximum allowed difference for each compared angle.</param>
    /// <returns>
    /// <see langword="true"/> if the end of <paramref name="angles"/> matches <paramref name="pattern"/>
    /// within <paramref name="tolerance"/> for every element; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool EndsWithPatternWithinTolerance(IReadOnlyList<float> angles, IReadOnlyList<float> pattern, float tolerance)
    {
        if (angles.Count < pattern.Count)
            return false;

        int startIndex = angles.Count - pattern.Count;
        for (int i = 0; i < pattern.Count; i++)
        {
            if (!AreWithinTolerance(angles[startIndex + i], pattern[i], tolerance))
                return false;
        }

        return true;
    }
}
