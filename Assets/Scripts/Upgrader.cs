using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Upgrader : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private TextMeshProUGUI rateText;


    [SerializeField] private float storageLimit;

    public Generator waterGenerator;

    private void Start()
    {
        rateText.text = "Water Generator Rate : " + waterGenerator.rate.ToString();
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
        waterGenerator.rate++;
        rateText.text = "Water Generator Rate : " + waterGenerator.rate.ToString();
    }
}
