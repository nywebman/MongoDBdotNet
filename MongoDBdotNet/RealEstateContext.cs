using MongoDB.Driver;
using MongoDBdotNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDBdotNet
{
    public class RealEstateContext
    {
        public MongoDatabase Database;
        public RealEstateContext()
        {
            var client = new MongoClient(Settings.Default.RealestatConnectionString);
            MongoServer server = client.GetServer();
            Database = server.GetDatabase(Settings.Default.RealEstateDatabaseName);
        }
    }
}