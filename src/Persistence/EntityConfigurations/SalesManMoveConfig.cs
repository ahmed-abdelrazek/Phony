using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class SalesManMoveConfig : EntityTypeConfiguration<Model.SalesManMove>
    {
        public SalesManMoveConfig()
        {
            HasKey(s => s.Id);

            HasRequired(s => s.SalesMan)
                .WithMany(s=> s.SalesMenMoves)
                .HasForeignKey(s => s.SalesManId)
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