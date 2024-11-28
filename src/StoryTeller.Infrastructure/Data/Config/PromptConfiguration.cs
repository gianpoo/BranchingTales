namespace StoryTeller.Infrastructure.Data.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryTeller.Core.PromptAggregate;

public class PromptConfiguration : IEntityTypeConfiguration<Prompt>
{
    public void Configure(EntityTypeBuilder<Prompt> builder)
    {
        builder.Property(p => p.Text)
            .IsRequired()
            .HasMaxLength(1000);
    }
} 