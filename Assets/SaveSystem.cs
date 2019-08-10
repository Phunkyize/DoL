using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
   
    public static void Save(string scene, Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.sfp";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(scene, player);
        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static SaveData Load()
    {
        string path = Application.persistentDataPath + "/save.sfp";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;

        }
        else
        {
            Debug.LogError("save file not found");
            return null; 
        }
    }
}
[System.Serializable]
public class SaveData
{
    public string scene;
    public Player.PlayerStats stats;
    public float[] position;

    public SaveData(string scene, Player player)
    {
        this.scene = scene;
        stats = player.playerStats;
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }

}


