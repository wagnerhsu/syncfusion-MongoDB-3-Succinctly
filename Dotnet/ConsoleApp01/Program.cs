using MongoDB.Driver;
using System;

DatabaseHelper dbHelper = new DatabaseHelper("192.168.0.224", "mongodb://localhost:27017");
//dbHelper.ConnectWithoutAuthentication();
dbHelper.ConnectWithAuthentication();

public class DatabaseHelper
{
    private readonly string _host;
    private readonly string _connectionString;

    public DatabaseHelper(string host, string connectionString)
    {
        _host = host;
        _connectionString = connectionString;
    }
    public void ConnectWithoutAuthentication()
    {

        var client = new MongoClient(_connectionString);
        
        Console.WriteLine("Connected");
    }
    public void ConnectWithAuthentication()
    {
        string dbName = "local";
        string userName = "admin";
        string password = "sa1q2w3E*";

        var credentials = MongoCredential.CreateCredential(dbName, userName, password);

        MongoClientSettings clientSettings = new MongoClientSettings()
        {
            Credential =  credentials,
            Server = new MongoServerAddress(_host, 27017)
        };

        var client = new MongoClient(clientSettings);

        Console.WriteLine("Connected as {0}", userName);
    }

}