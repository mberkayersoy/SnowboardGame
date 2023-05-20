using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         transform.DORotate(new Vector3(0f, 360f, 0f), 10f, RotateMode.WorldAxisAdd)
        .SetEase(Ease.Linear)
        .SetLoops(-1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("collected");
        }
    }
}
