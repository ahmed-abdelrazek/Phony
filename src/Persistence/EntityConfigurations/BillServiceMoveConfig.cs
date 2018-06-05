using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class BillServiceMoveConfig : EntityTypeConfiguration<Model.BillServiceMove>
    {
        public BillServiceMoveConfig()
        {
            HasKey(b => b.Id);

            Property(b => b.Discount)
                .IsOptional();

            HasRequired(b => b.Bill)
                .WithMany(b => b.BillsServicesMoves)
                .HasForeignKey(b => b.BillId);
            
            HasRequired(b => b.Service)
                .WithMany(c => c.BillsServicesMoves)
                .HasForeignKey(b => b.ServiceId)
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