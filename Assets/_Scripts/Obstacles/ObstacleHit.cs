using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEditor.Build.Content;
public class ObstacleHit : MonoBehaviour
{
    public UnityEvent onTrainHit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Train"))
        {
            onTrainHit?.Invoke();
            // GameManager.instance.health--;
            // other.gameObject.GetComponent<Health>().currentHealth--;
            FadeManager.Instance.FadeInOut();   
        }
    }
}
