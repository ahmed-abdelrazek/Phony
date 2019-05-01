using System;

namespace Phony.WPF.Models.Lite
{
    public class CompanyMove : IBaseModel
    {
        public virtual Company Company { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

        public uint Id { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedOn { get; set; }

        public User Creator { get; set; }

        public DateTime? EditedOn { get; set; }

        public User Editor { get; set; }
    }
}