using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; set; }

    [SerializeField] private TextMeshProUGUI waterText;
    [SerializeField] private TextMeshProUGUI plantText;
    [SerializeField] private Image image;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void UpdatePlantText(int amount, int limit)
    {
        plantText.text = "Need Water \n" + amount + " / " + limit;
    }

    public void UpdatePlantBar(float amount)
    {
        image.fillAmount = amount;
    }
}
