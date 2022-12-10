using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Worker : MonoBehaviour
{
    #region Worker Variables
    public string workerName;
    public enum JobType { Gardener, Carrier};
    public enum TreeType { Apple, Orange, Banana };
    public JobType job;
    public TreeType targetTree;
    #endregion

    public Converter converter;
    private Collector collector;
    private FollowPath _followpath;
    public WPManager _wpManager;
    public ShopManager targetShop;

    public List<GameObject> wayPointList = new List<GameObject> ();
    public List<GameObject> generators = new List<GameObject>();

    public bool isFull;
    public bool isEmpty = true;
    private int genTurn = 1;

    private void Start()
    {
        _followpath = GetComponent<FollowPath>();
        generators = GameObject.FindGameObjectsWithTag("Generator").ToList();
        wayPointList = _wpManager.waypoints.ToList();
        collector = GetComponent<Collector>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(WaitTillEmptyThenGo());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        if (collector.backpack.Count >= collector.capacity)
            isFull = true;
        else
            isFull = false;

        if (collector.backpack.Count == 0)
            isEmpty = true;
        else
            isEmpty = false;
    }

    public IEnumerator WaitTillEmptyThenGo()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (isEmpty && job == JobType.Carrier)
            {
                _followpath?.GoTo(wayPointList.IndexOf(converter.convertWayPoint));
                StartCoroutine(WaitTillFullThenGo(targetShop.sellWaypoint));

                yield break;
            }

            if (isEmpty && job == JobType.Gardener)
            {
                if (genTurn == 1)
                {
                    GameObject closestGeneratorWP = CalculateClosest(generators, "Fertilizer1").GetComponent<Generator>().wayPoint;
                    _followpath?.GoTo(wayPointList.IndexOf(closestGeneratorWP));
                    StartCoroutine(WaitTillFullThenGo(converter.consumeWayPoint));

                    if (targetTree != TreeType.Apple)
                        genTurn++;

                    yield break;
                }
                if (genTurn == 2)
                {
                    GameObject closestGeneratorWP = CalculateClosest(generators, "Fertilizer2").GetComponent<Generator>().wayPoint;
                    _followpath?.GoTo(wayPointList.IndexOf(closestGeneratorWP));
                    StartCoroutine(WaitTillFullThenGo(converter.consumeWayPoint));

                    if (targetTree != TreeType.Orange)
                        genTurn++;
                    else genTurn = 1;

                    yield break;
                }
                if (genTurn == 3)
                {
                    Debug.Log("gen3");
                    GameObject closestGeneratorWP = CalculateClosest(generators, "Fertilizer3").GetComponent<Generator>().wayPoint;
                    _followpath?.GoTo(wayPointList.IndexOf(closestGeneratorWP));
                    StartCoroutine(WaitTillFullThenGo(converter.consumeWayPoint));
                    genTurn = 1;
                    yield break;
                }
            }
        }
    }

    public IEnumerator WaitTillFullThenGo(GameObject target)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (isFull)
            {
                _followpath?.GoTo(wayPointList.IndexOf(target));
                StartCoroutine(WaitTillEmptyThenGo());
                yield break;
            }   
        }
    }

    public GameObject CalculateClosest(List<GameObject> list, string tag)
    {
        List<GameObject> temp = new List<GameObject>();

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].gameObject.GetComponent<Generator>().prefabName == tag)
                temp.Add(list[i]);
        }

        float closest = Vector3.Distance(transform.position, temp[0].transform.position);

        int targetNumber = 0;

        for (int i = 0; i < temp.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, temp[i].transform.position);
            if (distance < closest)
            {
                closest = distance;
                targetNumber = i;
            }
        }
        return temp[targetNumber];
    }
}
