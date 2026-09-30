using System;

// Backend'in kalbi: Gevşek Bağlılık (Loose Coupling) ve Bağımlılık Enjeksiyonu'nun (Dependency Injection) temeli.
// Programımız HANGİ veritabanı olduğunu umursamıyor, sadece IDatabase kuralına uyan bir şey istiyor.

IDatabase db = new MongoDatabase(); 
// Yarın öbür gün MongoDb'ye geçmek istersek, sadece üst satırı "new MongoDatabase()" yaparız! 
// Kodun geri kalanı DEĞİŞMEZ. Backend'in en güçlü tarafı budur.

db.SaveData("Kullanıcı: Ahmet");
