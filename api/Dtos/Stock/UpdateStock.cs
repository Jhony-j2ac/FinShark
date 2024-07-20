﻿using api.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Dtos.Stock
{
    public class UpdateStock
    {
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal Purchase { get; set; }
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }

    }
}
