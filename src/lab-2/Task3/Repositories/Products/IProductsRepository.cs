using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Products;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Queries;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Repositories.Products;

public interface IProductsRepository
{
    public Task<ProductDto> CreateProductAsync(Product product, CancellationToken cancellationToken);

    public IAsyncEnumerable<ProductDto> SearchProductAsync(ProductQuery query, CancellationToken cancellationToken);

    public Task<ProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken);
}