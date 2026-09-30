using System;

// SqlDatabase, IDatabase'in kurallarına uymak ZORUNDA.
// "Eğer IDatabase sözleşmesini imzaladıysan, SaveData metodunu yazacaksın!" der C#.
public class SqlDatabase : IDatabase
{


    public void SaveData(string data)
    {

    
        Console.WriteLine($"Veri SQL veritabanına kaydedildi: {data}");
    }
}
