using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public List<GameObject> gameObjectList = new List<GameObject>();

    public Transform spawnPoint;
    private GameObject prefab;

    [SerializeField] 
    private float collectRate;
    [SerializeField]
    private int storageLimit;

    [Range(0f, 1f)] 
    public float paddingY;

    private bool isCollecting;

    private Generator generator;

    private void Start()
    {
        StartCoroutine(CollectEnum());
    }

    IEnumerator CollectEnum()
    {
        while (true)
        {
            if (isCollecting)
            {
                Collect();
            }
            yield return new WaitForSeconds(1 / collectRate);
        }
    }

    public void ReOrder()
    {
        for (int i = 0; i < gameObjectList.Count; i++)
        {
            gameObjectList[i].transform.localPosition = new Vector3(0, paddingY * i, 0);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Generator"))
        {
            generator = other.GetComponent<Generator>();
            prefab = generator.prefab;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Generator"))
        {
            isCollecting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Generator"))
        {
            isCollecting = false;
        }
    }

    void Collect()
    {
        if (gameObjectList.Count < storageLimit && generator.gameObjectList.Count > 0)
        {
            GameObject temp = Instantiate(prefab, spawnPoint);
            //temp.transform.position = new Vector3(spawnPoint.transform.position.x, gameObjectList.Count * paddingY, spawnPoint.transform.position.z);
            gameObjectList.Add(temp);
            generator.RemoveLast();
            ReOrder();
        }
    }

    public void RemoveLast()
    {
        Destroy(gameObjectList[gameObjectList.Count - 1]);
        gameObjectList.RemoveAt(gameObjectList.Count - 1);
    }
}
