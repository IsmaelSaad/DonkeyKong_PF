using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

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
        string connectionString = "mongodb+srv://a24ismsaajdi:JPuaFG1pAsNCdZEp@cluster0.2wmy0yt.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

        try
        {
            client = new MongoClient(connectionString);
            db = client.GetDatabase("DONKEY_KONG_PROJECT");
            usersCollection = db.GetCollection<BsonDocument>("Users");
            Debug.Log("Connection");
        }
        catch (System.Exception e)
        {
            Debug.LogError("DONKEY_KONG_PROJECT database couldn't connect: " + e.Message);
        }

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

    }

    public async void GetHighestScore()
    {
        try
        {
            int i = 0;

            await usersCollection
                .Find(new BsonDocument())
                .Sort(Builders<BsonDocument>.Sort.Descending("score"))
                .Limit(3)
                .ForEachAsync(doc =>
                {
                    if (i >= 3) return; // seguridad extra

                    string player = doc.Contains("player") ? doc["player"].AsString : "Unknown";
                    int score = doc.Contains("score") ? doc["score"].AsInt32 : 0;

                    Database.highScoreName[i] = player;
                    Database.highScorePoints[i] = score;

                    i++;
                });
        }
        catch (System.Exception e)
        {
            string errorMessage = "DONKEY_KONG_PROJECT database couldn't connect: " + e.Message;
            Debug.LogError(errorMessage);
            LogErrorToFile(errorMessage);
        }

    }

    private void LogErrorToFile(string message)
    {
        string path = Application.persistentDataPath + "/mongo_error_log.txt";
        try
        {
            System.IO.File.AppendAllText(path, DateTime.Now + " - " + message + Environment.NewLine);
        }
        catch (Exception fileEx)
        {
            Debug.LogError("Failed to write error log: " + fileEx.Message);
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
