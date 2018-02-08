using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Model
{
    public class DetailedStore
    {
        public int StoreID { get; set; }
        public string Name { get; set; }
        public string QRCode { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public byte[] Image { get; set; }
        public IEnumerable<Branch> Branches { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
