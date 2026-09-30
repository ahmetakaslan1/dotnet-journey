using System.Collections.Generic;
using Npgsql;
using Lesson8.Domain;
using Lesson8.Application;

namespace Lesson8.Infrastructure;

public class PostgresUserRepository : IUserRepository
{
    private readonly string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=mysecretpassword;Database=postgres";

    public PostgresUserRepository()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS Users (Id SERIAL PRIMARY KEY, Name TEXT NOT NULL, Email TEXT NOT NULL, PasswordHash TEXT NOT NULL)", connection);
        command.ExecuteNonQuery();
    }

    public void Add(User user)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new NpgsqlCommand("INSERT INTO Users (Name, Email, PasswordHash) VALUES (@name, @email, @hash)", connection);
        command.Parameters.AddWithValue("name", user.Name);
        command.Parameters.AddWithValue("email", user.Email);
        command.Parameters.AddWithValue("hash", user.PasswordHash);
        command.ExecuteNonQuery();
    }

    public List<User> GetAll()
    {
        var users = new List<User>();
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new NpgsqlCommand("SELECT Id, Name, Email, PasswordHash FROM Users", connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            users.Add(new User 
            { 
                Id = reader.GetInt32(0), 
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                PasswordHash = reader.GetString(3)
            });
        }
        return users;
    }
}
