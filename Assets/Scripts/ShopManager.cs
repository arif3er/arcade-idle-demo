using System.Collections;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fruitText;
    [SerializeField] private float collectRate;

    private Collector collector;

    public string fruitName;
    public int fruitPrice = 0;
    public int currentFruit = 0;

    private bool inArea;

    private void Start()
    {
        StartCoroutine(Collect(fruitName));
        UpdateUI();
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
                inArea = true;
            else
                inArea = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inArea = false;
        }
    }

    IEnumerator Collect(string fruitName)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f / collectRate);
            if (inArea)
            {
                for (int i = collector.backpack.Count - 1; i >= 0; i--)
                {
                    GameObject go = collector.backpack[i];
                    if (go.CompareTag(fruitName) && inArea)
                    {
                        currentFruit++;
                        collector.backpack.Remove(go);
                        Destroy(go);
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
