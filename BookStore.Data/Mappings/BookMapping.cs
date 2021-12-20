using BookStore.Domain.Models.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Data.Mappings;

public class BookMapping : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder
            .HasKey(b => b.Id);

        builder
            .Property(b => b.Price)
            .HasPrecision(2);

        builder
            .HasOne(b => b.Author);

        builder
            .HasOne(b => b.Category);
    }
}