using Itmo.Csharp.Microservices.Lab4.Entities.Products;

namespace Itmo.Csharp.Microservices.Lab4.Services.Products;

public interface IProductService
{
    public Task<ProductDto> CreateProductAsync(Product product, CancellationToken cancellationToken);

    public Task<ProductDto?> GetProductByIdAsync(long productId, CancellationToken cancellationToken);
}