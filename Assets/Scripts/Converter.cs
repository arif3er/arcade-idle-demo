using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> gameObjectList = new List<GameObject>();

    public Transform spawnPoint;
    public GameObject prefab;

    private ShopManager shopManager;

    public float rate;
    public int coinLimit;
    public int stackLimit;

    [SerializeField][Range(0f, 1f)] float paddingY;
    [SerializeField][Range(0f, 5f)] float paddingX;
    [SerializeField][Range(0f, 5f)] float paddingZ;

    private void Start()
    {
        shopManager = GetComponent<ShopManager>();
        StartCoroutine(Convert());
    }

    IEnumerator Convert()
    {
        while (true)
        {
            if (shopManager.currentApple > 0 && gameObjectList.Count < coinLimit)
            {
                shopManager.currentApple--;
                GameObject temp = Instantiate(prefab, spawnPoint);
                temp.transform.position = new Vector3(spawnPoint.transform.position.x + (paddingX * gameObjectList.Count),
                                                     (spawnPoint.transform.position.y) + paddingY,
                                                     spawnPoint.transform.position.z);
                gameObjectList.Add(temp);
                Debug.Log("Coin ");
            }   
            yield return new WaitForSeconds(1 / rate);
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
