using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepo;
        private readonly IBasketService _basketService;

        public OrderService( IBasketService basketService, IRepository<Order> orderRepo)
        {
            _orderRepo = orderRepo;
            _basketService = basketService;
        }
        public async Task<Order> CreateOrderAsync(string buyerId, Adress shippingAdress)
        {
            var basket = await _basketService.GetorCreateBasketAsync(buyerId);
            Order order = new Order()
            {
                ShippingAddres = shippingAdress,
                BuyerId = buyerId,
                Items = basket.Items.Select(x => new OrderItem()
                {
                    PictureUri = x.Product.PictureUri,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    UnitPrice = x.Product.Price
                }).ToList()
            };
            
            
            return await _orderRepo.AddAsync(order);
        }
    }
}
