using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoad : MonoBehaviour
{

    public List<SavableObjects> savableObjects;
    MaxstSceneManager maxstSceneManager;

    void Start()
    {
        maxstSceneManager = GetComponent<MaxstSceneManager>();
    }

    public void Save()
    {
        // Getting the objects from the randomplacer
        savableObjects = maxstSceneManager.savableObjects;

        // Turns the data from the objects class into binary data
        FileStream fs = File.Create(Application.persistentDataPath + "/SavableObjects.dat");
        Debug.Log("!!" + Application.persistentDataPath);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, savableObjects);
        fs.Close(); // You MUST close the filestream otherwise it will cause errors!!
    }

    public bool Load()
    {
        string path = Application.persistentDataPath + "/SavableObjects.dat";
        // Checking if the file exists
        if (File.Exists(path))
        {
            FileStream fs = File.Open(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            // Making sure the file is not empty
            if (fs.Length > 0)
            {
                // Turns the data back from the binary into strings and floats
                savableObjects = (List<SavableObjects>)bf.Deserialize(fs);
                maxstSceneManager.savableObjects = savableObjects;
                maxstSceneManager.Reinstantiate();
                fs.Close(); // You MUST close the filestream otherwise it will cause errors!!
                return true;
            }
        }
        // This will happen if the file is non existant or empty
        return false;
    }

}