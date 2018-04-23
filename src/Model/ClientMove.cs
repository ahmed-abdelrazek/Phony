using System;

namespace Phony.Model
{
    public class ClientMove : BaseModel
    {
        public ClientMove()
        {
            Date = DateTime.Now;
        }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}