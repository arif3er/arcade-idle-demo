using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    private ShopManager shopManager;

    private void Start()
    {
        shopManager = GetComponentInParent<ShopManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Customer"))
        {
            shopManager.Sell();
        }
    }
}
