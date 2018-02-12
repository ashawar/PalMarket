using System;
using System.ComponentModel.DataAnnotations;

namespace PalMarket.API.DTOs
{
    public struct DeviceDTO
    {

        public int? DeviceID { get; set; }
        [MaxLength(100)]
        public string DeviceCode { get; set; }
        [MaxLength(100)]
        public string RegisterID { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}