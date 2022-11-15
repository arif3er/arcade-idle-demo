using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI moneyText;

    public int currenetMoney = 1000;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        UpdateMoneyText();
    }

    public void AddMoney(int amount)
    {
        currenetMoney += amount;
        UpdateMoneyText();
    }

    public void SpendMoney(int amount)
    {
        if (currenetMoney >= amount)
        {
            currenetMoney -= amount;
            UpdateMoneyText();
        }

        if (currenetMoney < amount)
        {
            Debug.Log("Dont have enough money !");
        }
    }

    public void UpdateMoneyText()
    {
        moneyText.text = "$ " + currenetMoney.ToString();
    }
}
