using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingManager : MonoBehaviour
{
    [SerializeField] private GameObject plant;
    [SerializeField] private GameObject plantUI;

    [SerializeField] private float collectRate;
    [SerializeField] private int waterNeeded;

    private Collector collector;


    private int currentWater = 0;
    private bool isCollecting;
    private bool isUnlocked;

    private void Start()
    {
        plant.SetActive(false);
        StartCoroutine(CollectEnum());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collector = other.GetComponent<Collector>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isCollecting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isCollecting = false;
        }
    }

    IEnumerator CollectEnum()
    {
        while (true)
        {
            if (isCollecting)
            {
                CollectWater();
            }
            yield return new WaitForSeconds(1 / collectRate);
        }
    }

    private void CollectWater()
    {
        if (currentWater >= waterNeeded)
        {
            isUnlocked = true;
            StopAllCoroutines();
            plant.SetActive(true);
            plantUI.SetActive(false);
        }
        if (!isUnlocked && collector.gameObjectList.Count > 0)
        {
            currentWater++;
            collector.RemoveLast();
            UIManager.Instance.UpdatePlantText(currentWater, waterNeeded);
            UIManager.Instance.UpdatePlantBar((float)currentWater / waterNeeded);
        }
    }
}
