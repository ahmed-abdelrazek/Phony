using System;

namespace Phony.Data.Models.Lite
{
    public class CompanyMove : BaseModel
    {
        public virtual Company Company { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}