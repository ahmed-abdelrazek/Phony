using System.Data.Entity.ModelConfiguration;

namespace Phony.EntityConfigurations
{
    public class SupplierConfig : EntityTypeConfiguration<Model.Supplier>
    {
        public SupplierConfig()
        {
            HasKey(s => s.Id);

            Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(50);

            HasIndex(s => s.Name)
                .IsUnique();

            Property(s => s.Image)
                .HasColumnType("image")
                .IsOptional();

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
