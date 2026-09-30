using System;
using System.Collections.Generic;
using Npgsql;

namespace lesson4;

public class PostgresUserRepository : IUserRepository
{
    // Docker'daki Postgres'e bağlanmak için gereken bilgiler
    private readonly string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=mysecretpassword;Database=postgres";

    public PostgresUserRepository()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS Users (Id SERIAL PRIMARY KEY, Name TEXT NOT NULL)", connection);
        command.ExecuteNonQuery();
    }

    public void Add(User user)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new NpgsqlCommand("INSERT INTO Users (Name) VALUES (@name)", connection);
        command.Parameters.AddWithValue("name", user.Name);
        command.ExecuteNonQuery();
        Console.WriteLine($"[POSTGRESQL] {user.Name} PostgreSQL veritabanına eklendi!");
    }

    public List<User> GetAll()
    {
        var users = new List<User>();
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new NpgsqlCommand("SELECT Id, Name FROM Users", connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            users.Add(new User { Id = reader.GetInt32(0), Name = reader.GetString(1) });
        }
        return users;
    }
}
