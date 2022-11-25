using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public List<GameObject> backpack = new List<GameObject>();

    [SerializeField] private Transform spawnPoint;

    public float collectRate;
    public float capacity;
    [Range(0f, 1f)] public float paddingY;

    private Generator generator;
    private Converter converter;

    private bool isCollectingG;
    private bool isCollectingC;

    public bool full;
    public bool halfFull;

    private void Start()
    {
        Collector[] otherCollectors = GameObject.FindObjectsOfType<Collector>();
        for (int i = 0; i < otherCollectors.Length; i++)
            Physics.IgnoreCollision(otherCollectors[i].GetComponent<Collider>(), GetComponent<Collider>());

        GameObject p = FindObjectOfType<Player>().gameObject;
        Physics.IgnoreCollision(p.GetComponent<CharacterController>(), GetComponent<Collider>());
        Physics.IgnoreCollision(p.GetComponentInChildren<Collider>(), GetComponent<Collider>());

        //Physics.IgnoreCollision(Player.Instance.GetComponent<CharacterController>(), GetComponent<Collider>());
    }

    private void OnEnable()
    {
        StartCoroutine(ResourceEnum());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Generator"))
        {
            generator = other.GetComponent<Generator>();
        }
        if (other.gameObject.CompareTag("Converter"))
        {
            converter = other.GetComponentInParent<Converter>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Generator"))
        {
            //if (!PlayerController.Instance.isMoving)    
                isCollectingG = true;
            //else
              //  isCollectingG = false;
        }
        if (other.gameObject.CompareTag("Converter"))
        {
            //if (!PlayerController.Instance.isMoving)
                isCollectingC = true;
            //else
              //  isCollectingC = false;
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
        if (backpack.Count < capacity && generator.resourceList.Count > 0)
        {
            Transform item = generator.resourceList[generator.resourceList.Count - 1].transform;
            var v3 = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + backpack.Count * paddingY, spawnPoint.transform.position.z);
            var tween = item.DOJump(v3, 1f,1,0.2f).SetEase(Ease.InOutSine);
            item.SetParent(spawnPoint);
            backpack.Add(item.gameObject);
            generator.resourceList.RemoveAt(generator.resourceList.Count - 1);
            tween.OnComplete(() => { ReOrder(); });
        }
    }

    void CollectFromConverter()
    {
        if (backpack.Count < capacity && converter.endProductList.Count > 0)
        {
            Transform item = converter.endProductList[converter.endProductList.Count - 1].transform;
            var v3 = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + backpack.Count * paddingY, spawnPoint.transform.position.z);
            var tween = item.DOJump(v3, 1f, 1, 0.2f).SetEase(Ease.InOutSine);
            item.SetParent(spawnPoint);
            backpack.Add(item.gameObject);
            converter.endProductList.RemoveAt(converter.endProductList.Count - 1);
            tween.OnComplete(() => { ReOrder(); });
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
        backpack.RemoveAt(backpack.Count - 1);
        ReOrder();
    }
        
    public void RemoveOne(GameObject resource)
    {
        resource.transform.SetParent(null);
        backpack.Remove(resource);
        resource.SetActive(false);
        ReOrder();
    }
}
