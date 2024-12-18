using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Products;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Services.Products;

public interface IProductService
{
    public Task<ProductDto> CreateProductAsync(Product product, CancellationToken cancellationToken);

    public Task<ProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken);
}