using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketViewModelService _basketViewModelService;

        public BasketController(IBasketViewModelService basketViewModelService)
        {
            _basketViewModelService = basketViewModelService;
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _basketViewModelService.GetBasketItemViewModelAsync();
            return View(vm);
        }

        [Authorize]
        public async Task<IActionResult> CheckOut()
        {
            var basket = await _basketViewModelService.GetBasketItemViewModelAsync();
            var vm = new CheckoutViewModel()
            {
                Basket = basket
            };
            return View(vm);
        }


        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(CheckoutViewModel vm)
        {

            if (ModelState.IsValid)
            {
                await _basketViewModelService.CheckoutAsync(vm.Street, vm.City, vm.Street, vm.Country, vm.ZipCode);
                return RedirectToAction("OrderConfirmed");
            }
            vm.Basket = await _basketViewModelService.GetBasketItemViewModelAsync();
            return View(vm);
        }

        [Authorize]
        public async Task<IActionResult> OrderConfirmed()
        {

            return View();
        }

        public async Task<IActionResult> AddToBasket(int productId, int quantity =1)
        {
            var vm = await _basketViewModelService.AddItemToBasketAsync(productId, quantity);
            return Json(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Empty()
        {
            await _basketViewModelService.EmptyBasketAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int productId)
        {
            await _basketViewModelService.DeleteBasketItemAsync(productId);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([ModelBinder(Name = "quantites")] Dictionary<int, int> quantites)
        {
            await _basketViewModelService.UpdateBasketAsync(quantites);
            return RedirectToAction(nameof(Index));
        }
    }
}
