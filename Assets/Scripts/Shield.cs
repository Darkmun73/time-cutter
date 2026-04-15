using UnityEngine;
using UnityEngine.Events;

public class Shield : MonoBehaviour
{
    bool isBroken = false;

    public UnityAction Broken;
    public UnityAction Blocked;

    public void Break()
    {
        isBroken = true;
        Broken?.Invoke();
    }

    public bool TryBlock()
    {
        if (isBroken) return false;

        Blocked?.Invoke();
        return true;
    }
}
