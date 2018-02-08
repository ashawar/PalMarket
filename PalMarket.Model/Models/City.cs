﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Model
{
    public class City
    {
        public int CityID { get; set; }
        public string Name { get; set; }
        public virtual List<Branch> Branches { get; set; }
    }
}
