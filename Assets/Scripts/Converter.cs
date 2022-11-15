using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> endProductList = new List<GameObject>();

    public GameObject endProductPrefab;
    public string endProductName;
    public Transform spawnPoint;

    [SerializeField] private string source1Tag;
    [SerializeField] private string source2Tag;
    [SerializeField] private string source3Tag;
    private int currentSource1;
    private int currentSource2;
    private int currentSource3;

    enum SoureNeed { one, two, three }
    [SerializeField] SoureNeed soureNeed;

    [SerializeField] private bool oneSource;
    [SerializeField] private bool twoSource;
    [SerializeField] private bool threeSource;

    public List<Collector> collectorList = new List<Collector>();
    private Collider m_collider;

    [SerializeField] private float convertRate;
    [SerializeField] private float consumeRate;

    #region Positioning
    [SerializeField] private int stackLimit;
    [SerializeField][Range(0f, 1f)] private float paddingY;
    [SerializeField][Range(-2f, 2f)] private float paddingX;
    [SerializeField][Range(-2f, 2f)] private float paddingZ;
    #endregion

    private void OnEnable()
    {
        m_collider = GetComponent<Collider>();

        StartCoroutine(Consume());
        StartCoroutine(Convert());
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
            
            if ((oneSource || twoSource || threeSource))
            {
                foreach (var c in collectorList.ToArray())    
                {
                    Collector collector = c.GetComponent<Collector>();
                    for (int i = collector.backpack.Count - 1; i >= 0; i--)
                    {
                        GameObject go = collector.backpack[i];
                        if (go.CompareTag(source1Tag) && m_collider.bounds.Contains(c.gameObject.transform.position))
                        {
                            currentSource1++;
                            collector.RemoveOne(go);
                            UpdateUI();
                            yield return new WaitForSeconds(0.5f / consumeRate);
                        }
                    }
                }

                if ((twoSource || threeSource))
                {
                    foreach (var c in collectorList.ToArray())
                    {
                        Collector collector = c.GetComponent<Collector>();
                        for (int i = collector.backpack.Count - 1; i >= 0; i--)
                        {
                            GameObject go = collector.backpack[i];
                            if (go.CompareTag(source1Tag) && m_collider.bounds.Contains(c.gameObject.transform.position))
                            {
                                currentSource2++;
                                collector.RemoveOne(go);
                                UpdateUI();
                                yield return new WaitForSeconds(0.5f / consumeRate);
                            }
                        }
                    }
                }

                if ((threeSource))
                {
                    foreach (var c in collectorList.ToArray())
                    {
                        Collector collector = c.GetComponent<Collector>();
                        for (int i = collector.backpack.Count - 1; i >= 0; i--)
                        {
                            GameObject go = collector.backpack[i];
                            if (go.CompareTag(source1Tag) && m_collider.bounds.Contains(c.gameObject.transform.position))
                            {
                                currentSource3++;
                                collector.RemoveOne(go);
                                UpdateUI();
                                yield return new WaitForSeconds(0.5f / consumeRate);
                            }
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
        GameObject temp = ObjectPooler.Instance.SpawnFromPool(endProductName, spawnPoint.transform.position, spawnPoint.transform.rotation);
        temp.transform.position = new Vector3(spawnPoint.transform.position.x + (float)(rowCount * paddingX),
                                             (endProductList.Count % stackLimit) * paddingY,
                                             spawnPoint.transform.position.z + (float)(rowCount * paddingZ));
        temp.transform.SetParent(spawnPoint.transform);
        endProductList.Add(temp);
    }

    public void RemoveLast()
    {
        if (endProductList.Count > 0)
        {
            endProductList[endProductList.Count - 1].gameObject.SetActive(false);
            //Destroy(endProductList[endProductList.Count - 1]);
            endProductList.RemoveAt(endProductList.Count - 1);
        }
    }

    private void UpdateUI()
    {
        Debug.Log("UI Updated");
    }
}
