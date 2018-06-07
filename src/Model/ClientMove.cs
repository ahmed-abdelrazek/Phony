using System;

namespace Phony.Model
{
    public class ClientMove : BaseModel
    {
        public long ClientId { get; set; }

        public virtual Client Client { get; set; }

        public decimal Amount { get; set; }
    }
}