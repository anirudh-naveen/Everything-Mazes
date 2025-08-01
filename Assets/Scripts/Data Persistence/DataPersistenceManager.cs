using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    public static DataPersistenceManager instance { get; private set; }

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public TMP_Dropdown ddSave;
    private string fullName;

    private void Awake() 
    {
        if (instance != null) {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        instance = this;
    }


    private void Start() 
    {
        fullName = fileName + ddSave.value;
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        NewGame();
    }


     public void NewGame() 
    {
        gameData = new GameData();
    }



    public void onSaveFileDropdownChanged() {
        fullName = fileName + ddSave.value;
        dataHandler = new FileDataHandler(Application.persistentDataPath, fullName, useEncryption);
    }


    // Loads the maze from a chosen JSON save file
    public void LoadGame() 
    {
        // Load any saved data from a file using the data handler
        this.gameData = dataHandler.Load();

        // If there is no data to load, initialize a new game
        if (this.gameData == null) {
            Debug.Log("No save data was found. initializing to defaults.");
            NewGame();
        }

        // Push the loaded data into all other scripts that use it
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects) 
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }


    // Saves the maze to a JSON save file
    public void SaveGame() 
    {
        // Initialize gameData if it's null
        if (this.gameData == null) 
        {
            Debug.Log("GameData was null. Initializing new GameData for save.");
            this.gameData = new GameData();
        }

        // Pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects) 
        {
            dataPersistenceObject.SaveData(ref gameData);
        }
        
        dataHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
