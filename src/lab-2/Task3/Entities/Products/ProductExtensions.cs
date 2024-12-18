namespace Itmo.Csharp.Microservices.Lab2.Task3.Entities.Products;

public static class ProductExtensions
{
    public static Product ToProduct(this ProductDto productDto)
    {
        return new Product(
            productDto.Name,
            productDto.Price,
            productDto.Id);
    }
}