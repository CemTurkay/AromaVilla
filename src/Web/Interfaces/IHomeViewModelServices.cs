namespace Web.Interfaces
{
    public interface IHomeViewModelServices
    {
        Task<HomeViewModel> GetHomeViewModelAsync(int? categoryId, int? brandId, int pageId = 1);
    }
}
