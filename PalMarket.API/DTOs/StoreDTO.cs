using System;
using System.ComponentModel.DataAnnotations;

namespace PalMarket.API.DTOs
{
    public struct StoreDTO
    {
        public int? StoreID { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public string QRCode { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsSubscribed { get; set; }
    }
}