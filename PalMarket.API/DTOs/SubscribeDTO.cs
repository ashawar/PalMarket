using System;
using System.ComponentModel.DataAnnotations;

namespace PalMarket.API.DTOs
{
    public struct SubscribeDTO
    {
        [Required]
        public string DeviceCode { get; set; }
        [Required]
        public string StoreQR { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}