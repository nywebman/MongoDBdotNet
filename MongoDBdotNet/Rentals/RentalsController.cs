using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace MongoDBdotNet.Rentals
{
    public class RentalsController :Controller
    {
        public readonly RealEstateContext context = new RealEstateContext();

        public ActionResult Index()
        {
            var rentals = context.Rentals.FindAll();
            return View(rentals);
        }


        public ActionResult Post()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Post(PostRental postRental)
        {
            var rental = new Rental(postRental);
            context.Rentals.Insert(rental);
            return RedirectToAction("Index");

            //return View();
        }

        public ActionResult AdjustPrice(string id)
        {
            var rental = GetRental(id);
            return View(rental);
        }

        private Rental GetRental(string id)
        {
            var rental = context.Rentals.FindOneById(new ObjectId(id));
            return rental;
        }

        [HttpPost]
        public ActionResult AdjustPrice(string id, AdjustPrice adjustPrice)
        {
            var rental = GetRental(id);
            rental.AdjustPrice(adjustPrice);
            context.Rentals.Save(rental);
            return RedirectToAction("Index");
        }


        public ActionResult Delete(string id)
        {
            context.Rentals.Remove(Query.EQ("_id", new ObjectId(id)));
            return RedirectToAction("Index");
        }
    }
}