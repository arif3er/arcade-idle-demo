using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Truck : MonoBehaviour, ISaveable
{
    FollowPath _followPath;
    Collector playerCollector;

    [SerializeField] int orderCooldown;
    [SerializeField] int waitTime;
    public float timePassed;

    GameObject[] fruitPanels = new GameObject[3];
    TextMeshProUGUI[] fruitTexts = new TextMeshProUGUI[3];
    int[] orderAmounts = new int[3];
    int[] randoms = new int[3];
    int _length = 1;

    Vector3Int currentAmounts = new Vector3Int(0,0,0);
    bool inArea;

    private void Start()
    {
        playerCollector = Player.Instance.GetComponent<Collector>();
        _followPath = GetComponent<FollowPath>();

        for (int i = 0; i < fruitPanels.Length; i++)
        {
            fruitPanels[i] = this.transform.GetChild(0).GetChild(i).gameObject;
            fruitTexts[i] = fruitPanels[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        StartCoroutine(TimeTick());
        StartCoroutine(SendTruck());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inArea = false;
        }
    }

    IEnumerator SendTruck()
    {
        _followPath.GoTo(0);

        yield return ArifHelpers.GetWait(orderCooldown);
        CreateOrder();
        StartCoroutine(GetFruits());

        _followPath.GoTo(7);
    }

    void CreateOrder()
    {
        if (timePassed >= 1200) // 20 min
            _length = 3;
        else if (timePassed >= 600) // 10 min
            _length = 2;

        for (int i = 0; i < _length; i++)
        {
            randoms[i] = UnityEngine.Random.Range(10, 20);
            orderAmounts[i] = randoms[i];
        }

        for (int i = 0; i < _length; i++)
        {
            fruitPanels[i].SetActive(true);
            fruitTexts[i].text = currentAmounts[i] + "/" + randoms[i].ToString();
        }
    }

    IEnumerator GetFruits()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (inArea)
            {
                for (int i = playerCollector.backpack.Count - 1; i >= 0; i--)
                {
                    GameObject go = playerCollector.backpack[i];
                    yield return ArifHelpers.GetWait(0.1f);

                    if (_length == 1 && go.CompareTag("Apple"))
                    {
                        Tween tween = go.transform.DOJump(transform.position, 1f, 1, 0.2f).SetEase(Ease.InOutSine);
                        tween.OnComplete(() => { playerCollector.RemoveOne(go); });

                        currentAmounts[0]++;
                        fruitTexts[0].text = currentAmounts[0] + "/" + randoms[0].ToString();

                        if (currentAmounts[0] == randoms[0])
                        {
                            Player.Instance.AddMoney(currentAmounts[0] * 100);
                            currentAmounts[0] = 0;
                            StartCoroutine(SendTruck());
                            yield break;
                        }
                    }
                    else if (_length == 2)
                    {
                        if (go.CompareTag("Apple"))
                        {
                            Tween tween = go.transform.DOJump(transform.position, 1f, 1, 0.2f).SetEase(Ease.InOutSine);
                            tween.OnComplete(() => { playerCollector.RemoveOne(go); });

                            currentAmounts[0]++;
                            fruitTexts[0].text = currentAmounts[0] + "/" + randoms[0].ToString();
                        }
                        else if(go.CompareTag("Orange"))
                        {
                            Tween tween = go.transform.DOJump(transform.position, 1f, 1, 0.2f).SetEase(Ease.InOutSine);
                            tween.OnComplete(() => { playerCollector.RemoveOne(go); });

                            currentAmounts[1]++;
                            fruitTexts[1].text = currentAmounts[1] + "/" + randoms[1].ToString();
                        }

                        if (currentAmounts[0] == randoms[0] && currentAmounts[1] == randoms[1])
                        {
                            Player.Instance.AddMoney(currentAmounts[0] * 100 + currentAmounts[1] * 150);
                            currentAmounts[0] = 0;
                            currentAmounts[1] = 0;
                            StartCoroutine(SendTruck());
                            yield break;
                        }
                    }
                    else if (_length == 3)
                    {
                        if (go.CompareTag("Apple") && currentAmounts[0] < randoms[0])
                        {
                            Tween tween = go.transform.DOJump(transform.position, 1f, 1, 0.2f).SetEase(Ease.InOutSine);
                            tween.OnComplete(() => { playerCollector.RemoveOne(go); });

                            currentAmounts[0]++;
                            fruitTexts[0].text = currentAmounts[0] + "/" + randoms[0].ToString();

                        }
                        else if (go.CompareTag("Orange") && currentAmounts[1] < randoms[1])
                        {
                            Tween tween = go.transform.DOJump(transform.position, 1f, 1, 0.2f).SetEase(Ease.InOutSine);
                            tween.OnComplete(() => { playerCollector.RemoveOne(go); });

                            currentAmounts[1]++;
                            fruitTexts[1].text = currentAmounts[1] + "/" + randoms[1].ToString();

                        }
                        else if (go.CompareTag("Banana") && currentAmounts[2] < randoms[2])
                        {
                            Tween tween = go.transform.DOJump(transform.position, 1f, 1, 0.2f).SetEase(Ease.InOutSine);
                            tween.OnComplete(() => { playerCollector.RemoveOne(go); });

                            currentAmounts[2]++;
                            fruitTexts[2].text = currentAmounts[2] + "/" + randoms[2].ToString();
                        }

                        if (currentAmounts[0] == randoms[0] && currentAmounts[1] == randoms[1] && currentAmounts[2] == randoms[2]) 
                        {
                            Player.Instance.AddMoney(currentAmounts[0] * 100 + currentAmounts[1] * 150 + currentAmounts[2] * 250);
                            currentAmounts[0] = 0;
                            currentAmounts[1] = 0;
                            currentAmounts[2] = 0;
                            StartCoroutine(SendTruck());
                            yield break;
                        }
                    }
                }
            }
        }
    }

    IEnumerator TimeTick()
    {
        while (true)
        {
            if (timePassed > 1200) yield break;

            yield return ArifHelpers.GetWait(1);
            timePassed += 1;
        }
    }

    #region Saving System

    [Serializable]
    private struct SaveData
    {
        public float timePassed;
    }

    public object CaptureState()
    {
        return new SaveData
        {
            timePassed = timePassed
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        timePassed = saveData.timePassed;
    }
    #endregion
}
