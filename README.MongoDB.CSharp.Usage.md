## 关于BsonDocument
- BsonDocument内部使用的是Dictionary<string,object>，与JSON类似，但又不完全一样
```c#
new BsonDocument
{
    {"name", "MongoDB"},
    {"type", "Database"},
    {"count", 1},
    {
        "info", new BsonDocument
        {
            {"x", 203},
            {"y", 102}
        }
    }
});
// Construct BsonDocument with BsonArray
BsonDocument sevenSamurai = new BsonDocument()
{
    {"name", "The Seven Samurai"},
    {"directorName", " Akira Kurosawa"},
    {
        "actors", new BsonArray
        {
            new BsonDocument("name", "Toshiro Mifune"),
            new BsonDocument("name", "Takashi Shimura")
        }
    },
    {"year", 1954}
};
```

## Mongo Shell
- Access a mongoDb server
```powershell
mongo --host <serverName> --port 27017 --u <userName> --p <password>
```
## Reference
- MongoDb C# and .NET Driver [>>](https://docs.mongodb.com/drivers/csharp/)