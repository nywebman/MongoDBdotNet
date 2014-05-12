using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NUnit.Framework;

namespace Tests
{
    [TestClass]
    public class BsonDocumentTests
    {
        public BsonDocumentTests()
        {
            //indents in the output of the other test methods
            JsonWriterSettings.Defaults.Indent = true;
        }

        [Test]
        [TestMethod]
        public void EmptyDocument()
        {
            var doc = new BsonDocument();
            Console.WriteLine(doc);
        }

        [Test]
        [TestMethod]
        public void AddElements()
        {
            var person = new BsonDocument()
                {
                    {"age",new BsonInt32(54)},
                    {"IsAlive",true}
                };
            person.Add("firstname", new BsonString("bob"));

            Console.WriteLine(person);
        }

        [Test]
        [TestMethod]
        public void AddingArrays()
        {
            var person = new BsonDocument();
            person.Add("address", new BsonArray(new[]{"101 Some road", "unit 501"}));
            Console.WriteLine(person);
        }

        [Test]
        [TestMethod]
        public void EmbeddedDocument()
        {
            var person = new BsonDocument()
                {
                    {
                        "contact",new BsonDocument
                        {
                            {"phone","123-456-7890"},
                            {"email","whatever@email.com"}
                        }
                    }
                }    ;
            Console.WriteLine(person);
        }

        [Test]
        [TestMethod]
        public void BsconValueConversions()
        {
            var person = new BsonDocument
            {
                {"age",54}
            };
            Console.WriteLine(person["age"].AsInt32+10); //performing case behind the scenes

            Console.WriteLine(person["age"].ToDouble() + 10);
            Console.WriteLine(person["age"].IsString);
            Console.WriteLine(person["age"].IsInt32); 
        }

        [Test]
        [TestMethod]
        public void ToBson()
        {
            var person = new BsonDocument
            {
                {"firstname","bob"}
            };

            var bson = person.ToBson();//bson is binary

            Console.WriteLine(BitConverter.ToString(bson)); 

            //desrialize
            var desir = BsonSerializer.Deserialize<BsonDocument>(bson);
            Console.WriteLine(desir); 
        }
    }
}
