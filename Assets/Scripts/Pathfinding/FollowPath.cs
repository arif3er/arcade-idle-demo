using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    private Transform goal;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float accuracy = 3f;
    [SerializeField] private float rotSpeed = 0.5f;
    public GameObject wpManager;
    private GameObject[] wps;
    private GameObject currentNode;
    private int currentWP = 0;
    private Graph g;

    private void Start()
    {
        SetupThings();
    }

    public void GoTo(int wpIndex)
    {
        if (wps == null)
            SetupThings();

        g.AStar(currentNode, wps[wpIndex]);
        currentWP = 0;
    }

    private void LateUpdate()
    {
        if (g.getPathLength() == 0 || currentWP == g.getPathLength()) return;

        //The node we are closest to at this moment
        currentNode = g.getPathPoint(currentWP);

        //If we are close enough to the current waypoint move to next
        if (Vector3.Distance(
            g.getPathPoint(currentWP).transform.position,
            transform.position) < accuracy)
        {
            currentWP++;
        }

        //If we are not at the end of the path
        if (currentWP < g.getPathLength())
        {
            goal = g.getPathPoint(currentWP).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x,
                                             this.transform.position.y,
                                             goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    Time.deltaTime * 3 * rotSpeed);

            this.transform.Translate(0,0, speed *Time.deltaTime);
        }
    }

    void SetupThings()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints;
        g = wpManager.GetComponent<WPManager>().graph;
        currentNode = wps[0];
    }
}
