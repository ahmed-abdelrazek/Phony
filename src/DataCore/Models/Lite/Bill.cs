using System;

namespace Phony.Data.Models.Lite
{
    public class Bill : BaseModel
    {
        public virtual Client Client { get; set; }

        public virtual Store Store { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAfterDiscounts { get; set; }

        public decimal TotalPayed { get; set; }

        public bool IsReturned { get; set; }
    }
}