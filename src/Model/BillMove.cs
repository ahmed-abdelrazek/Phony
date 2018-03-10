namespace Phony.Model
{
    public class BillMove : BaseModel
    {
        public int? BillId { get; set; }

        public virtual Bill Bill { get; set; }

        public int? ItemId { get; set; }

        public virtual Item Item { get; set; }

        public decimal? QTY { get; set; }

        public int? ServiceId { get; set; }

        public virtual Service Service { get; set; }

        public decimal? Discount { get; set; }
    }
}