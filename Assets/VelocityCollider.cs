using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityCollider : MonoBehaviour
{
    public bool addVelocity;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("IceFloor"))
        {
            addVelocity = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("IceFloor"))
        {
            addVelocity = false;
        }
    }
}
