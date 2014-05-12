using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDBdotNet.Rentals;
using NUnit.Framework;

namespace Tests.Rentals
{
    [TestClass]
    [TestFixture]
    public class RentalTest : AssertionHelper
    {
        [TestMethod]
        public void ToDocument_RentalWithPrice_PriceRepresepntedAsDouble()
        {
            var rental = new Rental();
            rental.Price = 1;

            var document = rental.ToBsonDocument();

            //Passes bcse of BsonRepresentation attrib on Price prop
            Expect(document["Price"].BsonType, Is.EqualTo(BsonType.Double)); 

        }

        [TestMethod]
        public void ToDocument_RentalWithAnId_IdIsRepresentedAsAnObject()
        {
            var rental = new Rental();
            rental.Id = ObjectId.GenerateNewId().ToString();

            var document = rental.ToBsonDocument();

            Expect(document["_id"].BsonType, Is.EqualTo(BsonType.ObjectId));
        }
    }
}
