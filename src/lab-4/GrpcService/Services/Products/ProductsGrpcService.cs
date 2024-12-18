using Grpc.Core;
using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Products;
using Itmo.Csharp.Microservices.Lab2.Task3.Services.Products;
using Itmo.Csharp.Microservices.Lab4.Mappers.FromDomainToGrpc;
using Itmo.Csharp.Microservices.Lab4.Mappers.FromGrpcToDomain;
using products;

namespace Itmo.Csharp.Microservices.Lab4.GrpcService.Services.Products;

public class ProductsGrpcService : ProductsService.ProductsServiceBase
{
    private readonly IProductService _service;

    public ProductsGrpcService(IProductService service)
    {
        _service = service;
    }

    public override async Task<CreateProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
    {
        ProductDto productDto = await _service.CreateProductAsync(request.ToProduct(), context.CancellationToken);

        return productDto.ToCreateProductResponse();
    }

    public override async Task<GetProductByIdResponse> GetProductById(GetProductByIdRequest request, ServerCallContext context)
    {
        ProductDto? productDto = await _service.GetProductByIdAsync(request.Id, context.CancellationToken);

        if (productDto is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "product not found"));
        }

        return productDto.ToGetProductByIdResponse();
    }
}