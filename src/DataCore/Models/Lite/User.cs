using DataCore.Data;
using System;

namespace Phony.WPF.Models.Lite
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Pass { get; set; }

        public UserGroup Group { get; set; }

        public string Phone { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? EditedOn { get; set; }
    }
}