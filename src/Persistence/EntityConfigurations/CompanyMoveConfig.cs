using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class CompanyMoveConfig : EntityTypeConfiguration<Model.CompanyMove>
    {
        public CompanyMoveConfig()
        {
            HasKey(c => c.Id);

            HasRequired(c => c.Company)
                .WithMany(c=> c.CompaniesMoves)
                .HasForeignKey(c => c.CompanyId)
                .WillCascadeOnDelete(true);

            HasRequired(c => c.Creator)
                .WithMany()
                .HasForeignKey(c => c.CreatedById)
                .WillCascadeOnDelete(false);

            Property(c => c.EditById)
                .IsOptional();

            HasOptional(c => c.Editor)
                .WithMany()
                .HasForeignKey(c => c.EditById)
                .WillCascadeOnDelete(false);
        }
    }
}