using System;

namespace Phony.WPF.Models.Lite
{
    public class Treasury : IBaseModel
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public virtual Store Store { get; set; }

        public uint Id { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedOn { get; set; }

        public User Creator { get; set; }

        public DateTime? EditedOn { get; set; }

        public User Editor { get; set; }
    }
}