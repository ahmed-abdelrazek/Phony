using System;

namespace Phony.Data.Models.Lite
{
    public class Treasury : BaseModel
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public virtual Store Store { get; set; }
    }
}