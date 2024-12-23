using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEditor.Build.Content;
public class ObstacleHit : MonoBehaviour
{
    public UnityEvent triggerAction;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Train"))
        {
            triggerAction?.Invoke();
            // GameManager.instance.health--;
            // other.gameObject.GetComponent<Health>().currentHealth--;
               
        }
    }
}
