using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;


    public static DataPersistenceManager instance {get; private set;}

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    private void Awake() 
    {
        if (instance != null) {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        instance = this;
    }


    private void Start() 
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }


     public void NewGame() 
    {
        this.gameData = new GameData();
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
        // Pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects) 
        {
            dataPersistenceObject.SaveData(ref gameData);
        }
    }


    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
