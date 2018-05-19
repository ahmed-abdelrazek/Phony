using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class BillMoveConfig : EntityTypeConfiguration<Model.BillMove>
    {
        public BillMoveConfig()
        {
            HasKey(b => b.Id);

            Property(b => b.Discount)
                .IsOptional();

            Property(b => b.ItemId)
                .IsOptional();

            Property(b => b.ServiceId)
                .IsOptional();

            Property(b => b.ServicePayment)
                .IsOptional();

            HasRequired(b => b.Bill)
                    .WithMany(b => b.BillsMoves)
                    .HasForeignKey(b => b.BillId);

            HasOptional(b => b.Item)
                    .WithMany(i => i.BillMoves)
                    .HasForeignKey(b => b.ItemId);

            HasOptional(b => b.Service)
                    .WithMany(c => c.BillMoves)
                    .HasForeignKey(b => b.ServiceId);

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