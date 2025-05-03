using Itmo.Csharp.Microservices.Lab4.Entities.Products;
using Itmo.Csharp.Microservices.Lab4.Entities.Queries;
using Npgsql;
using System.Data;
using System.Runtime.CompilerServices;

namespace Itmo.Csharp.Microservices.Lab4.Repositories.Products;

public class ProductsRepository : IProductsRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public ProductsRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<ProductDto> CreateProductAsync(Product product, CancellationToken cancellationToken)
    {
        const string sql = """
                           INSERT INTO PRODUCTS (PRODUCT_NAME, PRODUCT_PRICE)
                           VALUES (:productName,:productPrice)
                           RETURNING PRODUCT_ID;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("productName", product.Name));
        command.Parameters.Add(new NpgsqlParameter("productPrice", product.Price));

        object? result = await command.ExecuteScalarAsync(cancellationToken);
        long productId = Convert.ToInt64(result);

        return new ProductDto(product.Name, product.Price, productId);
    }

    public async IAsyncEnumerable<ProductDto> SearchProductAsync(
        ProductQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT PRODUCT_ID,PRODUCT_NAME,PRODUCT_PRICE
                           FROM PRODUCTS
                           WHERE
                               (PRODUCT_ID> :Cursor)
                               AND (cardinality(:Ids) =0 OR PRODUCT_NAME=ANY(:Ids))
                               AND (:NamePattern IS NULL OR PRODUCT_NAME LIKE :NamePattern)
                               AND (:MinCost IS NULL OR PRODUCT_PRICE>=:MinCost)
                               AND (:MaxCost IS NULL OR PRODUCT_PRICE<=:MaxCost)
                           LIMIT :PageSize;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("Cursor", query.Cursor));
        command.Parameters.Add(new NpgsqlParameter("Ids", query.Ids));
        command.Parameters.Add(new NpgsqlParameter("NamePattern", query.NamePattern));
        command.Parameters.Add(new NpgsqlParameter("MinCost", query.MinCost));
        command.Parameters.Add(new NpgsqlParameter("MaxCost", query.MaxCost));
        command.Parameters.Add(new NpgsqlParameter("PageSize", query.PageSize));

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new ProductDto(
                reader.GetString("product_name"),
                reader.GetDecimal("product_price"),
                reader.GetInt64("product_id"));
        }
    }

    public async Task<ProductDto?> GetProductByIdAsync(long productId, CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT PRODUCT_ID,PRODUCT_NAME,PRODUCT_PRICE
                           FROM PRODUCTS
                           WHERE PRODUCT_ID=:productId ;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.Add(new NpgsqlParameter("productId", productId));

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            return new ProductDto(
                reader.GetString("product_name"),
                reader.GetDecimal("product_price"),
                reader.GetInt64("product_id"));
        }

        return null;
    }
}