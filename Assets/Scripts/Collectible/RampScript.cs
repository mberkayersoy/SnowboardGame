using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IceFloor"))
        {
            // GetComponent<Rigidbody>().isKinematic = true;

        }

    }
}
