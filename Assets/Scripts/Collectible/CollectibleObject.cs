using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    public GameObject bronze;
    public GameObject silver;
    public GameObject gold;

    UIManager uiManager;
    GameObject currentMedal;

    // Start is called before the first frame update
    void Start()
    {
        bronze.SetActive(true);
        silver.SetActive(false);
        gold.SetActive(false);
        currentMedal = bronze;

        transform.DORotate(new Vector3(0f, 360f, 0f), 10f, RotateMode.WorldAxisAdd)
       .SetEase(Ease.Linear)
       .SetLoops(-1);

        uiManager = UIManager.Instance;
    }

    void Update()
    {
        if (uiManager.totalScore > 20)
        {
            if (currentMedal != gold)
            {
                ActivateMedal(gold);
            }
        }
        else if (uiManager.totalScore > 10 && uiManager.totalScore <= 20)
        {
            if (currentMedal != silver)
            {
                ActivateMedal(silver);
            }
        }
    }

    void ActivateMedal(GameObject medal)
    {
        currentMedal.SetActive(false);
        medal.SetActive(true);
        currentMedal = medal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("collected");
            UIManager.Instance.UpdateScore(+1);
            gameObject.SetActive(false);
        }

        if (other.CompareTag("IceFloor"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
