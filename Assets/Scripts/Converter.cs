using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;


public class Converter : MonoBehaviour
{
    public List<GameObject> endProductList = new List<GameObject>();
    public GameObject consumeWayPoint;
    public GameObject convertWayPoint;
    public Worker[] workers;

    public string endProductName;
    public Transform spawnPoint;

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

    [SerializeField] private float convertRate;
    [SerializeField] private float consumeRate;

    [SerializeField] private int stackLimit;
    [SerializeField][Range(0f, 1f)] private float paddingY;
    [SerializeField][Range(-2f, 2f)] private float paddingX;
    [SerializeField][Range(-2f, 2f)] private float paddingZ;

    private void OnEnable()
    {
        _collider = GetComponent<Collider>();

        StartCoroutine(Consume());
        StartCoroutine(Convert());
        UpdateUI();
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

    IEnumerator ConsumeAlternate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / consumeRate);

            foreach (var c in collectorList.ToArray())
            {
                Collector collector = c.GetComponent<Collector>();
                for (int i = collector.backpack.Count - 1; i >= 0; i--)
                {
                    yield return new WaitForSeconds(1 / consumeRate);

                    if (_collider.bounds.Contains(c.gameObject.transform.position))
                    {
                        GameObject go = collector.backpack[i];
                        if ((sourceNeed == SourceNeed.One || sourceNeed == SourceNeed.Two || sourceNeed == SourceNeed.Three))
                        {
                            if (go.CompareTag(sourceType1.ToString()))
                            {
                                Tween tween = go.transform.DOJump(transform.position, 1f, 1, 0.2f).SetEase(Ease.InOutSine);
                                tween.OnComplete(() => { collector.RemoveOne(go); });
                                currentSource1++;
                                UpdateUI();
                            }
                        }
                        if ((sourceNeed == SourceNeed.Two || sourceNeed == SourceNeed.Three))
                        {
                            if (go.CompareTag(sourceType2.ToString()))
                            {
                                Tween tween = go.transform.DOJump(transform.position, 1f, 1, 0.2f).SetEase(Ease.InOutSine);
                                tween.OnComplete(() => { collector.RemoveOne(go); });
                                currentSource2++;
                                collector.RemoveOne(go);
                                UpdateUI();
                            }
                        }
                        if ((sourceNeed == SourceNeed.Three))
                        {
                            if (go.CompareTag(sourceType3.ToString()))
                            {
                                Tween tween = go.transform.DOJump(transform.position, 1f, 1, 0.2f).SetEase(Ease.InOutSine);
                                tween.OnComplete(() => { collector.RemoveOne(go); });
                                currentSource3++;
                                collector.RemoveOne(go);
                                UpdateUI();
                            }
                        }
                    }
                }
            }
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
                ProduceEndProduct();
            }
            if (sourceNeed == SourceNeed.Two && currentSource1 > 0 && currentSource2 > 0)
            {
                currentSource1--;
                currentSource2--;
                ProduceEndProduct();
            }
            if (sourceNeed == SourceNeed.Three && currentSource1 > 0 && currentSource2 > 0 && currentSource3 > 0)
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
        GameObject temp = ObjectPooler.Instance.SpawnFromPool(Utility.ToTitleCase(endProduct.ToString()), spawnPoint.transform.position, 
                                                                                                          spawnPoint.transform.rotation);
        temp.transform.DOShakeScale(0.2f, 0.1f);
        temp.transform.position = new Vector3(spawnPoint.transform.position.x + (float)(rowCount * paddingX),
                                             (endProductList.Count % stackLimit) * paddingY,
                                             spawnPoint.transform.position.z + (float)(rowCount * paddingZ));
        temp.transform.SetParent(spawnPoint.transform);
        endProductList.Add(temp);
        UpdateUI();
    }

    public void RemoveLast()
    {
        if (endProductList.Count > 0)
        {
            endProductList[endProductList.Count - 1].gameObject.SetActive(false);
            endProductList.RemoveAt(endProductList.Count - 1);
        }
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
}
