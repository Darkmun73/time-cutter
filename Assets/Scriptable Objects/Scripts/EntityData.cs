using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "Scriptable Objects/Entity Data")]
public class EntityData : ScriptableObject
{
    [field: SerializeField] public float MovementSpeed {get;} = 10f;
    [field: SerializeField] public float MaxFallSpeed {get;} = 10f;
    [field: SerializeField] public float JumpHeight {get;} = 2f;
    [field: SerializeField] public float JumpTime {get;} = 1f;
    [field: SerializeField] public float MaxHealth {get;} = 100f;
    [field: SerializeField] public float BaseDamage {get;} = 10f;

    [field: SerializeField] public ContactFilter2D Ground {get;} // TODO: перенести отсюда куда-нибудь в другое место
}
