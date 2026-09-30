using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace lesson4;

public class SqliteUserRepository : IUserRepository
{
    private readonly string _connectionString = "Data Source=app.db";

    public SqliteUserRepository()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL)";
        command.ExecuteNonQuery();
    }

    public void Add(User user)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Users (Name) VALUES ($name)";
        command.Parameters.AddWithValue("$name", user.Name);
        command.ExecuteNonQuery();
        Console.WriteLine($"[SQLite] {user.Name} SQLite veritabanına eklendi!");
    }

    public List<User> GetAll()
    {
        var users = new List<User>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name FROM Users";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            users.Add(new User { Id = reader.GetInt32(0), Name = reader.GetString(1) });
        }
        return users;
    }
}
