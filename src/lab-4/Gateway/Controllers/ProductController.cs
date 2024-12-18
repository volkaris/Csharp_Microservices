using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Products;
using Itmo.Csharp.Microservices.Lab4.Mappers.FromDomainToGrpc;
using Itmo.Csharp.Microservices.Lab4.Mappers.FromGrpcToDomain;
using Microsoft.AspNetCore.Mvc;
using products;
using Swashbuckle.AspNetCore.Annotations;

namespace Itmo.Csharp.Microservices.Lab4.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductsService.ProductsServiceClient _client;

    public ProductController(ProductsService.ProductsServiceClient client)
    {
        _client = client;
    }

    [HttpPost]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns when product successfully created")]
    public async Task<ActionResult<ProductDto>> CreateProductAsync(
        [FromBody] Product product,
        CancellationToken cancellationToken)
    {
        CreateProductResponse? createProductResponse = await _client.CreateProductAsync(product.ToCreateProductRequest(), cancellationToken: cancellationToken);

        return Ok(createProductResponse.ToProductDto());
    }

    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns if product was found")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns when product wasn't found")]
    public async Task<ActionResult<ProductDto>> GetProductByIdAsync([FromQuery] long productId, CancellationToken cancellationToken)
    {
        GetProductByIdResponse res = await _client.GetProductByIdAsync(new GetProductByIdRequest { Id = productId }, cancellationToken: cancellationToken);

        return Ok(res.ToProductDto());
    }
}