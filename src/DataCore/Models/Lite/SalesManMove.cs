using System;

namespace Phony.Data.Models.Lite
{
    public class SalesManMove : BaseModel
    {
        public virtual SalesMan SalesMan { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}