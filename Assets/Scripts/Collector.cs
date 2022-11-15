using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public List<GameObject> backpack = new List<GameObject>();

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float collectRate;
    [SerializeField] private int storageLimit;
    [Range(0f, 1f)] public float paddingY;

    private Generator generator;
    private Converter converter;
    private GameObject prefab;

    private bool isCollectingG;
    private bool isCollectingC;

    private void Start()
    {
        Collector[] otherCollectors = GameObject.FindObjectsOfType<Collector>();
        for (int i = 0; i < otherCollectors.Length; i++)
            Physics.IgnoreCollision(otherCollectors[i].GetComponent<Collider>(), GetComponent<Collider>());

        StartCoroutine(ResourceEnum());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Generator"))
        {
            generator = other.GetComponent<Generator>();
            prefab = generator.prefab;
        }
        if (other.gameObject.CompareTag("Converter"))
        {
            converter = other.GetComponentInParent<Converter>();
            prefab = converter.endProductPrefab;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Generator"))
        {
            if (!PlayerController.Instance.isMoving)    
                isCollectingG = true;
            else
                isCollectingG = false;
        }
        if (other.gameObject.CompareTag("Converter"))
        {
            if (!PlayerController.Instance.isMoving)
                isCollectingC = true;
            else
                isCollectingC = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Generator"))
        {
            isCollectingG = false;
        }
        if (other.gameObject.CompareTag("Converter"))
        {
            isCollectingC = false;
        }
    }

    IEnumerator ResourceEnum()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / collectRate);
            if (isCollectingG)
            {
                CollectFromGenerator();
            }
            if (isCollectingC)
            {
                CollectFromConverter();
            }
        }
    }

    void CollectFromGenerator()
    {
        if (backpack.Count < storageLimit && generator.resourceList.Count > 0)
        {
            GameObject temp = ObjectPooler.Instance.SpawnFromPool(generator.prefabName, spawnPoint.transform.position, spawnPoint.transform.rotation);
            //GameObject temp = Instantiate(prefab, spawnPoint);
            temp.transform.SetParent(spawnPoint);
            backpack.Add(temp);
            generator.RemoveLast();
            ReOrder();
        }
    }

    void CollectFromConverter()
    {
        if (backpack.Count < storageLimit && converter.endProductList.Count > 0)
        {
            GameObject temp = ObjectPooler.Instance.SpawnFromPool(converter.endProductName, spawnPoint.transform.position, spawnPoint.transform.rotation);
            //GameObject temp = Instantiate(prefab, spawnPoint);
            temp.transform.SetParent(spawnPoint);
            backpack.Add(temp);
            converter.RemoveLast();
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
        backpack[backpack.Count - 1].gameObject.SetActive(false);
        //Destroy(backpack[backpack.Count - 1]);
        backpack.RemoveAt(backpack.Count - 1);
        ReOrder();
    }
        
    public void RemoveOne(GameObject resource)
    {
        resource.SetActive(false);
        //Destroy(resource);
        backpack.Remove(resource);
        ReOrder();
    }
}
