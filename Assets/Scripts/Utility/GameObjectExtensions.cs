using UnityEngine;

public static class GameObjectExtensions
{
    public static void SetAllComponentsEnabled(this GameObject gameObject, bool enable)
    {
        SetAllComponentsEnabled(gameObject, enable, null);
    }

    public static void SetAllComponentsEnabled(this GameObject gameObject, bool enable, Component exception)
    {
        var components = gameObject.GetComponents<Component>();
        foreach (var component in components)
        {
            if (component == exception) continue;

            if (component is Behaviour behaviour)
                behaviour.enabled = enable;
            else if (component is Renderer renderer)
                renderer.enabled = enable;
            else if (component is Rigidbody2D rigidbody2D)
                rigidbody2D.simulated = enable;
        }
    }
}