using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Model
{
    public class Store
    {
        public int StoreID { get; set; }
        public string Name { get; set; }
        public string QRCode { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string ImageFileName { get; set; }
        public int ImageFileSize { get; set; }

        public virtual List<Branch> Branches { get; set; }
        public virtual List<StoreDevice> StoreDevices { get; set; }

        public Store()
        {
            DateCreated = DateTime.UtcNow;
        }
    }
}
