using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveData
{
    public int[] lockPlane; // 10
    public int[] starLevel; // 60
    public int[] levelPlane; // 10
    public int[] levelScore; // 60
    public int rocket;
    public int shield;
    public int support;
    public int levelCompleted;
    public int highScore;
    public int gold;
    public int energy;
    public int crystal;
    public int levelMissile;
    public int levelSupport;
    public int levelShield;
    // Convert class instance to byte array
    public static byte[] ToBytes(SaveData data)
    {

        var formatter = new BinaryFormatter();

        using (var stream = new MemoryStream())
        {

            formatter.Serialize(stream, data);
            return stream.ToArray();
        }
    }

    // Convert byte array to class instance
    public static SaveData FromBytes(byte[] data)
    {

        using (var stream = new MemoryStream())
        {

            var formatter = new BinaryFormatter();
            stream.Write(data, 0, data.Length);
            stream.Seek(0, SeekOrigin.Begin);

            SaveData block = (SaveData)formatter.Deserialize(stream);
            return block;
        }
    }
}