using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyArea : MonoBehaviour, ISaveable
{
    [SerializeField] private GameObject lockedObject;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image bar;
    [SerializeField] private ParticleSystem partyEffect;

    public int moneySpended;
    [SerializeField] private int moneyNeed; 

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
                lockedObject.transform.DOShakeScale(0.5f, 1);
                partyEffect.transform.position = this.transform.position;
                partyEffect.Play(inArea);
                gameObject.SetActive(false);
            }
            if (inArea && Player.Instance.currentMoney >= (moneyNeed / 100) && moneySpended < moneyNeed)
            {
                Player.Instance.SpendMoney((moneyNeed / 100));
                GameObject money = ObjectPooler.Instance.SpawnFromPool("Money", Player.Instance.transform.position, this.transform.rotation);
                Tween tw = money.transform.DOMove(this.transform.position, 0.25f);
                tw.OnComplete(() => money.SetActive(false));
                Player.Instance.UpdateInventoryText();
                moneySpended += (moneyNeed / 100);
                UpdateUI(moneySpended, moneyNeed);
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
