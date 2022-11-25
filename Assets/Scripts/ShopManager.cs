using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fruitText;
    [SerializeField] private float collectRate;

    private List<Collector> collectorList = new List<Collector>();

    public GameObject sellWaypoint;
    public GameObject buyWaypoint;

    private Collider _collider;

    public string fruitName;
    public int fruitPrice = 0;
    public int currentFruit = 0;

    private void Start()
    {
        _collider = GetComponent<Collider>();

        StartCoroutine(Collect(fruitName));
        UpdateUI();
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

    IEnumerator Collect(string fruitName)
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / collectRate);

            foreach (var c in collectorList.ToArray())
            {
                Collector collector = c.GetComponent<Collector>();
                for (int i = collector.backpack.Count - 1; i >= 0; i--)
                {
                    GameObject go = collector.backpack[i];
                    if (go.CompareTag(fruitName) && _collider.bounds.Contains(c.gameObject.transform.position))
                    {
                        Tween tween = go.transform.DOJump(transform.position, 1f, 1, 0.2f).SetEase(Ease.InOutSine);
                        tween.OnComplete(() => { collector.RemoveOne(go); });
                        currentFruit++;
                        UpdateUI();
                        yield return new WaitForSeconds(0.5f / collectRate);
                    }
                }
            }
        }
    }
    
    public void Sell()
    {
        if (currentFruit > 0)
        {
            currentFruit--;
            UpdateUI();
            Player.Instance.AddMoney(fruitPrice);
        }
        else
        {
            Debug.Log("No fruit left.");
        }
    }

    private void UpdateUI()
    {
        fruitText.text = currentFruit.ToString();
    }
}
