using UnityEngine;

public class DemoEndController : MonoBehaviour
{
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject coreCanvases;

    void Awake()
    {
        endText.SetActive(false);
    }

    public void Initialize()
    {
        endText.SetActive(true);
        var player = FindFirstObjectByType<Player>();
        player.gameObject.SetActive(false);
        coreCanvases.SetActive(false);
    }
}
