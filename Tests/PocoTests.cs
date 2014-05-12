using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Attributes;
using NUnit.Framework;

namespace Tests
{
    [TestClass]
    public class PocoTests
    {
        public PocoTests()
        {
            JsonWriterSettings.Defaults.Indent = true;
        }

        public class Person 
        {
            public string FirstName { get; set; }
            public int Age { get; set; }

            public List<string> Address = new List<string>();
            public Contact Contact =new Contact();
            [BsonIgnore]
            public string IgnoreMe { get; set; }
            [BsonElement("New")] //rename
            public string Old { get; set; }
            [BsonElement]
            private string Encapsulated; //private so wont show in output, unless the attib is there
        }

        public class Contact
        {
            public string Email { get; set; }
            public string Phone { get; set; }
        }

        [TestMethod]
        [Test]
        public void Automatic()
        {
            var person = new Person() { Age = 54, FirstName = "bob" };

            person.Address.Add("101 some road");
            person.Address.Add("Unit 501");
            person.Contact.Email = "someone@somewhere.com";
            person.Contact.Phone = "123-456-7890";
            Console.WriteLine(person.ToJson()); //Was missing a using to get the .ToJson() method
        }

        [TestMethod]
        [Test]
        public void SerializationAttributes()
        {
            var person = new Person();
            Console.WriteLine(person.ToJson());         
        }
    }
}
