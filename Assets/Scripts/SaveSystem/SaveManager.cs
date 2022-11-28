using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public BuyArea[] buyAreas;

    private void Awake()
    {
        LoadBuyAreas();
    }

    public void OnApplicationQuit()
    {
        Debug.Log("Game is closing.");
        SaveBuyAreas();
    }

    public void SaveBuyAreas()
    {
        SaveSystem.SaveBuyAreas(buyAreas);
        Debug.Log("BuyArea datas saved.");
    }

    public void LoadBuyAreas()
    {
        BuyAreaData[] dataArray = SaveSystem.LoadBuyAreas(buyAreas.Length);

        for (int i = 0; i < dataArray.Length; i++)
        {
            buyAreas[i].moneySpended = dataArray[i].moneySpended;
            buyAreas[i].gameObject.SetActive(dataArray[i].isActive);
        }

        Debug.Log("BuyArea datas load safely");
    }
}
