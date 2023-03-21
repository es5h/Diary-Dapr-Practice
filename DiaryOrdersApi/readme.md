Docker mssql

```shell
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD={password}" -p 1455:1433 --name diaryordersql -d mcr.microsoft.com/mssql/server:2022-latest

```

EF

```
dotnet tool install --global dotnet-ef
dotnet ef migrations add init --output-dir Persistence/Migrations
dotnet ef database update
```

IList Json 으로 바인드
https://stackoverflow.com/questions/44829824/how-to-store-json-in-an-entity-field-with-ef-core

```shell
dapr run --app-id orderapi --app-port 5003 --dapr-http-port 50003 dotnet run --resources-path  "../components"
```

```shell
docker run -p 1080:1080 -p 1025:1025 maildev/maildev
```