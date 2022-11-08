using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [HideInInspector] 
    public List<GameObject> resourceList = new List<GameObject>();

    public GameObject prefab;
    [SerializeField] private Transform spawnPoint;
        
     public float spawnRate;
     public float storageLimit;
     public int stackLimit;

    [SerializeField][Range(0f, 1f)] float paddingY;
    [SerializeField][Range(0f, 5f)] float paddingX;
    [SerializeField][Range(0f, 5f)] float paddingZ;

    private void Start()
    {
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / spawnRate);
            if (resourceList.Count < storageLimit)
            {
                int rowCount = resourceList.Count / stackLimit;
                GameObject temp = Instantiate(prefab, spawnPoint);
                temp.transform.position = new Vector3(spawnPoint.transform.position.x + (float)(rowCount * paddingX),
                                                     (resourceList.Count % stackLimit) * paddingY,
                                                     spawnPoint.transform.position.z + (float)(rowCount * paddingZ));
                resourceList.Add(temp);   
            }
        }
    }   

    public void RemoveLast()
    {
        if (resourceList.Count > 0)
        {
            Destroy(resourceList[resourceList.Count - 1]);
            resourceList.RemoveAt(resourceList.Count - 1);
        }
    }
}
