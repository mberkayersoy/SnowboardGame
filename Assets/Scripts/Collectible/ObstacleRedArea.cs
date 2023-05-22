using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRedArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OnCollision: Player failed.");
            UIManager.Instance.EndGame(false);
        }
    }
}
