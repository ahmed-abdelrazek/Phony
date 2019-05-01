using Phony.WPF.Data;

namespace Phony.WPF.Models
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
    }
}