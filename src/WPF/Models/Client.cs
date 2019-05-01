namespace Phony.WPF.Models
{
    public class Client : BaseModel
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public string Site { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}