using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrader : MonoBehaviour
{
    public static Upgrader Instance { get; private set; }

    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private TextMeshProUGUI rateText;
    [SerializeField] private GameObject workerPrefab;
    [SerializeField] private Transform spawnPoint;

    //prices

    //Player

    private Player player;

    public int playerCollectRatePrice = 250;
    public int playerCapacityPrice = 250;
    public int playerSpeedPrice = 250;

    public Button playerCollectRateButton;
    public Button playerCapacityButton;
    public Button playerSpeedButton;

    //Workers

    public List<Worker> workerList = new List<Worker>();

    public int workerSpawnPrice = 1000;
    public int workerCollectRatePrice = 500;
    public int workerCapacityPrice = 500;
    public int workerSpeedPrice = 500;

    public Button workerSpawnButton;
    public Button workerCollectRateButton;
    public Button workerCapacityButton;
    public Button workerSpeedButton;

    //Generator
    public int generatorRatePrice = 500;
    public int generatorCapacityPrice = 500;

    //Converter
    public int converterConsumeRatePrice = 500;
    public int converterConvertRatePrice = 500;
    public int converterCapacityPrice = 500;


    private void Start()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

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

    #region Player Upgrades

    public void UpgradePlayerCollectRate()
    {
        if (Player.Instance.currenetMoney < playerCollectRatePrice)
        {
            Debug.Log("More money is required !");
            return;
        }

        Player.Instance.SpendMoney(playerCollectRatePrice);
        playerCollectRatePrice += playerCollectRatePrice;
        player.GetComponent<Collector>().collectRate += 0.5f;

        CheckCap(player.GetComponent<Collector>().collectRate, 5, playerCollectRateButton, playerCollectRatePrice);
    }

    public void UpgradePlayerCapacity()
    {
        if (Player.Instance.currenetMoney < playerCapacityPrice)
        {
            Debug.Log("More money is required !");
            return;
        }

        Player.Instance.SpendMoney(playerCapacityPrice);
        playerCapacityPrice += (2 * playerCapacityPrice);
        player.GetComponent<Collector>().capacity += 5;

        CheckCap(player.GetComponent<Collector>().capacity, 50f, playerCapacityButton, playerCapacityPrice);

    }

    public void UpgradePlayerSpeed()
    {
        if (Player.Instance.currenetMoney < playerSpeedPrice)
        {
            Debug.Log("More money is required !");
            return;
        }
        Player.Instance.SpendMoney(playerSpeedPrice);
        playerSpeedPrice += (int)(1.5 * playerSpeedPrice);
        player.GetComponent<PlayerController>().playerSpeed += 0.5f;

        CheckCap(player.GetComponent<PlayerController>().playerSpeed, 6.5f, playerSpeedButton, playerSpeedPrice);
    }

    #endregion

    #region Worker Upgrades

    public void SpawnWorker()
    {
        if (Player.Instance.currenetMoney < workerSpawnPrice)
        {
            Debug.Log("More money is required");
            return;
        }

        Player.Instance.SpendMoney(workerSpawnPrice);
        workerSpawnPrice += (2 * workerSpawnPrice);
        var go = workerList[workerList.Count - 1];
        workerList.Remove(go);
        go.gameObject.SetActive(true);

        // Unique CheckCap for Worker
        if (workerList.Count <= 0)
        {
            workerSpawnButton.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
            workerSpawnButton.interactable = false;
        }
        else
            workerSpawnButton.GetComponentInChildren<TextMeshProUGUI>().text = "$" + workerSpawnPrice;
    }

    #endregion

    

    public void CheckCap(float upgraded, float cap, Button button, int price)
    {
        if (upgraded >= cap)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
            button.interactable = false;
        }
        else
            button.GetComponentInChildren<TextMeshProUGUI>().text = "$" + price;
    }
}
