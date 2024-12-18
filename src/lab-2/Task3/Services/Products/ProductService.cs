using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Products;
using Itmo.Csharp.Microservices.Lab2.Task3.Repositories.Products;

namespace Itmo.Csharp.Microservices.Lab2.Task3.Services.Products;

public class ProductService : IProductService
{
    private readonly IProductsRepository _repository;

    public ProductService(IProductsRepository repository)
    {
        _repository = repository;
    }

    public Task<ProductDto> CreateProductAsync(Product product, CancellationToken cancellationToken)
    {
        return _repository.CreateProductAsync(product, cancellationToken);
    }

    public Task<ProductDto?> GetProductByIdAsync(long productId, CancellationToken cancellationToken)
    {
        return _repository.GetProductByIdAsync(productId, cancellationToken);
    }
}