using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "Scriptable Objects/Movement Data")]
public class MovementData : ScriptableObject
{
    [field: SerializeField] public float MovementSpeed {get; private set;} = 10f;
    [field: SerializeField] public float MaxFallSpeed {get; private set;} = 10f;
    [field: SerializeField] public float JumpHeight {get; private set;} = 2f;
    [field: SerializeField] public float JumpTime {get; private set;} = 1f;
    
    [field: SerializeField] public ContactFilter2D Ground {get; private set;} // TODO: перенести отсюда куда-нибудь в другое место
}
