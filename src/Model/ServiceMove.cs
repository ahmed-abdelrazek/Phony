using System;

namespace Phony.Model
{
    public class ServiceMove : BaseModel
    {
        public ServiceMove()
        {
            Date = DateTime.Now;
        }

        public int ServiceId { get; set; }

        public virtual Service Service { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}