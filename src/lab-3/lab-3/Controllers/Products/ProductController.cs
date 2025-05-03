using Itmo.Csharp.Microservices.Lab2.Task3.Entities.Products;
using Itmo.Csharp.Microservices.Lab2.Task3.Services.Products;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Itmo.Csharp.Microservices.Lab3.Controllers.Products;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [SwaggerResponse(StatusCodes.Status200OK, "Returns when product successfully created")]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProductAsync(
        [FromBody] Product product,
        CancellationToken cancellationToken)
    {
        ProductDto productDto = await _productService.CreateProductAsync(product, cancellationToken);

        return Ok(productDto);
    }

    [SwaggerResponse(StatusCodes.Status200OK, "Returns if product was found")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns when product wasn't found")]
    [HttpGet]
    public async Task<ActionResult<ProductDto>> GetProductByIdAsync(
        [FromQuery] int productId,
        CancellationToken cancellationToken)
    {
        ProductDto? productDto = await _productService.GetProductByIdAsync(productId, cancellationToken);

        if (productDto is null)
        {
            return NotFound($"Product with id {productId} wasn't found");
        }

        return Ok(productDto);
    }
}