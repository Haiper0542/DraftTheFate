using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public TextAsset jsonFile;
    public GameData gameData;

    public List<Card> cardPrefabs;
    public Dictionary<string, Card> cardPrefabData = new Dictionary<string, Card>();

    public static DataManager instance;

    private void Awake()
    {
        jsonFile = Resources.Load("GameData") as TextAsset;
        DontDestroyOnLoad(gameObject);
        instance = this;

        //SaveData(gameData);

        foreach (Card c in cardPrefabs)
            cardPrefabData.Add(c.cardData.wordIndex, c);

        LoadData();
    }

    public void PickupCard(string cardName)
    {
        gameData.cardDeselectedInventory.Remove(cardName);
        gameData.cardSelectedInventory.Add(cardName);
        gameData.myDeck.Add(cardName);
    }

    public void DropCard(string cardName)
    {
        gameData.myDeck.Remove(cardName);
        gameData.cardDeselectedInventory.Add(cardName);
        gameData.cardSelectedInventory.Remove(cardName);
    }

    public void SaveDeck()
    {
        SaveData(gameData);
    }

    public void SaveData(GameData data)
    {
        CreateJsonFile(JsonUtility.ToJson(data, prettyPrint: true));
    }

    public void LoadData()
    {
        byte[] data = jsonFile.bytes;
        string jsonData = Encoding.UTF8.GetString(data);
        gameData = JsonUtility.FromJson<GameData>(jsonData);
    }

    void CreateJsonFile(string jsonData)
    {
        string path = Application.dataPath + "/Resources/GameData.json";
        if (Application.platform == RuntimePlatform.Android)
            path = Application.persistentDataPath + "/Resources/GameData.json";
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            path = Application.dataPath + "/Resources/GameData.json";

        FileStream fileStream = new FileStream(path, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
}