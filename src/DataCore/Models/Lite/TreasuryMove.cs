using System;

namespace Phony.Data.Models.Lite
{
    public class TreasuryMove : BaseModel
    {
        public virtual Treasury Treasury { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}