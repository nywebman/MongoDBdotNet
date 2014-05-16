using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;

namespace MongoDBdotNet.Rentals
{
    public class RentalsController :Controller
    {
        public readonly RealEstateContext context = new RealEstateContext();

        public ActionResult Index(RentalsFilter filters)
        {
            var rentals = FilterRentals(filters);
              //  .SetSortOrder(SortBy<Rental>.Ascending(r=>r.Price));
            var model = new RentalsList
            {
                Rentals = rentals,
                Filters = filters
            };
            return View(model);
        }

        private IEnumerable<Rental> FilterRentals(RentalsFilter filters)
        {
            IQueryable<Rental> rentals = context.Rentals.AsQueryable()
                .OrderBy(r => r.Price);
            if (filters.MinimumRooms.HasValue)
            {
                rentals = rentals
                    .Where(r => r.NumberOfRooms >= filters.MinimumRooms);
            }

            if (filters.PriceLimit.HasValue)
            {
                var query = Query<Rental>.LTE(r=>r.Price,filters.PriceLimit);
                rentals = rentals
                    .Where(r => query.Inject());
            }

            
            return rentals;

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

        public string PriceDistribution()
        {
            return new QueryPriceDistribution()
            .Run(context.Rentals)
            .ToJson();
        }

        public ActionResult AttachImage(string id)
        {
            var rental = GetRental(id);
            return View(rental);
        }
        [HttpPost]
        public ActionResult AttachImage(string id, HttpPostedFileBase file)
        {
            var rental = GetRental(id);
            if (rental.HasImage())
            { 
                DeleteImage(rental);
            }
            StoreImage(file, rental);
            return RedirectToAction("Index");
        }

        private void DeleteImage(Rental rental)
        {
            context.Database.GridFS.DeleteById(new ObjectId(rental.ImageId));
            //rental.ImageId = null;
            //context.Rentals.Save(rental);
            SetRentalImageId(rental.Id, null);
        }

        private void StoreImage(HttpPostedFileBase file, Rental rental)
        {
            //Save rental first so that dont have orphaned image if something goes wrong
            //but in doing so, need an image id first
            var ImageId = ObjectId.GenerateNewId();
            rental.ImageId = ImageId.ToString();
            context.Rentals.Save(rental);
            var options = new MongoGridFSCreateOptions { ContentType = file.ContentType };
            //var fileInfo = 
            context.Database.GridFS.Upload(file.InputStream, file.FileName, options);
        }

        public ActionResult GetImage(string id)
        {
            var image = context.Database.GridFS
                .FindOneById(new ObjectId(id));
            if (image == null)
            { return HttpNotFound(); }
            return File(image.OpenRead(),image.ContentType);
        }

        private void SetRentalImageId(string rentalId, string imageId)
        {
            var rentalbyid = Query<Rental>.Where(r => r.Id == rentalId);
            var setRentalImageId = Update<Rental>.Set(r => r.ImageId, imageId);
            context.Rentals.Update(rentalbyid, setRentalImageId);

        }
    }
}