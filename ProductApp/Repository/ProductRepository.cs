using Dapper;
using Microsoft.Data.SqlClient;
using ProductApp.Models;
using System.Data;

namespace ProductApp.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ProductAppConnectionString");
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<Product>("SELECT * FROM Products");
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryFirstOrDefaultAsync<Product>("SELECT * FROM Products WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task<int> AddProductAsync(Product product)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price)";
                return await db.ExecuteAsync(sqlQuery, product);
            }
        }

        public async Task<int> UpdateProductAsync(Product product)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Products SET Name = @Name, Price = @Price WHERE Id = @Id";
                return await db.ExecuteAsync(sqlQuery, product);
            }
        }

        public async Task<int> DeleteProductAsync(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Products WHERE Id = @Id";
                return await db.ExecuteAsync(sqlQuery, new { Id = id });
            }
        }
    }
}
