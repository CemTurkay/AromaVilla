using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IBasketService
    {
        Task<Basket> GetorCreateBasketAsync(string buyerId);
        Task<Basket> AddItemToBasketAsync(string buyerId, int productId, int quantity);
        Task EmpytBasketAsync(string buyerId);
        Task DeleteBasketItemAsync(string buyerId, int productId);
        Task<Basket> SetQuantitites(string buyerId, Dictionary<int, int> quantities);
        Task TransferBasketAsync(string sourceBuyerId, string destinationBuyerId);


    }
}
