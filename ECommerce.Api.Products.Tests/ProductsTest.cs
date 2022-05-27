using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsTest
    {
        [Fact]
        public async Task GetProductsReturnsAllProducts()
        {
            //Dbcontext
            var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts)).Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            //Mapper
            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            //Provider
            var products = await productsProvider.GetProductsAsync();

            Assert.True(products.IsSuccess);
            Assert.True(products.Products.Any());
            Assert.Null(products.ErrorMessage);


        }

        [Fact]
        public async Task GetProductsReturnProductByValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductsReturnProductByValidId)).Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var products = await productsProvider.GetProductByIdAsync(1);

            Assert.True(products.IsSuccess);
            Assert.NotNull(products.Product);
            Assert.True(products.Product.Id == 1);
            Assert.Null(products.ErrorMessage);

        }
        [Fact]
        public async Task GetProductsReturnProductByInvalidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>().UseInMemoryDatabase(nameof(GetProductsReturnProductByInvalidId)).Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var products = await productsProvider.GetProductByIdAsync(-1);

            Assert.False(products.IsSuccess);
            Assert.Null(products.Product);
            Assert.NotNull(products.ErrorMessage);

        }


        private void CreateProducts(ProductsDbContext dbContext)
        {
            for (int i = 1; i <= 10; i++)
            {
                dbContext.Products.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventary = i + 10,
                    Price =  (Decimal)(i * 3.4)
                });
            }
            dbContext.SaveChanges();
        }
    }
}
