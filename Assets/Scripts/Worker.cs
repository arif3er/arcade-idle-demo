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

    public delegate void JobDone();
    public static event JobDone OnJobDone;

    private Collector collector;
    public Converter converter;
    public FollowPath _followpath;

    public List<GameObject> wayPointList = new List<GameObject> ();
    public List<GameObject> generators = new List<GameObject>();
    public List<GameObject> targets = new List<GameObject>();

    public bool isFull;
    public bool isEmpty = true;
    int genTurn = 1;

    private void Start()
    {
        if (job == JobType.Gardener)
            targets.Add(converter.consumeWayPoint);
        else if (job == JobType.Carrier)
            targets.Add(converter.convertWayPoint);

        _followpath = GetComponent<FollowPath>();

        generators = GameObject.FindGameObjectsWithTag("Generator").ToList();
        wayPointList = WPManager.Instance.waypoints.ToList();
        collector = GetComponent<Collector>();
        StartCoroutine(WaitTillEmptyThenGo());

        //gameObject.SetActive(false);
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

    public void RandomMoves()
    {
        int i = Random.Range(0, wayPointList.Count);
        _followpath.GoTo(i);
        Debug.Log(name + " " + i);
    }

    public IEnumerator WaitTillEmptyThenGo()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (isEmpty)
            {
                if (genTurn == 1)
                {
                    GameObject closestGeneratorWP = CalculateClosest(generators, "Water").GetComponent<Generator>().wayPoint;
                    _followpath.GoTo(wayPointList.IndexOf(closestGeneratorWP));
                    StartCoroutine(WaitTillFullThenGo(converter.consumeWayPoint));

                    if (targetTree != TreeType.Apple)
                        genTurn++;

                    yield break;
                }
                if (genTurn == 2)
                {
                    GameObject closestGeneratorWP = CalculateClosest(generators, "Fertilizer").GetComponent<Generator>().wayPoint;
                    _followpath.GoTo(wayPointList.IndexOf(closestGeneratorWP));
                    StartCoroutine(WaitTillFullThenGo(targets[0]));
                    genTurn++;

                    if (targetTree != TreeType.Orange)
                        genTurn++;
                    else genTurn = 1;

                    yield break;
                }
                if (genTurn == 3)
                {
                    GameObject closestGeneratorWP = CalculateClosest(generators, "Spray").GetComponent<Generator>().wayPoint;
                    _followpath.GoTo(wayPointList.IndexOf(closestGeneratorWP));
                    StartCoroutine(WaitTillFullThenGo(targets[0]));
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
                _followpath.GoTo(wayPointList.IndexOf(target));
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
