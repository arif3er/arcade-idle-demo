using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(Player player, PlayerController playerController, Collector collector)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.ldp";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player, playerController, collector);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.ldp";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveBuyAreas(BuyArea[] buyAreas)
    {

        for (int i = 0; i < buyAreas.Length; i++)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/buyArea" + i + ".ldp";
            FileStream stream = new FileStream(path, FileMode.Create);

            BuyAreaData data = new BuyAreaData(buyAreas[i]);
            formatter.Serialize(stream, data);
            stream.Close();
        }
    }

    public static BuyAreaData[] LoadBuyAreas(int totalBuyAreaAmount)
    {
        BuyAreaData[] buyAreas = new BuyAreaData[totalBuyAreaAmount];

        for (int i = 0; i < totalBuyAreaAmount; i++)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/buyArea" + i + ".ldp";

            if (File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                BuyAreaData data = formatter.Deserialize(stream) as BuyAreaData;

                buyAreas[i] = data;
                Debug.Log(data + " added to array");
                stream.Close();
            }
            else
            {
                Debug.LogError("Save file not found in " + path);
                return null;
            }
        }

        return buyAreas;
    }
}
