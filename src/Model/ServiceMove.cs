namespace Phony.Model
{
    public class ServiceMove : BaseModel
    {
        public long ServiceId { get; set; }

        public virtual Service Service { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}