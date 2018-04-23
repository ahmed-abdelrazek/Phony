using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class CompanyMoveConfig : EntityTypeConfiguration<Model.CompanyMove>
    {
        public CompanyMoveConfig()
        {
            HasKey(c => c.Id);

            HasRequired(c => c.Company)
                .WithMany()
                .HasForeignKey(c => c.CompanyId)
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