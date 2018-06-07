using System;

namespace Phony.Model
{
    public class SalesManMove : BaseModel
    {
        public long SalesManId { get; set; }

        public virtual SalesMan SalesMan { get; set; }

        public decimal Amount { get; set; }
    }
}