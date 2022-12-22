using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Upgrader : MonoBehaviour
{
    public static Upgrader Instance { get; private set; }

    // Colliding
    public GameObject _ControllerPanel;

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
    public Button playerCollectRateButtonAds;

    public Button playerCapacityButton;
    public Button playerCapacityButtonAds;

    public Button playerSpeedButton;
    public Button playerSpeedButtonAds;

    //  Workers
    public List<Worker> workerToSpawnList = new List<Worker>();
    private GameObject[] allWorkers;

    public int workerSpawnPrice = 1000;
    public int workerCollectRatePrice = 500;
    public int workerCapacityPrice = 500;
    public int workerSpeedPrice = 500;

    public Button workerSpawnButton;
    public Button workerSpawnButtonAds;

    public Button workerCollectRateButton;
    public Button workerCollectRateButtonAds;

    public Button workerCapacityButton;
    public Button workerCapacityButtonAds;

    public Button workerSpeedButton;
    public Button workerSpeedButtonAds;

    //  Generators
    public List<Generator> generatorList = new List<Generator>();

    public int generatorRatePrice = 500;
    public int generatorCapacityPrice = 500;

    public Button generatorRateButton;
    public Button generatorRateButtonAds;

    public Button generatorCapacityButton;
    public Button generatorCapacityButtonAds;

    //  Converters
    public List<Converter> converterList = new List<Converter>();

    public int converterCapacityPrice = 500;
    public int converterConvertRatePrice = 500;
    public int converterConsumeRatePrice = 500;

    public Button converterCapacityButton;
    public Button converterCapacityButtonAds;

    public Button converterConvertRateButton;
    public Button converterConvertRateButtonAds;

    public Button converterConsumeRateButton;
    public Button converterConsumeRateButtonAds;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        MatchPanelsAndShops();
        allWorkers = GameObject.FindGameObjectsWithTag("Worker");
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < shopList.Count; i++)
        {
            if (ArifHelpers.DistanceTrigger(shopList[i], player.gameObject, 3) && shopList[i].activeInHierarchy)
                GetIn(shopList[i], panelList[i]);
            else
                panelList[i].SetActive(false);
        }
    }

    #region Player Upgrades

    public void UpgradePlayerCollectRate(int i)
    {
        if (i == 1 )
        {
            if (CheckMoney(playerCollectRatePrice, "player collectRate", playerCollectRateButton.transform.position))
                Player.Instance.SpendMoney(playerCollectRatePrice);
            else return;
        }

        playerCollectRatePrice += playerCollectRatePrice;

        player.GetComponent<Collector>().collectRate += 0.5f;

        CheckCap(player.GetComponent<Collector>().collectRate, 5, playerCollectRateButton, playerCollectRateButtonAds, playerCollectRatePrice);
    }

    public void UpgradePlayerCapacity(int i)
    {
        if (i == 1)
        {
            if (CheckMoney(playerCapacityPrice, "player capacity", playerCapacityButton.transform.position))
                Player.Instance.SpendMoney(playerCapacityPrice);
            else return;
        }

        playerCapacityPrice += (2 * playerCapacityPrice);

        player.GetComponent<Collector>().capacity += 5;

        CheckCap(player.GetComponent<Collector>().capacity, 50f, playerCapacityButton, playerCollectRateButtonAds, playerCapacityPrice);
    }

    public void UpgradePlayerSpeed(int i)
    {
        if (i == 1)
        {
            if (CheckMoney(playerSpeedPrice, "player speed", playerSpeedButton.transform.position))
                Player.Instance.SpendMoney(playerSpeedPrice);
            else return;
        }

        playerSpeedPrice += (int)(1.1f * playerSpeedPrice);

        player.GetComponent<PlayerController>().playerSpeed += 1f;

        CheckCap(player.GetComponent<PlayerController>().playerSpeed, 12f, playerSpeedButton, playerSpeedButtonAds, playerSpeedPrice);
    }

    #endregion

    #region Worker Upgrades

    public void SpawnWorker(int i)
    {
        if (i == 1 && workerToSpawnList.Count > 0)
        {
            if (CheckMoney(workerSpawnPrice, "worker spawn", workerSpawnButton.transform.position))
                Player.Instance.SpendMoney(workerSpawnPrice);
            else return;
        }

        if (workerToSpawnList.Count > 0)
        {
            workerSpawnPrice += (int)(1.25f * workerSpawnPrice);
            Worker wo = workerToSpawnList[0];
            workerToSpawnList.Remove(wo);
            wo.gameObject.SetActive(true);
            CheckCapWorkerSpawn();
        }
    }

    /*public void UpgradeWorkerCollectRate(int i)
    {
        if (i == 1)
        {
            if (CheckMoney(workerCollectRatePrice, "worker collectRate", workerCollectRateButton.transform.position))
                Player.Instance.SpendMoney(workerCollectRatePrice);
            else return;
        }

        foreach (var worker in workerList)
        {
            worker.GetComponent<Collector>().collectRate += 0.5f;

            CheckCap(worker.GetComponent<Collector>().collectRate, 30, workerCollectRateButton, workerCollectRateButtonAds, workerCollectRatePrice);
        }
        workerCollectRatePrice += (int)(1f * workerCollectRatePrice);
    }*/

    public void UpgradeWorkerCapacity(int i)
    {
        if (i == 1)
        {
            if (CheckMoney(workerCapacityPrice, "worker capacity", workerCapacityButton.transform.position))
                Player.Instance.SpendMoney(workerCapacityPrice);
            else return;
        }

        foreach (var worker in allWorkers)
        {
            worker.GetComponent<Collector>().capacity += 5;

            CheckCap(worker.GetComponent<Collector>().capacity, 30, workerCapacityButton, workerCapacityButtonAds, workerCapacityPrice);
        }

        workerCapacityPrice += (int)(1.5f * workerCapacityPrice);
    }

    public void UpgradeWorkerSpeed(int i)
    {
        if (i == 1)
        {
            if (CheckMoney(workerSpeedPrice, "worker speed", workerSpeedButton.transform.position))
                Player.Instance.SpendMoney(workerSpeedPrice);
            else return;
        }

        foreach (var worker in allWorkers)
        {
            worker.GetComponent<FollowPath>().speed += 0.5f;

            CheckCap(worker.GetComponent<FollowPath>().speed, 5f, workerSpeedButton, workerSpeedButtonAds, workerSpeedPrice);
        }

        workerSpeedPrice += (int)(1.2f * workerSpawnPrice);
    }
    #endregion

    #region Generator Upgrades

    public void UpgradeGeneratorRate(int i)
    {
        if (i == 1)
        {
            if (CheckMoney(generatorRatePrice, "generator rate", generatorRateButton.transform.position))
                Player.Instance.SpendMoney(generatorRatePrice);
            else return;
        }

        foreach (var generator in generatorList)
        {
            generator.spawnRate += 0.2f;

            CheckCap(generator.spawnRate, 5f, generatorRateButton, generatorRateButtonAds, generatorRatePrice);
        }
        generatorRatePrice += (int)(1.05f * generatorRatePrice);
    }

    public void UpgradeGeneratorCapacity(int i)
    {
        if (i == 1)
        {
            if (CheckMoney(generatorCapacityPrice, "generator capacity", generatorCapacityButton.transform.position))
                Player.Instance.SpendMoney(generatorCapacityPrice);
            else return;
        }

        foreach (var generator in generatorList)
        {
            generator.capacity += 5f;

            CheckCap(generator.capacity, 30, generatorCapacityButton, generatorCapacityButtonAds, generatorCapacityPrice);
        }
        generatorCapacityPrice += (int)(1.2f * generatorCapacityPrice);
    }
    #endregion

    #region Converter Upgrades

    public void UpgradeConverterCapacity(int i)
    {
        if (i == 1)
        {
            if (CheckMoney(converterCapacityPrice, "converter capacity", converterCapacityButton.transform.position))
                Player.Instance.SpendMoney(converterCapacityPrice);
            else return;
        }

        foreach (var converter in converterList)
        {
            converter.capacity += 5;

            CheckCap(converter.capacity, 30, converterCapacityButton, generatorCapacityButtonAds, converterCapacityPrice);
        }
        converterCapacityPrice += (int)(1.2f * converterCapacityPrice);
    }

    public void UpgradeConverterConvertRate(int i)
    {
        if (i == 1)
        {
            if (CheckMoney(converterConvertRatePrice, "converter convertRate", converterConvertRateButton.transform.position))
                Player.Instance.SpendMoney(converterConvertRatePrice);
            else return;
        }

        foreach (var converter in converterList)
        {
            converter.convertRate += 0.2f;

            CheckCap(converter.convertRate, 3f, converterConvertRateButton, converterConvertRateButtonAds, converterConvertRatePrice);
        }
        converterConvertRatePrice += (int)(1.2f * converterConvertRatePrice);
    }

    public void UpgradeConverterConsumeRate(int i)
    {
        if (i == 1)
        {
            if (CheckMoney(converterConsumeRatePrice, "converter consumeRate", converterConsumeRateButton.transform.position))
                Player.Instance.SpendMoney(converterConsumeRatePrice);
            else return;
        }

        foreach (var converter in converterList)
        {
            converter.consumeRate += 0.2f;

            CheckCap(converter.consumeRate, 3f, converterConsumeRateButton, converterConsumeRateButtonAds, converterConsumeRatePrice);
        }
        converterConsumeRatePrice += (int)(1.2f * converterConsumeRatePrice);
    }
    #endregion

    public void CheckCap(float upgraded, float cap, Button buttonNormal, Button buttonAds, float price)
    {
        if (upgraded >= cap)
        {
            buttonNormal.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
            buttonNormal.interactable = false;

            buttonAds.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
            buttonAds.interactable = false;

            RewardedAdsButton.Instance.showAdButtons.Remove(buttonAds);
        }
        else
        {
            buttonNormal.GetComponentInChildren<TextMeshProUGUI>().text = "$" + price;
        }
    }

    public void CheckCapWorkerSpawn()
    {
        if (workerToSpawnList.Count <= 0)
        {
            workerSpawnButton.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
            workerSpawnButton.interactable = false;

            workerSpawnButtonAds.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
            workerSpawnButtonAds.interactable = false;

            RewardedAdsButton.Instance.showAdButtons.Remove(workerSpawnButtonAds);
        }
        else
        {
            workerSpawnButton.GetComponentInChildren<TextMeshProUGUI>().text = "$" + workerSpawnPrice;
            workerSpawnButton.interactable = true;

            workerSpawnButtonAds.GetComponentInChildren<TextMeshProUGUI>().text = "FREE";
            workerSpawnButtonAds.interactable = true;

            if (!RewardedAdsButton.Instance.showAdButtons.Contains(workerSpawnButtonAds))
                RewardedAdsButton.Instance.showAdButtons.Remove(workerSpawnButtonAds);
        }
    }

    public bool CheckMoney(int price, string tag, Vector3 position)
    {
        if (Player.Instance.currentMoney < price)
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
        PlayerController pc = player.GetComponent<PlayerController>();
        Animator animator = player.gameObject.GetComponentInChildren<Animator>();
        CameraFollower cf = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollower>();

        pc.enabled = false;
        cf.positionOffset.y = 15;
        animator.SetBool("IsMoving", false);
        
        panel.SetActive(true);
    }

    public void GetOut(GameObject upgrader)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        CameraFollower cf = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollower>();

        cf.positionOffset.y = 20;

        Vector3 getOutPos = upgrader.transform.position + new Vector3(0,0,-4f);
        Tween tween = player.transform.DOMove(getOutPos, 0.5f);

        tween.OnComplete(() => { pc.enabled = true; });
    }
}