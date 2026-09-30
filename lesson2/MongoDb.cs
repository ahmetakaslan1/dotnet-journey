class MongoDatabase : IDatabase
{
    public void SaveData(string data)
    {
        Console.WriteLine("MongoDb'ye kaydedildi.");
    }
}