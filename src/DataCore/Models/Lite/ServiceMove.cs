using System;

namespace Phony.Data.Models.Lite
{
    public class ServiceMove : BaseModel
    {
        public virtual Service Service { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}