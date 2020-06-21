using System;

namespace Phony.Data.Models.Lite
{
    public class SalesMan : BaseModel
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public string Site { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}