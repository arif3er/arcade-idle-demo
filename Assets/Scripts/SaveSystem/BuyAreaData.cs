
[System.Serializable]
public class BuyAreaData
{
    public int moneySpended;

    public bool isActive;
    
    public BuyAreaData(BuyArea buyArea)
    {
        moneySpended = buyArea.moneySpended;
        isActive = buyArea.gameObject.activeInHierarchy;
    }
}
