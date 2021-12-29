﻿namespace Phony.Models
{
    public class Company : BaseModel
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public byte[] Image { get; set; }

        public string Site { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}