using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRedArea : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        Debug.Log("OnCollision: " + other.tag);
        if (other.CompareTag("Player"))
        {
            Debug.Log("OnCollision fail.");
        }
    }
}
