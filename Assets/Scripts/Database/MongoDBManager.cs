using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using UnityEngine;

// Gestor de la connexi� amb la base de dades MongoDB
public class MongoDBManager : MonoBehaviour
{
    public static MongoDBManager Instance;  // Inst�ncia singleton

    // Dades del jugador
    public DatabaseUser playerData = new DatabaseUser();
    public DatabaseConfig playerConfig = new DatabaseConfig();
    public DatabaseHighscore playerHighscore = new DatabaseHighscore();

    // Variables de connexi� a MongoDB
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
            Debug.Log("Connexi� establerta amb �xit");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al connectar amb la base de dades: " + e.Message);
        }

        // Configuraci� del patr� singleton
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

    // M�tode per guardar les dades de l'usuari a la base de dades
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

        await usersCollection.InsertOneAsync(document);  // Inserci� as�ncrona
    }

    // M�tode per guardar les puntuacions altes a la base de dades
    public async void SaveHighScoreData(DatabaseHighscore data)
    {
        var document = new BsonDocument
    {
        { "player1", data.name[0] },  // Nom del 1r jugador
        { "score1", data.score[0] },  // Puntuaci� del 1r
        { "player2", data.name[1] },  // Nom del 2n jugador
        { "score2", data.score[1] },  // Puntuaci� del 2n
        { "player3", data.name[2] },  // Nom del 3r jugador (corregit �ndex)
        { "score3", data.score[2] },  // Puntuaci� del 3r (corregit �ndex)
        { "timestamp", BsonDateTime.Create(DateTime.UtcNow) }  // Data i hora
    };

        await highscoreCollection.InsertOneAsync(document);  // Inserim as�ncronament
    }

    // M�tode per guardar la configuraci� de l'usuari
    public async void SaveConfigData(DatabaseConfig data)
    {
        var document = new BsonDocument
    {
        { "player", data.name },          // Nom del jugador
        { "volumeUser", data.volumeUser }, // Volum preferit
        { "timestamp", BsonDateTime.Create(DateTime.UtcNow) }  // Data i hora
    };

        await configCollection.InsertOneAsync(document);  // Inserim as�ncronament
    }

    // Exporta totes les dades a fitxers JSON
    public void ExportCollectionsToJson()
    {
        ExportDataObjectToJson(playerData, "User.json");        // Dades d'usuari
        ExportHighScoreObjectToJson(playerHighscore, "HighScore.json"); // R�cords
        ExportConfigObjectToJson(playerConfig, "Configuration.json"); // Configuraci�
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

    // Exporta els r�cords a JSON
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

    // Exporta la configuraci� a JSON
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

    // Escriu un fitxer JSON al directori de dades de l'aplicaci�
    private void WriteJsonFile(string fileName, string json)
    {
        // Creem la ruta a la carpeta DonkeyKong dins de AppData
        string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DonkeyKong");
        Directory.CreateDirectory(folderPath);  // Creem la carpeta si no existeix

        // Combinem la ruta de la carpeta amb el nom del fitxer
        string filePath = Path.Combine(folderPath, fileName);
        File.WriteAllText(filePath, json);  // Escrivim el contingut JSON
    }

    // M�tode que s'executa en sortir del joc
    private void OnApplicationQuit()
    {
        ExportCollectionsToJson();  // Exportem les dades a JSON
    }

    // M�tode per obtenir les puntuacions m�s altes
    public async void GetHighestScore()
    {
        try
        {
            int i = 0;

            // Consulta ordenada per puntuaci� descendent (top 3)
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

                    // Guardem als arrays est�tics
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
