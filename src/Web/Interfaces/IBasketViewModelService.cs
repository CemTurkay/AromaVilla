namespace Web.Interfaces
{
    public interface IBasketViewModelService
    {
        Task<BasketViewModel> GetBasketItemViewModelAsync();

        Task<BasketViewModel> AddItemToBasketAsync(int productId, int quantity);

    }
}
