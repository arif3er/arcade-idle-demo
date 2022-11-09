using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Upgrader : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private TextMeshProUGUI rateText;
    [SerializeField] private GameObject workerPrefab;
    [SerializeField] private Transform spawnPoint;

    public Generator waterGenerator;

    private void Start()
    {
        upgradePanel.SetActive(false);
        rateText.text = "Water Generator Rate : " + waterGenerator.spawnRate.ToString();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            upgradePanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            upgradePanel.SetActive(false);
        }
    }

    public void UpgradeRate()
    {
        Debug.Log("Rate increased.");
        waterGenerator.spawnRate++;
        rateText.text = "Water Generator Rate : " + waterGenerator.spawnRate.ToString();
        Player.Instance.SpendMoney(100);
    }

    public void SpawnWorker()
    {
        Debug.Log("WorkerSpawned.");
        Instantiate(workerPrefab, spawnPoint);
        Player.Instance.SpendMoney(250);
    }
}
