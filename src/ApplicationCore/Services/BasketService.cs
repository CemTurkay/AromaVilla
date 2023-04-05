using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class BasketService : IBasketService
    {
        private readonly IRepository<Basket> _basketRepo;
        private readonly IRepository<BasketItem> _basketItemRepo;
        private readonly IRepository<Product> _productRepo;
        public BasketService(IRepository<Basket> basketRepo, IRepository<BasketItem> basketItemRepo, IRepository<Product> productRepo)
        {
            _basketRepo = basketRepo;
            _basketItemRepo = basketItemRepo;
            _productRepo = productRepo;
        }
        public async Task<Basket> AddItemToBasketAsync(string buyerId, int productId, int quantity)
        {
            var basket = await GetorCreateBasketAsync(buyerId);
            var basketItem = basket.Items.FirstOrDefault(x => x.ProductId == productId);

            if (basketItem == null)
            {
                basketItem = new BasketItem()
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Product = (await _productRepo.GetByIdAsync(productId))!
                };
                basket.Items.Add(basketItem);
            }
            else
            {
                basketItem.Quantity += quantity;
            }
            await _basketRepo.UpdateAsync(basket);
            return basket;
        }

        public async Task DeleteBasketItemAsync(string buyerId, int productId)
        {
            var basket = await GetorCreateBasketAsync(buyerId);
            var basketItem = basket.Items.FirstOrDefault(x => x.ProductId == productId);
            if (basketItem == null) return;
            await _basketItemRepo.DeleteAsync(basketItem);
            {

            }
        }

        public async Task EmpytBasketAsync(string buyerId)
        {
            var basket = await GetorCreateBasketAsync(buyerId);
            foreach (var item in basket.Items.ToList())
            {
                await _basketItemRepo.DeleteAsync(item);
            }
        }

        public async Task<Basket> GetorCreateBasketAsync(string buyerId)
        {
            var specBasket = new BasketWithItemSpecifation(buyerId);
            var basket = await _basketRepo.FirstOrDefaultAsync(specBasket);

            if (basket == null)
            {
                basket = new Basket() { BuyerId = buyerId };
                await _basketRepo.AddAsync(basket);
            }
            return basket;
        }

        public async Task<Basket> SetQuantitites(string buyerId, Dictionary<int, int> quantites)
        {
            var basket = await GetorCreateBasketAsync(buyerId);
            foreach (var item in basket.Items)
            {
                if (quantites.ContainsKey(item.ProductId))
                {
                    item.Quantity = quantites[item.ProductId];
                    await _basketItemRepo.UpdateAsync(item);
                }
            }
            return basket;
        }

        public async Task TransferBasketAsync(string sourceBuyerId, string destinationBuyerId)
        {
            var specSourceBasket = new BasketWithItemSpecifation(sourceBuyerId);
            var sourceBasket = await _basketRepo.FirstOrDefaultAsync(specSourceBasket);

            if (sourceBasket == null || sourceBasket.Items.Count == 0)
                return;

            var destinationBasket = await GetorCreateBasketAsync(destinationBuyerId);

            foreach (var item in sourceBasket.Items)
            {
                var targetItem = destinationBasket.Items.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (targetItem == null)
                {
                    destinationBasket.Items.Add(new BasketItem()
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }
                else
                {
                    targetItem.Quantity += item.Quantity;
                }
            }

            await _basketRepo.UpdateAsync(destinationBasket);
            await _basketRepo.DeleteAsync(sourceBasket);


        }
    }
}
