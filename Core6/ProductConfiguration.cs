using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Core6;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
        builder.HasKey(x => x.Id)
            .IsClustered(true);

        builder.Property(x => x.Id).HasConversion<ProductId.EfCoreValueConverter>();
        //builder.Property(x => x.Id).HasConversion(
        //    productId => productId.Id, 
        //    value => new ProductId2(value));


    }
}
