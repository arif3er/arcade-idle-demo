using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrader : MonoBehaviour
{
    public static Upgrader Instance { get; private set; }

    // Colliding
    private List<GameObject> shopList = new List<GameObject>();
    public GameObject playerUpgradeShop;
    public GameObject workerUpgradeShop;
    public GameObject generatorUpgradeShop;
    public GameObject converterUpgradeShop;
    public float collideRange;

    // Panels
    private List<GameObject> panelList = new List<GameObject>();
    public GameObject playerPanel;
    public GameObject workerPanel;
    public GameObject generatorPanel;
    public GameObject converterPanel;

    // Effects
    public GameObject moneyNeedWarnEffect;
    public GameObject successfulBuyEffect;

    //  Player
    public Player player;
    
    public int playerCollectRatePrice = 250;
    public int playerCapacityPrice = 250;
    public int playerSpeedPrice = 250;

    public Button playerCollectRateButton;
    public Button playerCapacityButton;
    public Button playerSpeedButton;

    //  Workers
    public List<Worker> workerList = new List<Worker>();

    public int workerSpawnPrice = 1000;
    public int workerCollectRatePrice = 500;
    public int workerCapacityPrice = 500;
    public int workerSpeedPrice = 500;

    public Button workerSpawnButton;
    public Button workerCollectRateButton;
    public Button workerCapacityButton;
    public Button workerSpeedButton;

    //  Generators
    public List<Generator> generatorList = new List<Generator>();

    public int generatorRatePrice = 500;
    public int generatorCapacityPrice = 500;

    public Button generatorRateButton;
    public Button generatorCapacityButton;

    //  Converters
    public List<Converter> converterList = new List<Converter>();

    public int converterCapacityPrice = 500;
    public int converterConvertRatePrice = 500;
    public int converterConsumeRatePrice = 500;

    public Button converterCapacityButton;
    public Button converterConvertRateButton;
    public Button converterConsumeRateButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        MatchPanelsAndShops();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < shopList.Count; i++)
        {
            if (ArifGDK.DistanceCollider(shopList[i], player.gameObject, 3) && shopList[i].activeInHierarchy)
                GetIn(shopList[i], panelList[i]);
            else
                panelList[i].SetActive(false);
        }
    }

    #region Player Upgrades

    public void UpgradePlayerCollectRate()
    {
        if (!CheckMoney(playerCollectRatePrice, "player collectRate", playerCollectRateButton.transform.position)) return;

        Player.Instance.SpendMoney(playerCollectRatePrice);
        playerCollectRatePrice += playerCollectRatePrice;
        player.GetComponent<Collector>().collectRate += 0.5f;

        CheckCap(player.GetComponent<Collector>().collectRate, 5, playerCollectRateButton, playerCollectRatePrice);
    }

    public void UpgradePlayerCapacity()
    {
        if (!CheckMoney(playerCapacityPrice, "player capacity", playerCapacityButton.transform.position)) return;

        Player.Instance.SpendMoney(playerCapacityPrice);
        playerCapacityPrice += (2 * playerCapacityPrice);
        player.GetComponent<Collector>().capacity += 5;

        CheckCap(player.GetComponent<Collector>().capacity, 50f, playerCapacityButton, playerCapacityPrice);
    }

    public void UpgradePlayerSpeed()
    {
        if (!CheckMoney(playerSpeedPrice, "player speed", playerSpeedButton.transform.position)) return;

        Player.Instance.SpendMoney(playerSpeedPrice);
        playerSpeedPrice += (int)(1.5 * playerSpeedPrice);
        player.GetComponent<PlayerController>().playerSpeed += 0.5f;

        CheckCap(player.GetComponent<PlayerController>().playerSpeed, 6.5f, playerSpeedButton, playerSpeedPrice);
    }

    #endregion

    #region Worker Upgrades

    public void SpawnWorker()
    {
        if (!CheckMoney(workerSpawnPrice, "worker spawn", workerSpawnButton.transform.position)) return;

        if (workerList.Count > 0)
        {
            Player.Instance.SpendMoney(workerSpawnPrice);
            workerSpawnPrice += (int)(1.25f * workerSpawnPrice);
            Worker wo = workerList[0];
            workerList.Remove(wo);
            wo.gameObject.SetActive(true);
            CheckCapWorkerSpawn();
        }
    }

    public void UpgradeWorkerCollectRate()
    {
        if (!CheckMoney(workerCollectRatePrice, "worker collectRate", workerCollectRateButton.transform.position)) return;

        foreach (var worker in workerList)
        {
            Player.Instance.SpendMoney(workerCollectRatePrice);
            worker.GetComponent<Collector>().collectRate += 0.5f;

            CheckCap(worker.GetComponent<Collector>().collectRate, 30, workerCollectRateButton, workerCollectRatePrice);
        }
        workerCollectRatePrice += (int)(1f * workerCollectRatePrice);
    }

    public void UpgradeWorkerCapacity()
    {
        if (!CheckMoney(workerCapacityPrice, "worker capacity", workerCapacityButton.transform.position)) return;

        foreach (var worker in workerList)
        {
            Player.Instance.SpendMoney(workerCapacityPrice);
            worker.GetComponent<Collector>().capacity += 5;

            CheckCap(worker.GetComponent<Collector>().capacity, 30, workerCapacityButton, workerCapacityPrice);
        }
        workerCapacityPrice += (int)(1.5f * workerCapacityPrice);
    }

    public void UpgradeWorkerSpeed()
    {
        if (!CheckMoney(workerSpeedPrice, "worker speed", workerSpeedButton.transform.position)) return;

        foreach (var worker in workerList)
        {
            Player.Instance.SpendMoney(workerSpeedPrice);
            worker.GetComponent<FollowPath>().speed += 0.5f;

            CheckCap(worker.GetComponent<FollowPath>().speed, 5f, workerSpeedButton, workerSpeedPrice);
        }
        workerSpeedPrice += (int)(1.2f * workerSpawnPrice);
    }
    #endregion

    #region Generator Upgrades

    public void UpgradeGeneratorRate()
    {
        if (!CheckMoney(generatorRatePrice, "generator rate", generatorRateButton.transform.position)) return;

        foreach (var generator in generatorList)
        {
            Player.Instance.SpendMoney(generatorRatePrice);
            generator.spawnRate += 0.2f;

            CheckCap(generator.spawnRate, 5f, generatorRateButton, generatorRatePrice);
        }
        generatorRatePrice += (int)(1.05f * generatorRatePrice);
    }

    public void UpgradeGeneratorCapacity()
    {
        if (!CheckMoney(generatorCapacityPrice, "generator capacity", generatorCapacityButton.transform.position)) return;

        foreach (var generator in generatorList)
        {
            Player.Instance.SpendMoney(generatorCapacityPrice);
            generator.capacity += 5f;

            CheckCap(generator.capacity, 30, generatorCapacityButton, generatorCapacityPrice);
        }
        generatorCapacityPrice += (int)(1.2f * generatorCapacityPrice);
    }
    #endregion

    #region Converter Upgrades

    public void UpgradeConverterCapacity()
    {
        if (!CheckMoney(converterCapacityPrice, "converter capacity", converterCapacityButton.transform.position)) return;

        foreach(var converter in converterList)
        {
            Player.Instance.SpendMoney(converterCapacityPrice);
            converter.capacity += 5;

            CheckCap(converter.capacity, 30, converterCapacityButton, converterCapacityPrice);
        }
        converterCapacityPrice += (int)(1.2f * converterCapacityPrice);
    }

    public void UpgradeConverterConvertRate()
    {
        if (!CheckMoney(converterConvertRatePrice, "converter convertRate", converterConvertRateButton.transform.position)) return;

        foreach (var converter in converterList)
        {
            Player.Instance.SpendMoney(converterConvertRatePrice);
            converter.convertRate += 0.2f;

            CheckCap(converter.convertRate, 3f, converterConvertRateButton, converterConvertRatePrice);
        }
        converterConvertRatePrice += (int)(1.2f * converterConvertRatePrice);
    }

    public void UpgradeConverterConsumeRate()
    {
        if (!CheckMoney(converterConsumeRatePrice, "converter consumeRate", converterConsumeRateButton.transform.position)) return;

        foreach (var converter in converterList)
        {
            Player.Instance.SpendMoney(converterConsumeRatePrice);
            converter.consumeRate += 0.2f;

            CheckCap(converter.consumeRate, 3f, converterConsumeRateButton, converterConsumeRatePrice);
        }
        converterConsumeRatePrice += (int)(1.2f * converterConsumeRatePrice);
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

    public void CheckCapWorkerSpawn()
    {
        if (workerList.Count <= 0)
        {
            workerSpawnButton.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
            workerSpawnButton.interactable = false;
        }
        else
        {
            workerSpawnButton.GetComponentInChildren<TextMeshProUGUI>().text = "$" + workerSpawnPrice;
            workerSpawnButton.interactable = true;
        }
    }

    public bool CheckMoney(int price, string tag, Vector3 position)
    {
        if (Player.Instance.currenetMoney < price)
        {
            moneyNeedWarnEffect.SetActive(true);
            moneyNeedWarnEffect.transform.position = new Vector3(position.x + 70, position.y, position.z);
            moneyNeedWarnEffect.transform.DOMove(new Vector3(position.x + 75,position.y + 8, 0), 0.2f).SetEase(Ease.InSine);
            Tween tween = moneyNeedWarnEffect.transform.DOShakeScale(0.3f, 0.1f);
            tween.OnComplete(() => { moneyNeedWarnEffect.SetActive(false); });
            Debug.Log("More money is required to upgrade " + tag + "!");
            return false;
        }
        else
        {
            successfulBuyEffect.SetActive(true);
            successfulBuyEffect.transform.position = new Vector3(position.x + 70,position.y,position.z);
            successfulBuyEffect.transform.DOMove(new Vector3(position.x + 75, position.y + 8, 0), 0.2f).SetEase(Ease.InSine);
            Tween tween = successfulBuyEffect.transform.DOShakeScale(0.3f, 0.1f);
            tween.OnComplete(() => { successfulBuyEffect.SetActive(false); });
            return true;
        }
    }
    public void MatchPanelsAndShops()
    {
        shopList.Add(playerUpgradeShop);
        shopList.Add(workerUpgradeShop);
        shopList.Add(generatorUpgradeShop);
        shopList.Add(converterUpgradeShop);

        panelList.Add(playerPanel);
        panelList.Add(workerPanel);
        panelList.Add(generatorPanel);
        panelList.Add(converterPanel);
    }
    
    public void GetIn(GameObject upgrader, GameObject panel)
    {
        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        panel.SetActive(true);
    }

    public void GetOut(GameObject upgrader)
    {
        CharacterController cc = player.GetComponent<CharacterController>();
        Vector3 getOutPos = upgrader.transform.position + new Vector3(0,0,4f);
        Tween tween = player.transform.DOMove(getOutPos, 0.5f);
        tween.OnComplete(() => { cc.enabled = true;});
        Debug.DrawLine(player.transform.position, getOutPos);
    }
}