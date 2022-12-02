using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Converter : MonoBehaviour, ISaveable
{
    public List<GameObject> endProductList = new List<GameObject>();
    public GameObject consumeWayPoint;
    public GameObject convertWayPoint;
    public Worker[] workers = new Worker[0];

    public string endProductName;
    public Transform spawnPoint;
    public Transform[] spawnPoints = new Transform[0];
    public bool[] usedSpawnPoints = new bool[20];
    public List<int> spawnInfo = new List<int>();

    public TextMeshProUGUI sourceText1;
    public TextMeshProUGUI sourceText2;
    public TextMeshProUGUI sourceText3;

    public enum EndProduct { Apple, Orange, Banana };
    public EndProduct endProduct;
    public enum SourceNeed { One, Two, Three };
    public SourceNeed sourceNeed;
    public enum SourceType { Water, Fertilizer, Spray };
    public SourceType sourceType1;
    public SourceType sourceType2;
    public SourceType sourceType3;

    private int currentSource1;
    private int currentSource2;
    private int currentSource3;

    public List<Collector> collectorList = new List<Collector>();
    private Collider _collider;

    public int capacity;
    public float convertRate;
    public float consumeRate;

    [SerializeField] private int stackLimit;
    [SerializeField][Range(0f, 1f)] private float paddingY;
    [SerializeField][Range(-2f, 2f)] private float paddingX;
    [SerializeField][Range(-2f, 2f)] private float paddingZ;

    private Vector3 scaleFactor;

    private void OnEnable()
    {
        _collider = GetComponent<Collider>();

        StartCoroutine(Consume());
        StartCoroutine(Convert());
        UpdateUI();
        SetScaleFactor();
    }

    private void Start()
    {
        for (int i = 0; i < workers.Length; i++)
        {
            if (workers[i] != null)
            {
                Upgrader.Instance.workerList.Add(workers[i]);
                Upgrader.Instance.CheckCapWorkerSpawn();
                Debug.Log("Worker " + workers[i].workerName + " added to Upgrader list.");
            }
            else
                Debug.Log("Worker slot is empty !");
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Worker"))
        {
            collectorList.Add(other.GetComponent<Collector>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Worker"))
        {
            collectorList.Remove(other.GetComponent<Collector>());
        }
    }

    IEnumerator Consume()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / consumeRate);
            
            if (sourceNeed == SourceNeed.One || sourceNeed == SourceNeed.Two || sourceNeed == SourceNeed.Three)
            {
                foreach (var c in collectorList.ToArray())    
                {
                    Collector collector = c.GetComponent<Collector>();
                    for (int i = collector.backpack.Count - 1; i >= 0; i--)
                    {
                        GameObject go = collector.backpack[i];
                        if (go.CompareTag(sourceType1.ToString()) && _collider.bounds.Contains(c.gameObject.transform.position))
                        {
                            Tween tween = go.transform.DOJump(transform.position, 1f, 1, 0.2f).SetEase(Ease.InOutSine);
                            tween.OnComplete(() => { collector.RemoveOne(go); });
                            currentSource1++;
                            UpdateUI();
                        }
                        yield return new WaitForSeconds(0.5f / consumeRate);
                    }
                }
            }
            if ((sourceNeed == SourceNeed.Two || sourceNeed == SourceNeed.Three))
            {
                foreach (var c in collectorList.ToArray())
                {
                    Collector collector = c.GetComponent<Collector>();
                    for (int i = collector.backpack.Count - 1; i >= 0; i--)
                    {
                        GameObject go2 = collector.backpack[i];
                        if (go2.CompareTag(sourceType2.ToString()) && _collider.bounds.Contains(c.gameObject.transform.position))
                        {
                            Tween tween2 = go2.transform.DOJump(transform.position, 1f, 1, 0.2f).SetEase(Ease.InOutSine);
                            tween2.OnComplete(() => { collector.RemoveOne(go2); });
                            currentSource2++;
                            UpdateUI();
                            yield return new WaitForSeconds(0.5f / consumeRate);
                        }
                    }
                }
            }
            if ((sourceNeed == SourceNeed.Three))
            {
                foreach (var c in collectorList.ToArray())
                {
                    Collector collector = c.GetComponent<Collector>();
                    for (int i = collector.backpack.Count - 1; i >= 0; i--)
                    {
                        GameObject go = collector.backpack[i];
                        if (go.CompareTag(sourceType3.ToString()) && _collider.bounds.Contains(c.gameObject.transform.position))
                        {
                            Tween tween = go.transform.DOJump(transform.position, 1f, 1, 0.2f).SetEase(Ease.InOutSine);
                            tween.OnComplete(() => { collector.RemoveOne(go); });
                            currentSource3++;
                            UpdateUI();
                            yield return new WaitForSeconds(0.5f / consumeRate);
                        }
                    }
                }
            }
        }
    }

    IEnumerator Convert()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / convertRate);
            if (sourceNeed == SourceNeed.One && currentSource1 > 0)
            {
                currentSource1--;
                UpdateUI();
                if (endProductList.Count < capacity) SpawnAtRandomSpawnPoint();
            }
            if (sourceNeed == SourceNeed.Two && currentSource1 > 0 && currentSource2 > 0)
            {
                currentSource1--;
                currentSource2--;
                UpdateUI();
                if (endProductList.Count < capacity) SpawnAtRandomSpawnPoint();
            }
            if (sourceNeed == SourceNeed.Three && currentSource1 > 0 && currentSource2 > 0 && currentSource3 > 0)
            {
                currentSource1--;
                currentSource2--;
                currentSource3--;
                UpdateUI();
                if (endProductList.Count < capacity) SpawnAtRandomSpawnPoint();
            }
        }
    }

    private void ProduceEndProduct()
    {
        int rowCount = endProductList.Count / stackLimit;
        GameObject temp = ObjectPooler.Instance.SpawnFromPool(ArifGDK.ToTitleCase(endProduct.ToString()), spawnPoint.transform.position, 
                                                                                                          spawnPoint.transform.rotation);
        temp.transform.DOShakeScale(0.2f, 0.1f);
        temp.transform.position = new Vector3(spawnPoint.transform.position.x + (float)(rowCount * paddingX),
                                             (endProductList.Count % stackLimit) * paddingY,
                                             spawnPoint.transform.position.z + (float)(rowCount * paddingZ));
        temp.transform.SetParent(spawnPoint.transform);
        endProductList.Add(temp);
    }

    void SpawnAtRandomSpawnPoint()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        int checkedPoints = 0;
        while (usedSpawnPoints[spawnIndex])
        { // If a spawn point is occupied
            spawnIndex = (spawnIndex + 1) % spawnPoints.Length; // Pick the next available spawn point
            checkedPoints++;
            if (checkedPoints >= spawnPoints.Length)
            {
                return; // No spawn points available
            }
        }
        
        SpawnAtPoint(spawnIndex);
    }

    void SpawnAtPoint(int index)
    {
        Transform spawnLocation = spawnPoints[index];
        GameObject fruit = ObjectPooler.Instance.SpawnFromPool(ArifGDK.ToTitleCase(endProduct.ToString()), spawnLocation.position,
                                                                                                           spawnLocation.rotation);
        //fruit.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 1f);
        fruit.transform.SetParent(spawnLocation);
        endProductList.Add(fruit);
        fruit.transform.localScale = scaleFactor;
        fruit.transform.DOShakeScale(0.3f, 0.1f);
        usedSpawnPoints[index] = true; // Mark spawn point as having been used
        spawnInfo.Add(index);
    }

    public void ResetSpawnPoint(int index)
    {
        usedSpawnPoints[index] = false;
    }

    private void SetScaleFactor()
    {
        if (endProduct == EndProduct.Apple)
            scaleFactor = new Vector3(0.5f, 0.5f, 0.5f);
        else if (endProduct == EndProduct.Orange)
            scaleFactor = Vector3.one;
        else if (endProduct == EndProduct.Banana)
            scaleFactor = Vector3.one;
    }


    private void UpdateUI()
    {
        if (sourceText1 != null)
            sourceText1.text = currentSource1.ToString();

        if (sourceText2 != null)
            sourceText2.text = currentSource2.ToString();

        if (sourceText3 != null)
            sourceText3.text = currentSource3.ToString();
    }


    #region Save System

    [Serializable]
    private struct SaveData
    {
        public int capacity;
        public float convertRate;
        public float consumeRate;
    }

    public object CaptureState()
    {
        return new SaveData
        {
            capacity = capacity,
            convertRate = convertRate,
            consumeRate = consumeRate
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        capacity = saveData.capacity;
        convertRate = saveData.convertRate;
        consumeRate = saveData.consumeRate;
    }
    #endregion
}
