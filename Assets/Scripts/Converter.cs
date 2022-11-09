using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> moneyList = new List<GameObject>();

    public Transform spawnPoint;
    public GameObject prefab;

    [SerializeField] private ShopManager shopManager;

    public float rate;
    public int moneyLimit;
    public int stackLimit;

    [SerializeField][Range(0f, 1f)] private float paddingY;
    [SerializeField][Range(0f, 5f)] private float paddingX;
    [SerializeField][Range(0f, 5f)] private float paddingZ;

    private void Start()
    {
        StartCoroutine(Convert());
    }

    IEnumerator Convert()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / rate);
            if (shopManager.currentFruit > 0 && moneyList.Count < moneyLimit)
            {
                shopManager.currentFruit--;
                int rowCount = moneyList.Count / stackLimit;
                GameObject temp = Instantiate(prefab, spawnPoint);
                temp.transform.position = new Vector3(spawnPoint.transform.position.x + (float)(rowCount * paddingX),
                                                     (moneyList.Count % stackLimit) * paddingY,
                                                     spawnPoint.transform.position.z + (float)(rowCount * paddingZ));
                moneyList.Add(temp);
            }   
        }
    }
   
    public void RemoveLast()
    {
        if (moneyList.Count > 0)
        {
            Destroy(moneyList[moneyList.Count - 1]);
            moneyList.RemoveAt(moneyList.Count - 1);
        }
    }
}
