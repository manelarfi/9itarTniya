using UnityEngine;

public class TrainRaycaster : MonoBehaviour
{
    public float hitDistance = 10f;
    public Transform raycastStartPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(raycastStartPoint.position, transform.forward, out RaycastHit hit, hitDistance))
        {

            if (hit.collider.CompareTag("Obstacle"))
            {
                Debug.Log("Obstacle found");
            }
        }
    }
}
