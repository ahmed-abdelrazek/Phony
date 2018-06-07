using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class NoteConfig : EntityTypeConfiguration<Model.Note>
    {
        public NoteConfig()
        {
            HasKey(n => n.Id);

            Property(n => n.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(n => n.Notes)
                .IsRequired();

            HasRequired(n => n.Creator)
                .WithMany()
                .HasForeignKey(n => n.CreatedById)
                .WillCascadeOnDelete(false);

            Property(n => n.EditById)
                .IsOptional();

            HasOptional(n => n.Editor)
                .WithMany()
                .HasForeignKey(n => n.EditById)
                .WillCascadeOnDelete(false);
        }
    }
}