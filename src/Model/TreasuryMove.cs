namespace Phony.Model
{
    public class TreasuryMove : BaseModel
    {
        public long TreasuryId { get; set; }

        public virtual Treasury Treasury { get; set; }

        public decimal In { get; set; }

        public decimal Out { get; set; }
    }
}