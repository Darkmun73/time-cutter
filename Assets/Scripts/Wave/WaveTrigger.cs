using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    [SerializeField] private WaveController waveController;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        waveController.Initialize();
        Destroy(gameObject);
    }
}
