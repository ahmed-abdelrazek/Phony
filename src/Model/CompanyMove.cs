using System;

namespace Phony.Model
{
    public class CompanyMove : BaseModel
    {
        public long CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public decimal Amount { get; set; }
    }
}