using System.Collections;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI appleText;
    [SerializeField] private float collectRate;

    private Collector collector;

    public string fruitName;
    public int currentFruit = 0;
    public int currentMoney = 0;

    private bool isCollecting;

    private void Start()
    {
        StartCoroutine(Collect(fruitName));
        StartCoroutine(ProduceMoney());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collector = other.GetComponent<Collector>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!PlayerController.Instance.isMoving)
                isCollecting = true;
            else
                isCollecting = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isCollecting = false;
        }
    }

    IEnumerator Collect(string fruitName)
    {
        while (true)
        {
            if (isCollecting)
            {
                 foreach (var item in collector.backpack.ToArray())
                 {
                     if (item.gameObject.CompareTag(fruitName) && isCollecting)
                     {
                        currentFruit++;
                        collector.backpack.Remove(item);
                        Destroy(item);
                        yield return new WaitForSeconds(0.5f / collectRate);
                     }
                 }
            }
            yield return new WaitForSeconds(0.5f / collectRate);
        }
    }

    IEnumerator ProduceMoney()
    {
        while (true)
        {
            if (currentFruit > 0)
            {
                currentFruit--;
                currentMoney++;
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
