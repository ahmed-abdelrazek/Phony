using System;

namespace Phony.Model
{
    public class CompanyMove : BaseModel
    {
        public CompanyMove()
        {
            Date = DateTime.Now;
        }

        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}