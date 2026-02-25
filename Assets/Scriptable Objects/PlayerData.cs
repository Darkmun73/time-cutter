using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Player Data")]
public class PlayerData : ScriptableObject
{
    [field: SerializeField] public float MovementSpeed {get; private set;} = 10f;
    [field: SerializeField] public float MaxFallSpeed {get; private set;} = 10f;
    [field: SerializeField] public float JumpHeight {get; private set;} = 2f;
    [field: SerializeField] public float JumpTime {get; private set;} = 1f;
    [field: SerializeField] public float MaxHealth {get; private set;} = 100f;

    [field: SerializeField] public ContactFilter2D Ground {get; private set;}
    [field: SerializeField] public InputReader InputReader {get; private set;}
}
