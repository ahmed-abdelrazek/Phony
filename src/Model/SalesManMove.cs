using System;

namespace Phony.Model
{
    public class SalesManMove : BaseModel
    {
        public SalesManMove()
        {
            Date = DateTime.Now;
        }

        public int SalesManId { get; set; }

        public virtual SalesMan SalesMan { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}