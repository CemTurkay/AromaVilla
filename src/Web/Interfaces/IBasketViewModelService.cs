﻿namespace Web.Interfaces
{
    public interface IBasketViewModelService
    {
        Task<BasketViewModel> GetBasketItemViewModelAsync();

        Task<BasketViewModel> AddItemToBasketAsync(int productId, int quantity);

        Task EmptyBasketAsync();
        Task DeleteBasketItemAsync(int productId);
        Task UpdateBasketAsync(Dictionary<int, int> quantites);

        Task TransferBasketAsync();

    }
}
