using Itmo.Csharp.Microservices.Lab4.Entities.Products;
using products;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.MyGrpcService.FromGrpcToDomain;

public static class ProductExtension
{
    public static ProductDto ToProductDto(this CreateProductResponse response)
    {
        return new ProductDto(response.Name, response.Price.DecimalValue, response.Id);
    }

    public static ProductDto ToProductDto(this GetProductByIdResponse response)
    {
        return new ProductDto(response.Name, response.Price.DecimalValue, response.Id);
    }

    public static Product ToProduct(this CreateProductRequest request)
    {
        return new Product(request.Name, request.Price.DecimalValue);
    }
}