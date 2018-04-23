using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class SupplierMoveConfig : EntityTypeConfiguration<Model.SupplierMove>
    {
        public SupplierMoveConfig()
        {
            HasKey(c => c.Id);

            HasRequired(c => c.Supplier)
                .WithMany()
                .HasForeignKey(c => c.SupplierId)
                .WillCascadeOnDelete(false);

            HasRequired(c => c.Creator)
                .WithMany()
                .HasForeignKey(c => c.CreatedById)
                .WillCascadeOnDelete(false);

            Property(c => c.EditById)
                .IsOptional();

            HasOptional(c => c.Editor)
                .WithMany()
                .HasForeignKey(s => s.EditById)
                .WillCascadeOnDelete(false);
        }
    }
}