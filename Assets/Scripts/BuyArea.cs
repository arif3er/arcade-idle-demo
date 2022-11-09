using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyArea : MonoBehaviour
{
    [SerializeField] private GameObject lockedObject;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image bar;

    [SerializeField] private string resourceName;
    [SerializeField] private int resourceNeed;
    [SerializeField] private float collectRate;

    private Collector playerCollector;
    private Collector workerCollector;

    public int currentResource = 0;
    private bool inAreaPlayer;
    private bool inAreaWorker;

    private void Start()
    {
        lockedObject.SetActive(false);
        StartCoroutine(ConsumeFromPlayer(resourceName));
        StartCoroutine(ConsumeFromWorker(resourceName));
        UpdateUI(currentResource, resourceNeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerCollector = other.GetComponent<Collector>();
        }
        if (other.gameObject.CompareTag("Worker"))
        {
            Debug.Log("worker entered");
            workerCollector = other.GetComponent<Collector>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!PlayerController.Instance.isMoving)
                inAreaPlayer = true;
            else
                inAreaPlayer = false;
        }
        if (other.gameObject.CompareTag("Worker"))
        {
            if (!PlayerController.Instance.isMoving)
                inAreaWorker = true;
            else
                inAreaWorker = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inAreaPlayer = false;
        }
        if (other.gameObject.CompareTag("Worker"))
        {
            inAreaWorker = false;
        }
    }

    IEnumerator ConsumeFromPlayer(string tag)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.0001f);
            if (currentResource >= resourceNeed)
            {
                StopAllCoroutines();
                lockedObject.SetActive(true);
                gameObject.SetActive(false);
            }
            if (inAreaPlayer && playerCollector.backpack.Count > 0)
            {
                for (int i = playerCollector.backpack.Count - 1; i >= 0; i--)
                {
                    GameObject go = playerCollector.backpack[i];
                    if (go.CompareTag(tag) && inAreaPlayer && currentResource <= resourceNeed)
                    {
                        currentResource++;
                        playerCollector.Remove(go);
                        UpdateUI(currentResource, resourceNeed);
                        yield return new WaitForSeconds(1f / collectRate);
                    }
                }
            }
        }
    }

    IEnumerator ConsumeFromWorker(string tag)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.0001f);
            if (currentResource >= resourceNeed)
            {
                StopAllCoroutines();
                lockedObject.SetActive(true);
                gameObject.SetActive(false);
            }
            if (inAreaWorker && workerCollector.backpack.Count > 0)
            {
                for (int i = workerCollector.backpack.Count - 1; i >= 0; i--)
                {
                    GameObject go = workerCollector.backpack[i];
                    if (go.CompareTag(tag) && inAreaWorker && currentResource <= resourceNeed)
                    {
                        currentResource++;
                        workerCollector.Remove(go);
                        UpdateUI(currentResource, resourceNeed);
                        yield return new WaitForSeconds(1f / collectRate);
                    }
                }
            }
        }
    }

    IEnumerator Consume(Collector collector, string tag, bool inArea)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.0001f);
            if (currentResource >= resourceNeed)
            {
                StopAllCoroutines();
                lockedObject.SetActive(true);
                gameObject.SetActive(false);
            }
            if (inArea && collector.backpack.Count > 0)
            {
                for (int i = collector.backpack.Count - 1; i >= 0; i--)
                {
                    GameObject go = collector.backpack[i];
                    if (go.CompareTag(tag) && inArea && currentResource <= resourceNeed)
                    {
                        currentResource++;
                        collector.Remove(go);
                        UpdateUI(currentResource, resourceNeed);
                        yield return new WaitForSeconds(1f / collectRate);
                    }
                }
            }
        }
    }

    private void UpdateUI(float current, int need)
    {
        text.text = current + " / " + need;
        bar.fillAmount = current/need;
    }
}
