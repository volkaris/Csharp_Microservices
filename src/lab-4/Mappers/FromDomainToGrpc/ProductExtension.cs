using Google.Type;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Products;
using products;

namespace Itmo.Csharp.Microservices.Lab4.Mappers.FromDomainToGrpc;

public static class ProductExtension
{
    public static CreateProductRequest ToCreateProductRequest(this Product product)
    {
        return new CreateProductRequest
        {
            Name = product.Name,

            Price = new Money
            {
                DecimalValue = product.Price,
            },
        };
    }

    public static CreateProductResponse ToCreateProductResponse(this ProductDto productDto)
    {
        return new CreateProductResponse
        {
            Name = productDto.Name,

            Price = new Money
            {
                DecimalValue = productDto.Price,
            },

            Id = productDto.Id,
        };
    }

    public static GetProductByIdResponse ToGetProductByIdResponse(this ProductDto productDto)
    {
        return new GetProductByIdResponse
        {
            Name = productDto.Name,

            Price = new Money
            {
                DecimalValue = productDto.Price,
            },

            Id = productDto.Id,
        };
    }
}