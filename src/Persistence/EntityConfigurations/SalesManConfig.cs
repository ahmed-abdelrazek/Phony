using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class SalesManConfig : EntityTypeConfiguration<Model.SalesMan>
    {
        public SalesManConfig()
        {
            HasKey(s => s.Id);

            Property(s => s.Name)
                    .IsRequired()
                    .HasMaxLength(50);

            HasIndex(s => s.Name)
                    .IsUnique();

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