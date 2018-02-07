using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PalMarket.API.DTOs
{
    public struct OfferDTO
    {
        public int? OfferID { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        [Range(1, double.MaxValue)]
        public decimal? OldPrice { get; set; }
        [Range(1, double.MaxValue)]
        public decimal? NewPrice { get; set; }
        public string ImageUrl { get; set; }
    }
}