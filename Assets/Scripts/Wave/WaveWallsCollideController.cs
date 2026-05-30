using System.Collections;
using TMPEffects.Parameters;
using UnityEngine;

public class WaveWallsCollideController : MonoBehaviour
{
    [SerializeField] private GameObject stopText;
    [SerializeField] private WaveController waveController;

    [SerializeField] private float stopTextAppearenceTime;

    private Coroutine coroutine;

    void Awake()
    {
        stopText.SetActive(false);
    }

    void OnDisable()
    {
        stopText.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        if (!waveController.IsWaveGoing)
            ShowStopText();
    }

    private void ShowStopText()
    {
        if (coroutine == null)
            coroutine = StartCoroutine(ShowStopTextRoutine());
    }

    private IEnumerator ShowStopTextRoutine()
    {
        stopText.SetActive(true);
        yield return new WaitForSeconds(stopTextAppearenceTime);
        stopText.SetActive(false);
        coroutine = null;
    }
}