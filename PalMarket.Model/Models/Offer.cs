using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Model
{
    public class Offer
    {
        public int OfferID { get; set; }
        public string Name { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public decimal? OldPrice { get; set; }
        public decimal? NewPrice { get; set; }
        public byte[] Image { get; set; }
        public string ImageFileName { get; set; }

        public int BranchID { get; set; }
        public Branch Branch { get; set; }

        public Offer()
        {
            DateCreated = DateTime.UtcNow;
        }
    }
}
