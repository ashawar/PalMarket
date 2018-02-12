using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.API.DTOs
{
    public class BranchDTO
    {
        public int BranchID { get; set; }
        public string Location { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string City { get; set; }
        public int? CityID { get; set; }
        public string Store { get; set; }
        public int StoreID { get; set; }
    }

    public class BranchSaveDTO
    {
        [Required]
        public int BranchID { get; set; }
        [Required]
        public string Location { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        [Required]
        public int CityID { get; set; }
        public int? StoreID { get; set; }
    }
}
