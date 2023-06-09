﻿using System.Security.Claims;
using Web.Interfaces;

namespace Web.Services
{
    public class BasketViewModelService : IBasketViewModelService
    {
        private readonly IBasketService _basketService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderService _orderService;

        //-----------------------COOKIE-----------------------------------//
        private HttpContext? HttpContext => _httpContextAccessor.HttpContext;
        private string? UserId => HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        private string? AnonId => HttpContext?.Request.Cookies[Constants.BASKET_COOKIENAME];
        private string BuyerId => UserId ?? AnonId ?? CreateAnonymousId();

        private string _createdAnonId = null!;
        private string CreateAnonymousId()
        {
            if (_createdAnonId == null)
            {
                _createdAnonId = Guid.NewGuid().ToString();
                HttpContext?.Response.Cookies.Append(Constants.BASKET_COOKIENAME, _createdAnonId, new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(14),
                    IsEssential = true
                });
            }

            return _createdAnonId;
        }
        //---------------COOKIE------------------//
        public BasketViewModelService(IBasketService basketService, IHttpContextAccessor httpContextAccessor, IOrderService orderService)
        {
            _basketService = basketService;
            _httpContextAccessor = httpContextAccessor;
            _orderService = orderService;
        }
        public async Task<BasketViewModel> AddItemToBasketAsync(int productId, int quantity)
        {
            var basket = await _basketService.AddItemToBasketAsync(BuyerId, productId, quantity);
            return basket.ToBasketViewModel();
        }

        public async Task<BasketViewModel> GetBasketItemViewModelAsync()
        {
            var basket = await _basketService.GetorCreateBasketAsync(BuyerId);
            return basket.ToBasketViewModel();
        }

        public async Task EmptyBasketAsync()
        {
            await _basketService.EmpytBasketAsync(BuyerId);
        }

        public async Task DeleteBasketItemAsync(int productId)
        {
            await _basketService.DeleteBasketItemAsync(BuyerId, productId);
        }

        public async Task UpdateBasketAsync(Dictionary<int, int> quantites)
        {
           await _basketService.SetQuantitites(BuyerId, quantites);
        }

        public async Task TransferBasketAsync()
        {
            if (AnonId == null || UserId == null) return;
            await _basketService.TransferBasketAsync(AnonId, UserId);
            HttpContext?.Response.Cookies.Delete(Constants.BASKET_COOKIENAME);
            
        }

        public async Task CheckoutAsync(string street, string city, string? state, string country, string zipCode)
        {
            var shippingAdress = new Adress(street, city, state, country, zipCode);
            await _orderService.CreateOrderAsync(BuyerId, shippingAdress);
            await _basketService.EmpytBasketAsync(BuyerId);
        }
    }
}
