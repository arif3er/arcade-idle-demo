using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> resourceList = new List<GameObject>();

    public string prefabName;
    public GameObject wayPoint;
    [SerializeField] private Transform spawnPoint;
        
     public float spawnRate;
     public float storageLimit;
     public int stackLimit;

    [SerializeField][Range(0f, 1f)] float paddingY;
    [SerializeField][Range(-2f, 2f)] float paddingX;
    [SerializeField][Range(-2f, 2f)] float paddingZ;

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
                GameObject temp = ObjectPooler.Instance.SpawnFromPool(prefabName, spawnPoint.transform.position, spawnPoint.transform.rotation);
                temp.transform.DOShakeScale(0.2f,0.1f);
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
            resourceList[resourceList.Count- 1].gameObject.SetActive(false);
            resourceList.RemoveAt(resourceList.Count - 1);
        }
    }
}
