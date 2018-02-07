using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Model
{
    public class StoreDevice
    {
        public int StoreDeviceID { get; set; }
        public int StoreID { get; set; }
        public int DeviceID { get; set; }
        public Store Store { get; set; }
        public Device Device { get; set; }
        public DateTime? DateCreated { get; set; }

        public StoreDevice()
        {
            DateCreated = DateTime.UtcNow;
        }
    }
}
