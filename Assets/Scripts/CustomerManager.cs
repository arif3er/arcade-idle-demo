using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public List<Customer> customerList = new List<Customer>();

    private void Start()
    {
        foreach (var item in customerList)
            item.gameObject.SetActive(false);

        StartCoroutine(SpawnCustomer());
    }

    private IEnumerator SpawnCustomer()
    {
        for (int i = 0; i < customerList.Count; i++)
        {
            yield return new WaitForSeconds(Random.Range(1,3));
            customerList[i].gameObject.SetActive(true);
        }
        yield break;
    }
}
