using UnityEngine;

[CreateAssetMenu(fileName = "PathfindingData", menuName = "Scriptable Objects/Pathfinding Data")]
public class PathfindingData : ScriptableObject
{
    [field: SerializeField] public float PathUpdateTime {get; private set;} = 0.5f; // MAYBE TODO: чекнуть в unity profiler сильно ли по перфомансу бьет с кучей врагов
    [field: SerializeField] public float JumpNodeHeightRequirement {get; private set;} = 1f;
    [field: SerializeField] public float NextWaypointDistance {get; private set;} = 3f;
}
