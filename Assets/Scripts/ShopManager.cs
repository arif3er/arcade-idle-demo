using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI appleText;
    [SerializeField] private float collectRate;

    private Collector collector;

    public int currentApple = 0;
    public int currentMoney = 0;
    private bool isCollecting;

    private void Start()
    {
        StartCoroutine(Collect("Apple"));
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
            isCollecting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isCollecting = false;
        }
    }

    IEnumerator Collect(string resourceName)
    {
        while (true)
        {
            if (isCollecting)
            {
                 foreach (var item in collector.gameObjectList.ToArray())
                 {
                     if (item.gameObject.CompareTag(resourceName) && isCollecting)
                     {
                        currentApple++;
                        collector.gameObjectList.Remove(item);
                        Destroy(item);
                        Debug.Log(currentApple);
                        collector.ReOrder();
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
            if (currentApple > 0)
            {
                currentApple--;
                currentMoney++;
                Debug.Log("Money : " + currentMoney);
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
