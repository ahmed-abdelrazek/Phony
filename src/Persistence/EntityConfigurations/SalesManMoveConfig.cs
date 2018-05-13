using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class SalesManMoveConfig : EntityTypeConfiguration<Model.SalesManMove>
    {
        public SalesManMoveConfig()
        {
            HasKey(c => c.Id);

            HasRequired(c => c.SalesMan)
                .WithMany(s=> s.SalesMenMoves)
                .HasForeignKey(c => c.SalesManId)
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