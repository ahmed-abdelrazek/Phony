namespace Phony.Model
{
    public class ClientMove : BaseModel
    {
        public long ClientId { get; set; }

        public virtual Client Client { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}