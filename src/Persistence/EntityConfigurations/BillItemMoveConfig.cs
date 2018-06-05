using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class BillItemMoveConfig : EntityTypeConfiguration<Model.BillItemMove>
    {
        public BillItemMoveConfig()
        {
            HasKey(b => b.Id);

            Property(b => b.Discount)
                .IsOptional();

            HasRequired(b => b.Bill)
                .WithMany(b => b.BillsItemsMoves)
                .HasForeignKey(b => b.BillId);

            HasRequired(b => b.Item)
                .WithMany(i => i.BillsItemsMoves)
                .HasForeignKey(b => b.ItemId)
                .WillCascadeOnDelete(false);

            HasRequired(b => b.Creator)
                .WithMany()
                .HasForeignKey(s => s.CreatedById)
                .WillCascadeOnDelete(false);

            Property(b => b.EditById)
                .IsOptional();

            HasOptional(b => b.Editor)
                .WithMany()
                .HasForeignKey(b => b.EditById)
                .WillCascadeOnDelete(false);
        }
    }
}