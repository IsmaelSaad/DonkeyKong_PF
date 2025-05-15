using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MongoDBManager : MonoBehaviour
{
    public static MongoDBManager Instance;

    private MongoClient client;
    private IMongoDatabase db;
    private IMongoCollection<BsonDocument> usersCollection;

    /* COSAS A TENER EN CUENTA:
        id de usuario
        - generacion de informe al acabar la partida con triggers
        */

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
    }

    async void Start()
    {
        string connectionString = "mongodb+srv://<a24ismsaajdi>:<JPuaFG1pAsNCdZEp>@cluster0.2wmy0yt.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

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

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(usersCollection.Find<>);
    }
}
