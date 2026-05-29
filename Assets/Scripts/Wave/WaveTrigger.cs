using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    [SerializeField] private WaveController waveController;

    void OnTriggerEnter2D(Collider2D collision)
    {
        waveController.Initialize();
        Destroy(gameObject);
    }
}
