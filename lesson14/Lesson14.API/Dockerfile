# AŞAMA 1: DERLEME (BUILD)
# İçinde .NET SDK (Derleyici) olan ağır bir imaj kullanıyoruz.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Çözüm dosyasını ve tüm katmanların proje dosyalarını kopyalayıp bağımlılıkları yüklüyoruz.
COPY *.sln* ./
COPY Lesson14.API/*.csproj ./Lesson14.API/
COPY Lesson14.Application/*.csproj ./Lesson14.Application/
COPY Lesson14.Domain/*.csproj ./Lesson14.Domain/
COPY Lesson14.Infrastructure/*.csproj ./Lesson14.Infrastructure/
RUN dotnet restore

# Şimdi tüm kodları kopyalayıp projeyi derliyoruz.
COPY . ./
WORKDIR /app/Lesson14.API
RUN dotnet publish -c Release -o /app/publish

# AŞAMA 2: ÇALIŞTIRMA (RUN)
# İçinde SDK (Derleyici) OLMAYAN, sadece kodu çalıştıran çok hafif bir imaj kullanıyoruz.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Aşama 1'de derlenen hazır dosyaları bu hafif imajın içine kopyalıyoruz.
COPY --from=build /app/publish .

# Uygulamamızın 8080 portundan yayın yapmasını söylüyoruz.
EXPOSE 8080
ENTRYPOINT ["dotnet", "Lesson14.API.dll"]
