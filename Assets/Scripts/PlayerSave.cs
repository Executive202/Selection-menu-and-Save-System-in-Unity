using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerSave : MonoBehaviour
{
    public static PlayerSave playerSaveInstance; // instance of our class to allow it to be used outside of this class
    public int coins;
    public int[] objectCost;
    private string filepath; //our file path
    public TextMeshProUGUI coinText;
    public int ownedObject = 1;
    public GameObject[] objectsToSelect;
    void Awake()
    {
        //the awake is called before the start fuc
        //we check here if a saved game exits 
        if(playerSaveInstance == null)
        {
            playerSaveInstance = this;
        }else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        filepath = Application.persistentDataPath + "/save.dat";
        if(File.Exists(filepath))
        {
            Load();
        }
        UpdateCoin();
    }
    private void Start()
    {
        
    }
    public void UpdateCoin()
    {
        coinText.text = coins.ToString();
    }
    //hanldes the saving of data
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filepath);
        PlayerData data = new PlayerData();
        data.coins = coins;
        data.objectCost = new int[objectCost.Length];
        for(int i =0; i < objectCost.Length; i++)
        {
            data.objectCost[i] = objectCost[i];
        }
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("File Saved");
    }
    //resets or deletes all saved file
    public void ResetAllSaved()
    {
        filepath = Application.persistentDataPath + "/save.dat";
        DirectoryInfo dic = new DirectoryInfo(filepath);
        dic.Delete(true);
        Directory.CreateDirectory(filepath);
        coinText.text = coins.ToString();
    }
//handles the loading of already saved file
   public void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filepath,FileMode.Open);
        PlayerData data = (PlayerData)bf.Deserialize(file);
        file.Close();
        coins = data.coins;
        ownedObject = data.ownedObject;
        for (int i = 0; i < data.objectCost.Length; i++)
        {
            objectCost[i] = data.objectCost[i];
        }
        Debug.Log("File Loaded.....");
    }
}
//make sure the class is serializable so it can be accessed outside of this script
[Serializable] 
public class PlayerData
{
    public int coins;
    public int[] objectCost;
    public int ownedObject;
}
