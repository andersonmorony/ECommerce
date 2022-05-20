using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDbContext _dbContext;
        private readonly ILogger<ProductsProvider> _logger;
        private readonly IMapper _mapper;

        public ProductsProvider(ProductsDbContext dbContext, ILogger<ProductsProvider> logger, IMapper mapper )
        {
            this._dbContext = dbContext;
            this._logger = logger;
            this._mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if(!_dbContext.Products.Any())
            {
                _dbContext.Products.Add(new Db.Product() { Id = 1, Name = "Keyboard", Price = 20, Inventary = 100 });
                _dbContext.Products.Add(new Db.Product() { Id = 2, Name = "Mouse", Price = 5, Inventary = 200 });
                _dbContext.Products.Add(new Db.Product() { Id = 3, Name = "Monitor", Price = 100, Inventary = 150 });
                _dbContext.Products.Add(new Db.Product() { Id = 4, Name = "CPU", Price = 200, Inventary = 1000 });
                _dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Product> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await _dbContext.Products.ToListAsync();

                if (products != null && products.Any())
                {
                    var results = _mapper.Map<IEnumerable<Db.Product>, IEnumerable<Models.Product>>(products);

                    return (true, results, null);
                }

                return (false, null, "Not found");

            } catch(Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Product Product, string ErrorMessage)> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(product => product.Id == id);

                if (product != null)
                {
                    var results = _mapper.Map<Db.Product, Models.Product>(product);

                    return (true, results, null);
                }

                return (false, null, "Not found");

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
