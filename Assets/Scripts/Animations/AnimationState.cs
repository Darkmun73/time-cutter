using UnityEngine;

public class AnimationState {
    public static readonly AnimationState NoState = new("");

    private readonly int hash;

    public AnimationState(string stateName)
    {
        hash = Animator.StringToHash(stateName);
    }
    
    public int GetHash()
    {
        return hash;
    }
}