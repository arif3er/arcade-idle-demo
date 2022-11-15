using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public struct WorkerCard
    {
        public string name;
        public enum job { GARDENER, CARRIER};
        public job description;
        public int age;

        public WorkerCard(string name, WorkerCard.job description, int age)
        {
            this.name = name;
            this.description = description;
            this.age = age;
        }
    }

    private FollowPath followPath;
    private List<float> distance = new List<float>();
    GameObject[] trees;

    private void Start()
    {
        followPath = GetComponent<FollowPath>();

        if (followPath != null)
        {
            Debug.Log("Not null");
        }
        else
        {
            Debug.Log("Its null");
        }
        //followPath.GoTo(1);
    }

    public void GardenerAITest()
    {
        for (int i = 0; i < trees.Length; i++)
        {
            //trees[i].GetComponent<Converter>().

        }
    }

    private void GetAllTrees(string targetTrees)
    {
       // trees = GameObject.FindObjectsOfType<Converter>(targetTrees);
    }
}
