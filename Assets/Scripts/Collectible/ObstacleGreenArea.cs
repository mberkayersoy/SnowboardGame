using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGreenArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.UpdateScore(+1);
            gameObject.SetActive(false);
        }
    }
}
