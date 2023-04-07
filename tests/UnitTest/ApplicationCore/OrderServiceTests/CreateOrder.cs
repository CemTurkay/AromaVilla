using ApplicationCore.Entities;
using ApplicationCore.Expections;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Ardalis.Specification;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.ApplicationCore.OrderServiceTests
{
    
    public class CreateOrder
    {
        private readonly string _demoBuyerId = "This-Is-A-Random-Buyer-Id";
        private readonly Adress _demoAdress = new Adress()
        {
            City = "London",
            Country = "UK",
            Street = "221B Baker Street",
            ZipCode = "NW1 6XE"
        };

        [Fact]
        public async Task ShoulThrowEmptyBasketExceptionIfBasketIsEmpty()
        {
            var mockBasketRepo = new Mock<IRepository<Basket>>();
            var mockBasketItemRepo = new Mock<IRepository<BasketItem>>();
            var mockProductRepo = new Mock<IRepository<Product>>();
            var mockOrderRepo = new Mock<IRepository<Order>>();

            Basket emptyBasket = new Basket()
            {
                Id = 33,
                BuyerId = _demoBuyerId,
                Items = new List<BasketItem>()
            };

            mockBasketRepo.Setup(b => b.FirstOrDefaultAsync(It.IsAny<Specification<Basket>>())).ReturnsAsync(emptyBasket);

            var basketRepo = mockBasketRepo.Object;
            var basketItemRepo = mockBasketItemRepo.Object;
            var productRepo = mockProductRepo.Object;
            var orderRepo = mockOrderRepo.Object;

            IBasketService basketService = new BasketService(basketRepo, basketItemRepo, productRepo);
            IOrderService orderService = new OrderService(basketService, orderRepo);

            
            //iddat ediyorum (assert) fırtlmalıdır EmptyBasketExpection hatasını
            // orderService
            await Assert.ThrowsAsync<EmptyBasketExpection> (async () =>
                await orderService.CreateOrderAsync(_demoBuyerId, _demoAdress));

        }
    }
}
