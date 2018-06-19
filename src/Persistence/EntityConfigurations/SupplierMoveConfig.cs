using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class SupplierMoveConfig : EntityTypeConfiguration<Model.SupplierMove>
    {
        public SupplierMoveConfig()
        {
            HasKey(s => s.Id);

            HasRequired(s => s.Supplier)
                .WithMany(s => s.SuppliersMoves)
                .HasForeignKey(s => s.SupplierId)
                .WillCascadeOnDelete(true);

            HasRequired(s => s.Creator)
                .WithMany()
                .HasForeignKey(s => s.CreatedById)
                .WillCascadeOnDelete(false);

            Property(s => s.EditById)
                .IsOptional();

            HasOptional(s => s.Editor)
                .WithMany()
                .HasForeignKey(s => s.EditById)
                .WillCascadeOnDelete(false);
        }
    }
}