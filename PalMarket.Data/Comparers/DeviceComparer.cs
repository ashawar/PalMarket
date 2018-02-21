using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Data.Comparers
{
    public class DeviceComparer : IEqualityComparer<Device>
    {
        public bool Equals(Device x, Device y)
        {
            // Check whether the compared object references the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            // Check whether the device IDs are equal.
            return x.DeviceID == y.DeviceID;
        }

        public int GetHashCode(Device obj)
        {
            return obj.DeviceID.GetHashCode();
        }
    }
}
