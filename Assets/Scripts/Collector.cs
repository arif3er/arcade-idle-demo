using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public List<GameObject> backpack = new List<GameObject>();

    [SerializeField]
    private Transform spawnPoint;
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
        Collector[] otherCollectors = GameObject.FindObjectsOfType<Collector>();
        for (int i = 0; i < otherCollectors.Length; i++)
            Physics.IgnoreCollision(otherCollectors[i].GetComponent<Collider>(), GetComponent<Collider>());

        StartCoroutine(CollectEnum());
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
    IEnumerator CollectEnum()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / collectRate);
            if (isCollecting)
            {
                Collect();
            }
        }
    }

    void Collect()
    {
        if (backpack.Count < storageLimit && generator.resourceList.Count > 0)
        {
            GameObject temp = Instantiate(prefab, spawnPoint);
            temp.transform.position = new Vector3(spawnPoint.transform.position.x, backpack.Count * paddingY, spawnPoint.transform.position.z);
            backpack.Add(temp);
            generator.RemoveLast();
            ReOrder();
        }
    }

    private void ReOrder()
    {
        for (int i = 0; i < backpack.Count; i++)
        {
            backpack[i].transform.localPosition = new Vector3(0, paddingY * i, 0);
        }
    }

    public void RemoveLast()
    {
        Destroy(backpack[backpack.Count - 1]);
        backpack.RemoveAt(backpack.Count - 1);
        ReOrder();
    }
        
    public void Remove(GameObject resource)
    {
        Destroy(resource);
        backpack.Remove(resource);
        ReOrder();
    }
}
