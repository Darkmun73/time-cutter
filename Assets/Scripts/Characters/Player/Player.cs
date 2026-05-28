using UnityEngine;

public class Player : MonoBehaviour
{
    void Awake()
    {
        var players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        foreach (var player in players)
        {
            if (player != this)
            {
                transform.GetPositionAndRotation(out Vector3 newPos, out Quaternion newRotation);
                player.transform.SetPositionAndRotation(newPos, newRotation);
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
