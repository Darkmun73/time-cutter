using UnityEngine;

public class DemoEndTrigger : MonoBehaviour
{
    [SerializeField] private DemoEndController demoEndController;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        demoEndController.Initialize();
    }
}
