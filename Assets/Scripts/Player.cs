using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour, ISaveable
{
    public static Player Instance { get; private set; }

    private PlayerController _controller;
    private Collector _collector;

    [SerializeField] private TextMeshProUGUI moneyText;

    public int currenetMoney = 1000;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        _controller = GetComponent<PlayerController>();
        _collector = GetComponent<Collector>();
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

    #region Saving System

    [Serializable]
    private struct SaveData
    {
        public float posX;
        public float posY;
        public float posZ;

        public int currentMoney;
        public float playerSpeed;
        public float collectRate;
        public float capacity;
    }

    public object CaptureState()
    {
        return new SaveData
        {
            posX = transform.position.x,
            posY = transform.position.y,
            posZ = transform.position.z,
            currentMoney = currenetMoney,
            playerSpeed = _controller.playerSpeed,
            collectRate = _collector.collectRate,
            capacity = _collector.capacity
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;
        var posData = new Vector3(saveData.posX, saveData.posY, saveData.posZ);

        transform.position = posData;
        currenetMoney = saveData.currentMoney;
        _controller.playerSpeed = saveData.playerSpeed;
        _collector.collectRate = saveData.collectRate;
        _collector.capacity = saveData.capacity;
    }
    #endregion
}
