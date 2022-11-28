using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyArea : MonoBehaviour, ISaveable
{
    [SerializeField] private GameObject lockedObject;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image bar;

    public int moneySpended;
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
            if (inArea && Player.Instance.currenetMoney > 0 && moneySpended < moneyNeed)
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

    #region Saving System

    [Serializable]
    private struct SaveData
    {
        public int moneySpended;
        public bool isActive;
    }

    public object CaptureState()
    {
        return new SaveData
        {
            moneySpended = moneySpended,
            isActive = gameObject.activeInHierarchy
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        moneySpended = saveData.moneySpended;
        gameObject.SetActive(saveData.isActive);
        UpdateUI(moneySpended, moneyNeed);
    }
    #endregion
}
