using UnityEngine;

public class PlateformMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f; // Speed at which the prefab moves
    public float despawnZ = 300f; // Z position at which the prefab is destroyed

    void Update()
    {
        // Move the prefab negatively along the Z-axis
        transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("spawnPoint"))
        {
            EventManager.Instance.GenerateNewLevel();
            Debug.Log("cc");
        }
    }
}
