using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class BillMoveConfig : EntityTypeConfiguration<Model.BillMove>
    {
        public BillMoveConfig()
        {
            HasKey(b => b.Id);

            HasOptional(b => b.Bill)
                    .WithMany(b => b.BillMoves)
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
