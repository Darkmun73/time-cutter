using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "DebugFunctions", menuName = "Scriptable Objects/Debug Functions")]
public class DebugFunctions : ScriptableObject
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject simpleEnemyPrefab;

    private void OnEnable()
    {
        if (inputReader == null)
            return;

        inputReader.DebugSpawnEnemyRequested += SpawnSimpleEnemyAtCursor;
    }

    private void OnDisable()
    {
        if (inputReader == null)
            return;

        inputReader.DebugSpawnEnemyRequested -= SpawnSimpleEnemyAtCursor;
    }

    private void SpawnSimpleEnemyAtCursor()
    {
        if (simpleEnemyPrefab == null)
        {
            Debug.LogWarning("SimpleEnemy prefab is not assigned in DebugFunctions.");
            return;
        }

        if (Pointer.current == null)
        {
            Debug.LogWarning("Pointer device is unavailable for debug enemy spawn.");
            return;
        }

        Camera currentCamera = Camera.main;
        if (currentCamera == null)
        {
            Debug.LogWarning("Main Camera was not found for debug enemy spawn.");
            return;
        }

        Vector2 cursorScreenPosition = Pointer.current.position.ReadValue();
        Vector3 cursorWorldPosition = currentCamera.ScreenToWorldPoint(
            new Vector3(cursorScreenPosition.x, cursorScreenPosition.y, currentCamera.nearClipPlane));
        cursorWorldPosition.z = simpleEnemyPrefab.transform.position.z;

        Instantiate(simpleEnemyPrefab, cursorWorldPosition, Quaternion.identity);
    }
}
