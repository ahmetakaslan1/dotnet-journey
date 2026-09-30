// Sektörde Kural 1: Soyutlama (Abstraction). 
// Kimlik kartı veya bir sözleşme gibi düşün. Ne yapılması gerektiğini söyler, NASIL yapılacağıyla ilgilenmez.
public interface IDatabase
{
    void SaveData(string data);
}
