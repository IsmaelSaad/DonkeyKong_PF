using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using UnityEngine;

// Gestor de la connexió amb la base de dades MongoDB
public class MongoDBManager : MonoBehaviour
{
    public static MongoDBManager Instance;  // Instància singleton

    // Dades del jugador
    public DatabaseUser playerData = new DatabaseUser();
    public DatabaseConfig playerConfig = new DatabaseConfig();
    public DatabaseHighscore playerHighscore = new DatabaseHighscore();

    // Variables de connexió a MongoDB
    private MongoClient client;
    private IMongoDatabase db;
    private IMongoCollection<BsonDocument> usersCollection;
    private IMongoCollection<BsonDocument> highscoreCollection;
    private IMongoCollection<BsonDocument> configCollection;

    private void Awake()
    {
        string connectionString = "mongodb+srv://a24ismsaajdi:JPuaFG1pAsNCdZEp@cluster0.2wmy0yt.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

        try
        {
            // Intentem connectar amb la base de dades
            client = new MongoClient(connectionString);
            db = client.GetDatabase("DONKEY_KONG_PROJECT");
            usersCollection = db.GetCollection<BsonDocument>("Users");
            highscoreCollection = db.GetCollection<BsonDocument>("HighScores");
            configCollection = db.GetCollection<BsonDocument>("Configuration");
            Debug.Log("Connexió establerta amb èxit");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al connectar amb la base de dades: " + e.Message);
        }

        // Configuració del patró singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persistim entre escenes
        }
        else
        {
            Destroy(gameObject);  // Evitem duplicats
        }

        playerData.playTime = (double)System.DateTime.Now.Second;  // Iniciem el temps de joc
    }

    // Mètode per guardar les dades de l'usuari a la base de dades
    public async void SaveUserData(DatabaseUser data)
    {
        var document = new BsonDocument
        {
            { "player", data.name },
            { "score", data.score },
            { "barrelsJumped", data.barrelsJumped },
            { "playTime", data.playTime },
            { "objectsPickedUp", data.objectsPickedUp },
            { "timestamp", BsonDateTime.Create(System.DateTime.UtcNow) }  // Marca de temps
        };

        await usersCollection.InsertOneAsync(document);  // Inserció asíncrona
    }

    // Mètode per guardar les puntuacions altes a la base de dades
    public async void SaveHighScoreData(DatabaseHighscore data)
    {
        var document = new BsonDocument
    {
        { "player1", data.name[0] },  // Nom del 1r jugador
        { "score1", data.score[0] },  // Puntuació del 1r
        { "player2", data.name[1] },  // Nom del 2n jugador
        { "score2", data.score[1] },  // Puntuació del 2n
        { "player3", data.name[2] },  // Nom del 3r jugador (corregit índex)
        { "score3", data.score[2] },  // Puntuació del 3r (corregit índex)
        { "timestamp", BsonDateTime.Create(DateTime.UtcNow) }  // Data i hora
    };

        await highscoreCollection.InsertOneAsync(document);  // Inserim asíncronament
    }

    // Mètode per guardar la configuració de l'usuari
    public async void SaveConfigData(DatabaseConfig data)
    {
        var document = new BsonDocument
    {
        { "player", data.name },          // Nom del jugador
        { "volumeUser", data.volumeUser }, // Volum preferit
        { "timestamp", BsonDateTime.Create(DateTime.UtcNow) }  // Data i hora
    };

        await configCollection.InsertOneAsync(document);  // Inserim asíncronament
    }

    // Exporta totes les dades a fitxers JSON
    public void ExportCollectionsToJson()
    {
        ExportDataObjectToJson(playerData, "User.json");        // Dades d'usuari
        ExportHighScoreObjectToJson(playerHighscore, "HighScore.json"); // Rècords
        ExportConfigObjectToJson(playerConfig, "Configuration.json"); // Configuració
    }

    // Exporta les dades d'usuari a JSON
    private void ExportDataObjectToJson(DatabaseUser data, string fileName)
    {
        var export = new DatabaseUser
        {
            name = data.name,
            score = data.score,
            barrelsJumped = data.barrelsJumped,
            playTime = data.playTime,
            objectsPickedUp = data.objectsPickedUp
        };

        var json = JsonUtility.ToJson(export, true);  // Convertim a JSON amb format
        WriteJsonFile(fileName, json);  // Escrivim al fitxer
    }

    // Exporta els rècords a JSON
    private void ExportHighScoreObjectToJson(DatabaseHighscore data, string fileName)
    {
        var export = new DatabaseHighscore
        {
            name = data.name,
            score = data.score
        };

        var json = JsonUtility.ToJson(export, true);  // Convertim a JSON amb format
        WriteJsonFile(fileName, json);  // Escrivim al fitxer
    }

    // Exporta la configuració a JSON
    private void ExportConfigObjectToJson(DatabaseConfig data, string fileName)
    {
        var export = new DatabaseConfig
        {
            name = data.name,
            volumeUser = data.volumeUser
        };

        var json = JsonUtility.ToJson(export, true);  // Convertim a JSON amb format
        WriteJsonFile(fileName, json);  // Escrivim al fitxer
    }

    // Escriu un fitxer JSON al directori de dades de l'aplicació
    private void WriteJsonFile(string fileName, string json)
    {
        // Creem la ruta a la carpeta DonkeyKong dins de AppData
        string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DonkeyKong");
        Directory.CreateDirectory(folderPath);  // Creem la carpeta si no existeix

        // Combinem la ruta de la carpeta amb el nom del fitxer
        string filePath = Path.Combine(folderPath, fileName);
        File.WriteAllText(filePath, json);  // Escrivim el contingut JSON
    }

    // Mètode que s'executa en sortir del joc
    private void OnApplicationQuit()
    {
        ExportCollectionsToJson();  // Exportem les dades a JSON
    }

    // Mètode per obtenir les puntuacions més altes
    public async void GetHighestScore()
    {
        try
        {
            int i = 0;

            // Consulta ordenada per puntuació descendent (top 3)
            await usersCollection
                .Find(new BsonDocument())
                .Sort(Builders<BsonDocument>.Sort.Descending("score"))
                .Limit(3)
                .ForEachAsync(doc =>
                {
                    if (i >= 3) return; // Seguretat per evitar sobrepassar l'array

                    // Obtenim dades del document
                    string player = doc.Contains("player") ? doc["player"].AsString : "Unknown";
                    int score = doc.Contains("score") ? doc["score"].AsInt32 : 0;

                    // Guardem als arrays estàtics
                    DatabaseUser.highScoreName[i] = player;
                    DatabaseUser.highScorePoints[i] = score;

                    i++;
                });
        }
        catch (System.Exception e)
        {
            string errorMessage = "Error al obtenir les puntuacions: " + e.Message;
            Debug.LogError(errorMessage);
        }
    }

}
