namespace Phony.Models
{
    public class Store : BaseModel
    {
        public string Name { get; set; }

        public string Motto { get; set; }

        public byte[] Image { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Tel1 { get; set; }

        public string Tel2 { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Email1 { get; set; }

        public string Email2 { get; set; }

        public string Site { get; set; }
    }
}