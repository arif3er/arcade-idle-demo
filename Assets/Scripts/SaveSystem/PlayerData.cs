
[System.Serializable]
public class PlayerData
{
    public int currentMoney;

    public float playerSpeed;
    public float collectRate;
    public float capacity;

    public float[] position;

    public PlayerData(Player player, PlayerController playerController, Collector collector) 
    {
        currentMoney = player.currenetMoney;
        playerSpeed = playerController.playerSpeed;
        collectRate = collector.collectRate;
        capacity = collector.capacity;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
