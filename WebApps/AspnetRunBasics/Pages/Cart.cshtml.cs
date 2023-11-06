using System;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class CartModel : PageModel
    {
        private readonly IBasketService _basketService;

        public CartModel(IBasketService cartRepository)
        {
            _basketService = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public BasketModel Cart { get; set; } = new BasketModel();        

        public async Task<IActionResult> OnGetAsync()
        {
            var userName = "swn";
            Cart = await _basketService.GetBasket(userName);            

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string productID)
        {
            var userName = "swn";
            var basket = await _basketService.GetBasket(userName);

            var item = basket.Items.Find(i => i.ProductId == productID);
            basket.Items.Remove(item);

            var basketUpdated = await _basketService.UpdateBasket(basket);
            return RedirectToPage();
        }
    }
}