using Xunit;
using webshop_barbie.Service;
using webshop_barbie.Models;
using webshop_barbie.Repository.Interfaces;
using webshop_barbie.DTOs;
using Moq;
using System.Threading.Tasks;
namespace webshop_barbie.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task CheckStockAsync_ProductExists_EnoughStock_ReturnsTrue()
        {

            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Product { Id = 1, Stock = 10 });

            var service = new ProductService(mockRepo.Object);

            var result = await service.CheckStockAsync(1, 5);

            Assert.True(result.IsAvailable);
            Assert.Equal(10, result.AvailableStock);
        }

        [Fact]
        public async Task CheckStockAsync_ProductExists_NotEnoughStock_ReturnsFalse()
        {
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Product { Id = 1, Stock = 3 });

            var service = new ProductService(mockRepo.Object);

            var result = await service.CheckStockAsync(1, 5);

            Assert.False(result.IsAvailable);
            Assert.Equal(3, result.AvailableStock);
        }

        [Fact]
        public async Task CheckStockAsync_ProductDoesNotExist_ThrowsKeyNotFoundException()
        {
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Product)null);

            var service = new ProductService(mockRepo.Object);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.CheckStockAsync(1, 5));
        }
    }
}