using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Model
{
    public class Device
    {
        public int DeviceID { get; set; }
        public string DeviceCode { get; set; }
        public string RegisterID { get; set; }
        public DateTime? DateCreated { get; set; }

        public virtual List<StoreDevice> StoreDevices { get; set; }

        public Device()
        {
            DateCreated = DateTime.Now;
        }
    }
}
