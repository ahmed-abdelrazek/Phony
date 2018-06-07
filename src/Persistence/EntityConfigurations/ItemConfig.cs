using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class ItemConfig : EntityTypeConfiguration<Model.Item>
    {
        public ItemConfig()
        {
            HasKey(i => i.Id);

            Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(i => i.Image)
                .HasColumnType("image")
                .IsOptional();

            Property(i => i.WholeSalePrice)
                .IsOptional();

            Property(i => i.HalfWholeSalePrice)
                .IsOptional();

            Property(i => i.RetailPrice)
                .IsOptional();

            Property(i => i.CompanyId)
                .IsOptional();

            Property(i => i.SupplierId)
                .IsOptional();

            HasOptional(i => i.Company)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CompanyId)
                .WillCascadeOnDelete(false);

            HasOptional(i => i.Supplier)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.SupplierId)
                .WillCascadeOnDelete(false);

            HasRequired(i => i.Creator)
                .WithMany()
                .HasForeignKey(i => i.CreatedById)
                .WillCascadeOnDelete(false);

            Property(i => i.EditById)
                .IsOptional();

            HasOptional(i => i.Editor)
                .WithMany()
                .HasForeignKey(i => i.EditById)
                .WillCascadeOnDelete(false);

        }
    }
}