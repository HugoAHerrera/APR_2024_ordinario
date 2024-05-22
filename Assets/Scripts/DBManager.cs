using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;
using System.Data.Common;
using Mono.Data.Sqlite;
using System.Drawing;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    public static DBManager Instance { get; private set; }
    private string dbUri = "URI=file:mydb.sqlite";
    private string SQL_COUNT_ELEMNTS = "SELECT count(*) FROM Posiciones;";
    private string SQL_CREATE_POSICIONES = "CREATE TABLE IF NOT EXISTS Posiciones "
                                            + " (id INTEGER PRIMARY KEY AUTOINCREMENT," 
                                            + " position_x STRING NOT NULL," 
                                            + " position_y STRING NOT NULL,"
                                            + " position_z STRING NOT NULL);";

    private IDbConnection dbConnection;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        IDbConnection dbConnection = OpenDatabase();
        InitializeDB(dbConnection);
    }

    private IDbConnection OpenDatabase()
    {
        dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "PRAGMA foreign_keys = ON";
        dbCommand.ExecuteNonQuery();
        return dbConnection;
    }

    private void InitializeDB(IDbConnection dbConnection)
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = SQL_CREATE_POSICIONES;
        Debug.Log("Base de datos creada o iniciada");
        dbCmd.ExecuteReader();
    }

    public void SavePosition(CharacterPosition position)
    {
        float position_x = position.position.x;
        float position_y = position.position.y;
        float position_z = position.position.z;
        string position_x1 = position_x.ToString();
        string position_y1 = position_y.ToString();
        string position_z1 = position_z.ToString();
        Debug.Log(position);
        string command = $"INSERT INTO Posiciones (position_x, position_y, position_z) VALUES ('{position_x1}', '{position_y1}', '{position_z1})'";
        Debug.Log(command);
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = command;
        dbCommand.ExecuteNonQuery();
        Debug.Log("Posicion añadida");
    }

    private void OnDestroy()
    {
        dbConnection.Close();
    }
}
