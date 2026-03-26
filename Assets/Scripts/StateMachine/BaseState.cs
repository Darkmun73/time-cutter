using UnityEngine;

public abstract class BaseState
{
    public virtual void Enter() { Debug.Log($"Entered state: {GetType()}"); }
    public virtual void Update() { Debug.Log($"Updated state: {GetType()}"); }
    public virtual void FixedUpdate() { Debug.Log($"Fixed updated state: {GetType()}"); }
    public virtual void Exit() { Debug.Log($"Exited state: {GetType()}"); }
}
