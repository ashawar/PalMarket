using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PalMarket.API.DTOs
{
    public struct NotificationDTO
    {
        [Required, MaxLength(200)]
        public string Message { get; set; }
    }
}