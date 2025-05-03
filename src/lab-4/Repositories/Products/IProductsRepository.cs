using Itmo.Csharp.Microservices.Lab4.Entities.Products;
using Itmo.Csharp.Microservices.Lab4.Entities.Queries;

namespace Itmo.Csharp.Microservices.Lab4.Repositories.Products;

public interface IProductsRepository
{
    public Task<ProductDto> CreateProductAsync(Product product, CancellationToken cancellationToken);

    public IAsyncEnumerable<ProductDto> SearchProductAsync(ProductQuery query, CancellationToken cancellationToken);

    public Task<ProductDto?> GetProductByIdAsync(long productId, CancellationToken cancellationToken);
}