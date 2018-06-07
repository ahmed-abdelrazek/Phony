using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class TreasuryConfig : EntityTypeConfiguration<Model.Treasury>
    {
        public TreasuryConfig()
        {
            HasKey(t => t.Id);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            HasIndex(t => t.Name)
                .IsUnique();

            HasRequired(t => t.Store)
                .WithMany(s => s.Treasuries)
                .HasForeignKey(t => t.StoreId)
                .WillCascadeOnDelete(false);

            HasRequired(t => t.Creator)
                .WithMany()
                .HasForeignKey(t => t.CreatedById)
                .WillCascadeOnDelete(false);

            Property(t => t.EditById)
                .IsOptional();

            HasOptional(t => t.Editor)
                .WithMany()
                .HasForeignKey(t => t.EditById)
                .WillCascadeOnDelete(false);
        }
    }
}