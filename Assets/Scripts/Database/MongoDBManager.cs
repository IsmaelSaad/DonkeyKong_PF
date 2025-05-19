using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;

public class MongoDBManager : MonoBehaviour
{
    public static MongoDBManager Instance;

    public Database playerData = new Database();

    public static MongoDBManager instance
    {
        get
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("MongoDBManager");
                DontDestroyOnLoad(go);
                Instance = go.AddComponent<MongoDBManager>();
            }
            return instance;
        }
    }

    private MongoClient client;
    private IMongoDatabase db;
    private IMongoCollection<BsonDocument> usersCollection;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        playerData.playTime = (double) System.DateTime.Now.Second;
    }

    async void Start()
    {
        string connectionString = "mongodb+srv://a24ismsaajdi:JPuaFG1pAsNCdZEp@cluster0.2wmy0yt.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

        try
        {
            client = new MongoClient(connectionString);
            db = client.GetDatabase("DONKEY_KONG_PROJECT");
            usersCollection = db.GetCollection<BsonDocument>("Users");
            Debug.Log("Connection");
        } 
        catch (System.Exception e) {
            Debug.LogError("DONKEY_KONG_PROJECT database couldn't connect: " + e.Message);
        }
    }

    public async void SaveUserData(Database data)
    {
        var document = new BsonDocument
    {
        { "player", data.name },
        { "score", data.score },
        { "barrelsJumped", data.barrelsJumped },
        { "playTime", data.playTime },
        { "objectsPickedUp", data.objectsPickedUp },
        { "timestamp", BsonDateTime.Create(System.DateTime.UtcNow) }
    };

        await usersCollection.InsertOneAsync(document);
    }

    private void OnEnable()
    {
    }


    // Update is called once per frame
    void Update()
    {
    }

}
