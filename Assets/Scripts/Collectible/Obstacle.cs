using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject greenArea;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OnTriggerEnter");
            UIManager.Instance.UpdateScore(+1);
            greenArea.SetActive(false);
        }
        if (other.CompareTag("IceFloor"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
