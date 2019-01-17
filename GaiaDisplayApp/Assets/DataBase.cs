using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class DataBase : MonoBehaviour
{
    public struct Star
    {
        public string designation;
        public Vector3 position;
        public Color colour;
        public float brightness;
    }

    private string connectionString;

    // Use this for initialization
    void Start()
    {
        connectionString = "URI=file:" + Application.dataPath + "/GAIA.db";
    }

    public List<Star> GetStarList()
    {
        List<Star> stars = new List<Star>();

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string query = "SELECT * FROM stars";
                dbCmd.CommandText = query;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Star new_star = new Star();
                        new_star.designation = reader.GetString(0);
                        new_star.position = new Vector3((float)reader.GetDouble(1), (float)reader.GetDouble(2), (float)reader.GetDouble(3));
                        new_star.colour = IntToColor(reader.GetInt32(4));
                        new_star.brightness = reader.GetFloat(5);
                        stars.Add(new_star);
                    }

                    dbConnection.Close();
                    reader.Close();
                }
            }
        }

        return stars;
    }

    private Color IntToColor(int colour)
    {
        int r = (colour >> 16) & 255;
        int g = (colour >> 8) & 255;
        int b = (colour >> 0) & 255;
        return new Color(r, g, b);
    }
}
