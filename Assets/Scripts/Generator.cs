using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [HideInInspector] 
    public List<GameObject> gameObjectList = new List<GameObject>();

    public GameObject prefab;
    [SerializeField] private Transform spawnPoint;

     public float rate;
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
            int rowCount = gameObjectList.Count / stackLimit;
            if (gameObjectList.Count < storageLimit)
            {
                GameObject temp = Instantiate(prefab, spawnPoint);
                temp.transform.position = new Vector3(spawnPoint.transform.position.x + (float)(rowCount * paddingX),
                                                     (gameObjectList.Count % stackLimit) * paddingY,
                                                     spawnPoint.transform.position.z + (float)(rowCount * paddingZ));
                gameObjectList.Add(temp);   
                
            }
            yield return new WaitForSeconds(1/rate);
        }
    }

    

    public void RemoveLast()
    {
        if (gameObjectList.Count > 0)
        {
            Destroy(gameObjectList[gameObjectList.Count - 1]);
            gameObjectList.RemoveAt(gameObjectList.Count - 1);
        }
    }
}
