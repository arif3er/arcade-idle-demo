using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyArea : MonoBehaviour
{
    [SerializeField] private GameObject lockedObject;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image bar;

    [SerializeField] private int moneySpended;
    [SerializeField] private int moneyNeed;
    [SerializeField] private float collectRate;

    private bool inArea;

    private void Start()
    {
        lockedObject.SetActive(false);
        StartCoroutine(GetMoney());
        UpdateUI(moneySpended, moneyNeed);
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

    IEnumerator GetMoney()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            if (moneySpended >= moneyNeed)
            {
                StopAllCoroutines();
                lockedObject.SetActive(true);
                gameObject.SetActive(false);
            }
            if (inArea && Player.Instance.currenetMoney > 0)
            {
                moneySpended++;
                UpdateUI(moneySpended, moneyNeed);
                Player.Instance.SpendMoney(1);
                Player.Instance.UpdateMoneyText();
            }
        }
    }

    private void UpdateUI(float current, int need)
    {
        text.text = current + " / " + need + " $";
        bar.fillAmount = current/need;
    }
}
