using System.Collections.Generic;
using UnityEngine;

public class ChildObjectsActivator : MonoBehaviour
{
    private List<GameObject> childObjects = new();

    void Awake()
    {
        var childTranforms = gameObject.GetComponentsInChildren<Transform>(true);
        foreach (var child in childTranforms)
        {
            if (child.gameObject == gameObject) continue;

            childObjects.Add(child.gameObject);
        }
    }

    public void SetActive(bool active)
    {
        foreach (var obj in childObjects)
        {
            obj.SetActive(active);
        }
    }
}