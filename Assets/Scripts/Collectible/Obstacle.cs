using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IceFloor"))
        {
            Debug.Log("HIT");
            //GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
