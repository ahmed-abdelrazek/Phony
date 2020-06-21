using System;

namespace Phony.Data.Models.Lite
{
    public class ClientMove : BaseModel
    {
        public virtual Client Client { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

    }
}