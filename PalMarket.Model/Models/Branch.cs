using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Model
{
    public class Branch
    {
        public int BranchID { get; set; }
        public string Location { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int CityID { get; set; }
        public int StoreID { get; set; }
        public byte[] Prices { get; set; }
        public string PricesFileName { get; set; }

        public virtual City City { get; set; }
        public virtual Store Store { get; set; }
        public virtual List<Offer> Offers { get; set; }
        public virtual List<User> Users { get; set; }
    }
}
