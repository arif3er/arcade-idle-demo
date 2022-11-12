using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Converter : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> endProductList = new List<GameObject>();

    public GameObject endProductPrefab;
    public Transform spawnPoint;

    [SerializeField] private string source1Tag;
    [SerializeField] private string source2Tag;
    [SerializeField] private string source3Tag;
    [SerializeField] private int currentSource1;
    [SerializeField] private int currentSource2;
    [SerializeField] private int currentSource3;
    [SerializeField] private int stockLimit;


    [SerializeField] private bool oneSource;
    [SerializeField] private bool twoSource;
    [SerializeField] private bool threeSource;

    private Collector playerCollector;
    private Collector workerCollector;

    [SerializeField] private float convertRate;
    [SerializeField] private float consumeRate;
    [SerializeField] private int stackLimit;

    [SerializeField][Range(0f, 1f)] private float paddingY;
    [SerializeField][Range(0f, 5f)] private float paddingX;
    [SerializeField][Range(0f, 5f)] private float paddingZ;

    private bool inAreaP;
    private bool inAreaW;

    private void Start()
    {
        StartCoroutine(Consume());
        StartCoroutine(Convert());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerCollector = other.GetComponent<Collector>();
        }
        if (other.gameObject.CompareTag("Worker"))
        {
            workerCollector = other.GetComponent<Collector>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!PlayerController.Instance.isMoving)
                inAreaP = true;
        }
        if (other.gameObject.CompareTag("Worker"))
        {
            if (!PlayerController.Instance.isMoving)
                inAreaW = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inAreaP = false;
        }
        if (other.gameObject.CompareTag("Worker"))
        {
            inAreaW = false;
        }
    }

    IEnumerator Consume()
    {
        while (true)
        {
            yield return new WaitForSeconds(1/ consumeRate);

            //Consumes resources from player
            if (inAreaP)
            {
                if ((oneSource || twoSource || threeSource) && currentSource1 < stockLimit)
                {
                    for (int i = playerCollector.backpack.Count - 1; i >= 0; i--)
                    {
                        GameObject go = playerCollector.backpack[i];
                        if (go.CompareTag(source1Tag) && inAreaP)
                        {
                            currentSource1++;
                            playerCollector.RemoveOne(go);
                            UpdateUI();
                            yield return new WaitForSeconds(1 / consumeRate);
                        }
                    }
                }

                if ((twoSource || threeSource) && currentSource2 < stockLimit)
                {
                    for (int i = playerCollector.backpack.Count - 1; i >= 0; i--)
                    {
                        GameObject go = playerCollector.backpack[i];
                        if (go.CompareTag(source2Tag) && inAreaP)
                        {
                            currentSource2++;
                            playerCollector.RemoveOne(go);
                            UpdateUI();
                            yield return new WaitForSeconds(1 / consumeRate);
                        }
                    }
                }

                if (threeSource && currentSource3 < stockLimit)
                {
                    for (int i = playerCollector.backpack.Count - 1; i >= 0; i--)
                    {
                        GameObject go = playerCollector.backpack[i];
                        if (go.CompareTag(source3Tag) && inAreaP)
                        {
                            currentSource3++;
                            playerCollector.RemoveOne(go);
                            UpdateUI();
                            yield return new WaitForSeconds(1 / consumeRate);
                        }
                    }
                }
            }

            //Consumes resources from worker
            if (inAreaW)
            {
                if ((oneSource || twoSource || threeSource) && currentSource1 < stockLimit)
                {
                    for (int i = workerCollector.backpack.Count - 1; i >= 0; i--)
                    {
                        GameObject go = workerCollector.backpack[i];
                        if (go.CompareTag(source1Tag) && inAreaW)
                        {
                            currentSource1++;
                            workerCollector.RemoveOne(go);
                            UpdateUI();
                            yield return new WaitForSeconds(1 / consumeRate);
                        }
                    }
                }

                if ((twoSource || threeSource) && currentSource2 < stockLimit)
                {
                    for (int i = workerCollector.backpack.Count - 1; i >= 0; i--)
                    {
                        GameObject go = workerCollector.backpack[i];
                        if (go.CompareTag(source2Tag) && inAreaW)
                        {
                            currentSource2++;
                            workerCollector.RemoveOne(go);
                            UpdateUI();
                            yield return new WaitForSeconds(1 / consumeRate);
                        }
                    }
                }

                if (threeSource && currentSource3 < stockLimit)
                {
                    for (int i = workerCollector.backpack.Count - 1; i >= 0; i--)
                    {
                        GameObject go = workerCollector.backpack[i];
                        if (go.CompareTag(source3Tag) && inAreaW)
                        {
                            currentSource3++;
                            workerCollector.RemoveOne(go);
                            UpdateUI();
                            yield return new WaitForSeconds(1 / consumeRate);
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
            if (oneSource && currentSource1 > 0)
            {
                currentSource1--;
                ProduceEndProduct();
            }

            if (twoSource && currentSource1 > 0 && currentSource2 > 0)
            {
                currentSource1--;
                currentSource2--;
                ProduceEndProduct();
            }

            if (threeSource && currentSource1 > 0 && currentSource2 > 0 && currentSource3 > 0)
            {
                currentSource1--;
                currentSource2--;
                currentSource3--;
                ProduceEndProduct();
            }
        }
    }

    private void ProduceEndProduct()
    {
        int rowCount = endProductList.Count / stackLimit;
        GameObject temp = Instantiate(endProductPrefab, spawnPoint);
        temp.transform.position = new Vector3(spawnPoint.transform.position.x + (float)(rowCount * paddingX),
                                             (endProductList.Count % stackLimit) * paddingY,
                                             spawnPoint.transform.position.z + (float)(rowCount * paddingZ));
        endProductList.Add(temp);
    }

    public void RemoveLast()
    {
        if (endProductList.Count > 0)
        {
            Destroy(endProductList[endProductList.Count - 1]);
            endProductList.RemoveAt(endProductList.Count - 1);
        }
    }

    private void UpdateUI()
    {
        Debug.Log("UI Updated");
    }
}
