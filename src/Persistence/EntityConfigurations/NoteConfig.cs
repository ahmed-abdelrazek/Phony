using System.Data.Entity.ModelConfiguration;

namespace Phony.Persistence.EntityConfigurations
{
    public class NoteConfig : EntityTypeConfiguration<Model.Note>
    {
        public NoteConfig()
        {
            HasKey(c => c.Id);

            Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(i => i.Notes)
                .IsRequired();

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