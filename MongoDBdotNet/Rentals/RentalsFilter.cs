using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDBdotNet.Rentals
{
    public class RentalsFilter
    {
        public decimal? PriceLimit { get; set; } //nullable to indicate optional
        public int? MinimumRooms { get; set; }
    }
}