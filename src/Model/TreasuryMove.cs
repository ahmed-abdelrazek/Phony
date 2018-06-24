namespace Phony.Model
{
    public class TreasuryMove : BaseModel
    {
        public long TreasuryId { get; set; }

        public virtual Treasury Treasury { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}