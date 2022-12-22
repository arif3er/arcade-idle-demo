using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, ISaveable
{
    public static Player Instance { get; private set; }

    private PlayerController _controller;
    private Collector _collector;

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject fullWarn;
    [SerializeField] private Image waterCanImage;

    public int currentMoney = 1000;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        _controller = GetComponent<PlayerController>();
        _collector = GetComponent<Collector>();
        UpdateInventoryText();
        fullWarn.transform.DOScale(new Vector3(1f, 1f, 1f), 1).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        if (_collector.backpack.Count == _collector.capacity)
            fullWarn.SetActive(true);
        else
            fullWarn.SetActive(false);

        fullWarn.transform.position = this.transform.position + new Vector3(0, 2, 0);

        ArifHelpers.FillImage(waterCanImage, _collector.waterLiter, 100);
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        moneyText.gameObject.transform.DOPunchScale(new Vector3(1,1,1), 0.1f);
        UpdateInventoryText();
    }

    public void SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateInventoryText();
        }

        if (currentMoney < amount)
        {
            Debug.Log("Dont have enough money !");
        }
    }

    public void UpdateInventoryText()
    {
        moneyText.text = "$ " + currentMoney.ToString();
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
            currentMoney = currentMoney,
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

        currentMoney = saveData.currentMoney;

        _controller.playerSpeed = saveData.playerSpeed;
        _collector.collectRate = saveData.collectRate;
        _collector.capacity = saveData.capacity;
    }
    #endregion
}
